import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChatUser, ChatMessage, CreateMessageRequest, SearchUser } from '../../models/chat.models';
import { ApiService } from '../../services/api.service';
import { UserService } from '../../services/user.service';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { Subject, Subscription, interval } from 'rxjs';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [CommonModule, FormsModule, DatePipe],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.scss'
})
export class ChatComponent implements OnInit, OnDestroy {
  // Maximum message length constant
  readonly MAX_MESSAGE_LENGTH = 500;
  private readonly WARNING_THRESHOLD = 0.8; // Show warning at 80% of limit
  
  // Updated properties to use new data models
  chats: ChatUser[] = [];
  selectedUserId: string | null = null;
  selectedChat: ChatUser | null = null;
  messages: ChatMessage[] = [];
  newMessage: string = '';
  
  // Validation properties
  messageValidationError: string = '';
  
  // Modal properties
  showNewChatModal: boolean = false;
  searchText: string = '';
  searchResults: SearchUser[] = [];
    // Loading states
  isLoadingChats: boolean = false;
  isLoadingMessages: boolean = false;
  isSendingMessage: boolean = false;
  isSearchingUsers: boolean = false;

  // Error handling
  errorMessage: string = '';
  hasError: boolean = false;
  // Search functionality
  private searchSubject = new Subject<string>();
  private searchSubscription?: Subscription;

  // Polling functionality
  private pollingSubscription?: Subscription;
  private isVisible: boolean = true;  private pollingInterval: number = 30000; // 30 seconds

  constructor(private apiService: ApiService, private userService: UserService) {}
  
  // Called when component starts
  ngOnInit(): void {
    this.loadChats();
    this.setupUserSearch();
    this.startPolling();
    this.setupVisibilityHandling();
  }
  ngOnDestroy(): void {
    if (this.searchSubscription) {
      this.searchSubscription.unsubscribe();
    }
    this.stopPolling();
  }

  private setupUserSearch(): void {
    this.searchSubscription = this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap(query => {
        if (!query.trim()) {
          this.searchResults = [];
          this.isSearchingUsers = false;
          return [];
        }
        this.isSearchingUsers = true;
        return this.apiService.searchUsers(query);
      })
    ).subscribe({
      next: (users: SearchUser[]) => {
        this.searchResults = users;
        this.isSearchingUsers = false;
      },
      error: (error) => {
        console.error('User search failed:', error);
        this.searchResults = [];
        this.isSearchingUsers = false;
      }
    });
  }

  private startPolling(): void {
    this.pollingSubscription = interval(this.pollingInterval).subscribe(() => {
      if (this.isVisible && !this.isLoadingChats) {
        this.refreshChats();
        
        // Refresh current conversation if open
        if (this.selectedUserId && !this.isLoadingMessages) {
          this.refreshCurrentConversation();
        }
      }
    });
  }

  private stopPolling(): void {
    if (this.pollingSubscription) {
      this.pollingSubscription.unsubscribe();
    }
  }

  private refreshChats(): void {
    this.apiService.getChats().subscribe({
      next: (response: ChatUser[]) => {
        this.updateChatsWithNewData(response);
      },
      error: (error) => {
        console.error('Failed to refresh chats:', error);
      }
    });
  }

  private refreshCurrentConversation(): void {
    if (!this.selectedUserId) return;
    
    this.apiService.getChatMessages(this.selectedUserId).subscribe({
      next: (response: ChatMessage[]) => {
        this.updateMessagesWithNewData(response);
      },
      error: (error) => {
        console.error('Failed to refresh messages:', error);
      }
    });
  }

  private updateChatsWithNewData(newChats: ChatUser[]): void {
    // Handle new chats that aren't in the current list
    if (newChats.length > this.chats.length) {
      // Find any truly new chats (not just updated existing ones)
      const existingChatIds = this.chats.map(chat => chat.userId);
      const brandNewChats = newChats.filter(chat => !existingChatIds.includes(chat.userId));
      
      // Add any brand new chats to the top of the list
      if (brandNewChats.length > 0) {
        console.log('New chats detected:', brandNewChats);
      }
    }
    
    // Update all chats with latest data (this will update lastMessage for existing chats)
    this.chats = newChats;
    
    // If we have a selected chat, make sure it's still up to date with the new data
    if (this.selectedUserId) {
      const updatedSelectedChat = this.chats.find(chat => chat.userId === this.selectedUserId);
      if (updatedSelectedChat) {
        this.selectedChat = updatedSelectedChat;
      }
    }
  }

  private updateMessagesWithNewData(newMessages: ChatMessage[]): void {
    // Compare with existing messages and add new ones
    const currentMessageIds = this.messages.map(m => m.id);
    const newMessagesOnly = newMessages.filter(m => !currentMessageIds.includes(m.id));
    
    if (newMessagesOnly.length > 0) {
      this.messages = [...this.messages, ...newMessagesOnly];
      // Scroll to bottom if new messages
      this.scrollToBottom();
    }
  }

  private setupVisibilityHandling(): void {
    document.addEventListener('visibilitychange', () => {
      this.isVisible = !document.hidden;
      
      if (this.isVisible) {
        // Refresh immediately when returning to visible
        this.refreshChats();
        if (this.selectedUserId) {
          this.refreshCurrentConversation();
        }
      }
    });
  }

  private scrollToBottom(): void {
    setTimeout(() => {
      const container = document.querySelector('.messages-container');
      if (container) {
        container.scrollTop = container.scrollHeight;
      }
    }, 100);
  }

  // Load chat data from API (replacing mock data)
  loadChats(): void {
    this.isLoadingChats = true;
    this.clearError();
    
    this.apiService.getChats().subscribe({
      next: (response: ChatUser[]) => {
        this.chats = response;
        this.isLoadingChats = false;
      },
      error: (error) => {
        this.handleError('Load chats', error);
        this.isLoadingChats = false;
      }    });
  }
  
  // Load messages for a specific user conversation
  loadChatMessages(userId: string): void {
    this.isLoadingMessages = true;
    this.clearError();
    
    this.apiService.getChatMessages(userId).subscribe({
      next: (response: ChatMessage[]) => {
        this.messages = response;
        this.isLoadingMessages = false;
      },
      error: (error) => {
        this.handleError('Load messages', error);
        this.isLoadingMessages = false;
      }
    });
  }

  // Open the new chat modal
  openNewChatModal(): void {
    this.showNewChatModal = true;
    this.searchText = '';
    this.searchResults = []; // Reset search results
  }

  // Close the new chat modal
  closeNewChatModal(): void {
    this.showNewChatModal = false;
    this.searchText = '';
  }
  // Search through users using API (replacing mock search)
  searchUsers(): void {
    this.searchSubject.next(this.searchText);
  }

  // Start chat with selected user (updated for new data structure)
  startChatWithUser(user: SearchUser): void {
    // Check if chat already exists
    const existingChat = this.chats.find(chat => chat.userEmail === user.emailAddress);
    
    if (existingChat) {
      // Select existing chat
      this.selectChat(existingChat.userId);
      this.closeNewChatModal();
      return;
    }

    // Create new chat user object with first message placeholder
    const newChatUser: ChatUser = {
      userId: user.id,
      userName: user.firstName,
      userSurname: user.surname,
      userEmail: user.emailAddress,
      lastMessage: '(Start a new conversation)',
      lastMessageTime: new Date()
    };

    // Add to chat list
    this.chats.unshift(newChatUser);
    
    // Select the new chat
    this.selectChat(newChatUser.userId);
    this.closeNewChatModal();
  }
  // When user clicks on a chat in the list (updated for new data structure)
  selectChat(userId: string): void {
    this.selectedUserId = userId;
    this.selectedChat = this.chats.find(chat => chat.userId === userId) || null;
    if (this.selectedChat) {
      this.loadChatMessages(userId);
    }  }
  
  // Send a message in the current chat (updated for new data structure)
  sendMessage(): void {
    // Validate the message before sending
    this.validateMessageInput();
    
    // Exit if message is invalid, empty, no user selected, or already sending
    if (this.messageValidationError || !this.newMessage.trim() || 
        !this.selectedUserId || this.isSendingMessage) {
      return;
    }

    const messageContent = this.newMessage.trim();
    const messageRequest: CreateMessageRequest = {
      content: messageContent
    };

    // Optimistic UI update
    const optimisticMessage: ChatMessage = {
      id: `temp-${Date.now()}`, // Temporary ID
      userId: this.getCurrentUserId(),
      userName: this.getCurrentUserName(),
      userSurname: '',
      userEmail: this.getCurrentUserEmail(),
      content: messageContent,
      dateCreated: new Date()
    };

    // Optimistically update chat preview immediately
    if (this.selectedChat) {
      this.selectedChat.lastMessage = messageContent;
      this.selectedChat.lastMessageTime = new Date();
    }

    this.messages.push(optimisticMessage);
    this.newMessage = '';
    this.isSendingMessage = true;

    this.apiService.sendMessage(this.selectedUserId, messageRequest).subscribe({
      next: (messageId: string) => {
        // Update the optimistic message with real ID
        const messageIndex = this.messages.findIndex(m => m.id === optimisticMessage.id);
        if (messageIndex !== -1) {
          this.messages[messageIndex].id = messageId;
        }
        this.isSendingMessage = false;
        
        // Update chat list order (move to top)
        this.updateChatListOrder();
      },      error: (error) => {
        this.handleError('Send message', error);
        // Remove optimistic message on error
        this.messages = this.messages.filter(m => m.id !== optimisticMessage.id);
        this.newMessage = messageContent; // Restore message for retry
        this.isSendingMessage = false;
      }
    });
  }

  // Public method for template usage
  getCurrentUserId(): string {
    const user = this.userService.getCurrentUser();
    return user?.id || '';
  }

  getRoleDescription(role: number): string {
    const roles: { [key: number]: string } = {
      1: 'Administrator',
      2: 'Lecturer', 
      3: 'Tutor',
      4: 'Student'
    };
    return roles[role] || 'Unknown';
  }

  // Error handling methods
  private handleError(operation: string, error: any): void {
    console.error(`${operation} failed:`, error);
    this.hasError = true;
    this.errorMessage = `Failed to ${operation.toLowerCase()}. Please try again.`;
    
    // Clear error after 5 seconds
    setTimeout(() => {
      this.hasError = false;
      this.errorMessage = '';
    }, 5000);
  }

  private clearError(): void {
    this.hasError = false;
    this.errorMessage = '';
  }

  // Retry method
  retryOperation(): void {
    this.clearError();
    this.loadChats();
  }

  // Helper methods
  private getCurrentUserName(): string {
    const user = this.userService.getCurrentUser();
    return user?.name?.split(' ')[0] || '';
  }
  private getCurrentUserEmail(): string {
    const user = this.userService.getCurrentUser();
    return user?.email || '';
  }
  
  private updateChatListOrder(): void {
    if (this.selectedChat && this.messages.length > 0) {
      // Get the latest message for this chat
      const latestMessage = this.messages[this.messages.length - 1];
      
      // Update the selected chat with latest message information
      this.selectedChat.lastMessage = latestMessage.content;
      this.selectedChat.lastMessageTime = latestMessage.dateCreated;
      
      // Move selected chat to top of list      this.chats = [this.selectedChat, ...this.chats.filter(c => c.userId !== this.selectedUserId)];
    }
  }
  
  // Validate message input (called from UI on input event)
  validateMessageInput(): void {
    const messageLength = this.newMessage.length;
    
    // Check if message exceeds max length
    if (messageLength > this.MAX_MESSAGE_LENGTH) {
      this.messageValidationError = `Message exceeds maximum length of ${this.MAX_MESSAGE_LENGTH} characters.`;
      return;
    }
    
    // Check if message is empty when trimmed
    if (this.newMessage.trim().length === 0 && this.newMessage.length > 0) {
      this.messageValidationError = 'Message cannot contain only whitespace.';
      return;
    }
    
    // Clear validation error if the message is valid    this.messageValidationError = '';
  }
  
  // Check if message length is approaching the limit (for UI warning)
  isApproachingLimit(): boolean {
    return this.newMessage.length > this.MAX_MESSAGE_LENGTH * this.WARNING_THRESHOLD &&           this.newMessage.length <= this.MAX_MESSAGE_LENGTH;
  }
  
  // Check if message length exceeds the limit
  isLimitExceeded(): boolean {
    return this.newMessage.length > this.MAX_MESSAGE_LENGTH;
  }
}

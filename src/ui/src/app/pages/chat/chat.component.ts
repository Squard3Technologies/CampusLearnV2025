import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';

// Simple message structure
interface ChatMessage {
  content: string;
  timestamp: Date;
  sender: 'user' | 'system';
}

// Chat conversation structure
interface Chat {
  id: string;
  title: string;
  timestamp: Date;
  lastMessage: string;
  messages: ChatMessage[];
}

// Simple user structure for search
interface User {
  id: string;
  name: string;
  email: string;
  role: string;
}

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [CommonModule, FormsModule, DatePipe],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.scss'
})
export class ChatComponent implements OnInit {
  // List of all chat conversations
  chats: Chat[] = [];
  // Currently selected chat
  selectedChatId: string | null = null;
  selectedChat: Chat | null = null;
  // Text being typed by the user
  newMessage: string = '';

  // Modal properties
  showNewChatModal: boolean = false;
  searchText: string = '';
  allUsers: User[] = [];
  filteredUsers: User[] = [];

  // Called when component starts
  ngOnInit(): void {
    this.loadChats();
    this.loadUsers();
  }

  // Load example chat data
  loadChats(): void {
    // Sample data - in a real app, this would come from a database
    this.chats = [
      {
        id: '1',
        title: 'Course Questions',
        timestamp: new Date(),
        lastMessage: 'Can you explain the last lecture?',
        messages: [
          {
            content: 'Hello, can you help me with the course material?',
            timestamp: new Date(Date.now() - 3600000), // 1 hour ago
            sender: 'user'
          },
          {
            content: 'Of course! What do you need help with?',
            timestamp: new Date(Date.now() - 3500000),
            sender: 'system'
          },
          {
            content: 'Can you explain the last lecture?',
            timestamp: new Date(),
            sender: 'user'
          }
        ]
      },
      {
        id: '2',
        title: 'Assignment Help',
        timestamp: new Date(Date.now() - 86400000), // 1 day ago
        lastMessage: 'Thanks for the explanation!',
        messages: [
          {
            content: 'I need help with assignment 3',
            timestamp: new Date(Date.now() - 90000000),
            sender: 'user'
          },
          {
            content: 'What specific part are you stuck on?',
            timestamp: new Date(Date.now() - 89000000),
            sender: 'system'
          },
          {
            content: 'Thanks for the explanation!',
            timestamp: new Date(Date.now() - 86400000),
            sender: 'user'
          }
        ]
      }
    ];
  }

  // Load example user data for search
  loadUsers(): void {
    // Sample users - in a real app, this would come from a database
    this.allUsers = [
      { id: '1', name: 'John Smith', email: 'john.smith@university.edu', role: 'student' },
      { id: '2', name: 'Mary Johnson', email: 'mary.johnson@university.edu', role: 'tutor' },
      { id: '3', name: 'David Brown', email: 'david.brown@university.edu', role: 'student' },
      { id: '4', name: 'Sarah Wilson', email: 'sarah.wilson@university.edu', role: 'admin' },
      { id: '5', name: 'Mike Davis', email: 'mike.davis@university.edu', role: 'student' },
      { id: '6', name: 'Lisa Garcia', email: 'lisa.garcia@university.edu', role: 'tutor' },
      { id: '7', name: 'Tom Miller', email: 'tom.miller@university.edu', role: 'student' },
      { id: '8', name: 'Anna Taylor', email: 'anna.taylor@university.edu', role: 'tutor' }
    ];
    this.filteredUsers = this.allUsers; // Start with all users visible
  }

  // Open the new chat modal
  openNewChatModal(): void {
    this.showNewChatModal = true;
    this.searchText = '';
    this.filteredUsers = this.allUsers; // Reset to show all users
  }

  // Close the new chat modal
  closeNewChatModal(): void {
    this.showNewChatModal = false;
    this.searchText = '';
  }

  // Search through users (basic raw sorting)
  searchUsers(): void {
    // If search is empty, show all users
    if (!this.searchText || this.searchText.trim() === '') {
      this.filteredUsers = this.allUsers;
      return;
    }

    // Convert search text to lowercase for case-insensitive search
    const searchLower = this.searchText.toLowerCase();

    // Create empty array for results
    this.filteredUsers = [];    // Loop through all users and check if they match
    for (let i = 0; i < this.allUsers.length; i++) {
      const user = this.allUsers[i];

      // Check if name, email, or role contains search text
      const nameMatch = user.name.toLowerCase().indexOf(searchLower) !== -1;
      const emailMatch = user.email.toLowerCase().indexOf(searchLower) !== -1;
      const roleMatch = user.role.toLowerCase().indexOf(searchLower) !== -1;

      // If any field matches, add to results
      if (nameMatch || emailMatch || roleMatch) {
        this.filteredUsers.push(user);
      }
    }
  }

  // Start chat with selected user
  startChatWithUser(user: User): void {
    const newChatId = Date.now().toString(); // Use timestamp as ID
    const newChat: Chat = {
      id: newChatId,
      title: `Chat with ${user.name}`,
      timestamp: new Date(),
      lastMessage: '',
      messages: []
    };

    // Add to beginning of list
    this.chats.unshift(newChat);
    this.selectChat(newChatId);
    this.closeNewChatModal();
  }

  // When user clicks on a chat in the list
  selectChat(chatId: string): void {
    this.selectedChatId = chatId;
    this.selectedChat = this.chats.find(chat => chat.id === chatId) || null;
  }

  // Create a new empty chat
  createNewChat(): void {
    const newChatId = Date.now().toString(); // Use timestamp as ID
    const newChat: Chat = {
      id: newChatId,
      title: 'New Conversation',
      timestamp: new Date(),
      lastMessage: '',
      messages: []
    };

    // Add to beginning of list
    this.chats.unshift(newChat);
    this.selectChat(newChatId);
  }

  // Send a message in the current chat
  sendMessage(): void {
    // Don't send if message is empty or no chat is selected
    if (!this.newMessage.trim() || !this.selectedChatId) return;

    // Create message object
    const newUserMessage: ChatMessage = {
      content: this.newMessage,
      timestamp: new Date(),
      sender: 'user'
    };

    // Find the selected chat
    const chatIndex = this.chats.findIndex(chat => chat.id === this.selectedChatId);

    if (chatIndex !== -1) {
      // Add message to chat
      this.chats[chatIndex].messages.push(newUserMessage);
      this.chats[chatIndex].lastMessage = this.newMessage;
      this.chats[chatIndex].timestamp = new Date();

      // Fake response after 1 second
      setTimeout(() => {
        const systemResponse: ChatMessage = {
          content: 'This is a test response. In a real app, this would come from your backend.',
          timestamp: new Date(),
          sender: 'system'
        };

        this.chats[chatIndex].messages.push(systemResponse);
        this.chats[chatIndex].lastMessage = systemResponse.content;
        this.chats[chatIndex].timestamp = new Date();
      }, 1000);

      // Clear the input box
      this.newMessage = '';
    }
  }
}

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

  // Called when component starts
  ngOnInit(): void {
    this.loadChats();
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

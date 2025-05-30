// Common interfaces for mock data

export interface Topic {
  id: string;
  createdByUserId: string;
  title: string;
  description: string;
  dateCreated: Date;
  moduleId: string;
}

export interface Module {
  id: string;
  title: string;
  progress: number;
}

// Chat related interfaces
export interface ChatMessage {
  content: string;
  timestamp: Date;
  sender: 'user' | 'system';
}

export interface Chat {
  id: string;
  title: string;
  timestamp: Date;
  lastMessage: string;
  messages: ChatMessage[];
}

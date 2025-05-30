// Mock data interfaces for Campus Learn

export interface User {
  id: number;
  name: string;
  email: string;
  role: 'Student' | 'Tutor' | 'Admin';
}

export interface Module {
  id: number;
  name: string;
  description: string;
  instructor: string;
  duration: string;
  level: string;
  topics?: Topic[];
}

export interface Topic {
  id: number;
  moduleId: number;
  name: string;
  description: string;
  duration: string;
  status: 'Not Started' | 'In Progress' | 'Completed';
  materials?: LearningMaterial[];
}

export interface LearningMaterial {
  id: number;
  topicId: number;
  name: string;
  description: string;
  fileType: 'PDF' | 'Video' | 'Article' | 'Slides' | 'Audio' | 'Code' | 'Quiz';
  fileName?: string;
  fileSize?: string;
  uploadDate: Date;
  downloadUrl?: string;
  viewUrl?: string;
}

export interface DiscussionPost {
  id: number;
  topicId: number;
  title: string;
  content: string;
  author: User;
  timestamp: Date;
  likes?: number;
  comments?: Comment[];
}

export interface Comment {
  id: number;
  postId: number;
  content: string;
  author: User;
  timestamp: Date;
}

export interface Chat {
  id: number;
  name: string;
  participants: User[];
  lastMessage?: string;
  lastMessageTime?: Date;
  unreadCount?: number;
}

export interface Quiz {
  id: number;
  topicId: number;
  name: string;
  description: string;
  duration: number; // in minutes
  totalQuestions: number;
  passingScore: number;
  difficulty: 'Easy' | 'Medium' | 'Hard';
  status: 'Active' | 'Draft' | 'Archived';
  createdDate: Date;
  dueDate?: Date;
  attempts?: number;
  maxAttempts?: number;
}

import { Chat } from './interfaces';
import { User } from '../services/user.service';

export const mockChats: Chat[] = [
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
  },
  {
    id: '3',
    title: 'Technical Issues',
    timestamp: new Date(Date.now() - 172800000), // 2 days ago
    lastMessage: 'Problem resolved, thank you!',
    messages: [
      {
        content: 'I am having trouble accessing the course materials',
        timestamp: new Date(Date.now() - 180000000),
        sender: 'user'
      },
      {
        content: 'Let me help you troubleshoot that issue',
        timestamp: new Date(Date.now() - 175000000),
        sender: 'system'
      },
      {
        content: 'Problem resolved, thank you!',
        timestamp: new Date(Date.now() - 172800000),
        sender: 'user'
      }
    ]
  }
];

export const mockUsers: User[] = [
  { id: '1', name: 'John Smith', email: 'john.smith@university.edu', role: 'student' },
  { id: '2', name: 'Mary Johnson', email: 'mary.johnson@university.edu', role: 'tutor' },
  { id: '3', name: 'David Brown', email: 'david.brown@university.edu', role: 'student' },
  { id: '4', name: 'Sarah Wilson', email: 'sarah.wilson@university.edu', role: 'admin' },
  { id: '5', name: 'Mike Davis', email: 'mike.davis@university.edu', role: 'student' },
  { id: '6', name: 'Lisa Garcia', email: 'lisa.garcia@university.edu', role: 'tutor' },
  { id: '7', name: 'Tom Miller', email: 'tom.miller@university.edu', role: 'student' },
  { id: '8', name: 'Anna Taylor', email: 'anna.taylor@university.edu', role: 'tutor' },
  { id: '9', name: 'James Wilson', email: 'james.wilson@university.edu', role: 'student' },
  { id: '10', name: 'Emma Johnson', email: 'emma.johnson@university.edu', role: 'tutor' }
];

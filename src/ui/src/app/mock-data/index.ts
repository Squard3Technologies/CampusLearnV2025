// Central export file for all mock data
export * from './interfaces';
export * from './topics';
export * from './modules';
export * from './chats';
// Re-export User from user service to avoid conflicts
export { User } from '../services/user.service';

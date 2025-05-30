import { Topic } from './interfaces';

export const mockTopics: Topic[] = [
  {
    id: '123e4567-e89b-12d3-a456-426614174000',
    createdByUserId: '98765432-e89b-12d3-a456-426614174000',
    title: 'Introduction to Angular',
    description: 'Learn the basics of Angular framework and component architecture',
    dateCreated: new Date('2023-01-15'),
    moduleId: '' // Will be set dynamically by the component
  },
  {
    id: '223e4567-e89b-12d3-a456-426614174001',
    createdByUserId: '88765432-e89b-12d3-a456-426614174001',
    title: 'Component Communication',
    description: 'Understanding how components interact in Angular applications',
    dateCreated: new Date('2023-01-20'),
    moduleId: '' // Will be set dynamically by the component
  },
  {
    id: '323e4567-e89b-12d3-a456-426614174002',
    createdByUserId: '78765432-e89b-12d3-a456-426614174002',
    title: 'Services and Dependency Injection',
    description: 'Learn how to create and use services in Angular applications',
    dateCreated: new Date('2023-01-25'),
    moduleId: ''
  },
  {
    id: '423e4567-e89b-12d3-a456-426614174003',
    createdByUserId: '68765432-e89b-12d3-a456-426614174003',
    title: 'Routing and Navigation',
    description: 'Master Angular routing and navigation between components',
    dateCreated: new Date('2023-01-30'),
    moduleId: ''
  }
];

export const mockModuleNames: { [key: string]: string } = {
  '1': 'Introduction to Programming',
  '2': 'Web Development Fundamentals', 
  '3': 'Database Systems',
  'default': 'Test Module'
};

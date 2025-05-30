// Mock learning materials data

import { LearningMaterial } from './interfaces';

export const mockLearningMaterials: LearningMaterial[] = [
  {
    id: 1,
    topicId: 1,
    name: 'Introduction to Angular Components',
    description: 'Comprehensive guide covering Angular component basics, lifecycle, and best practices',
    fileType: 'PDF',
    fileName: 'angular-components-guide.pdf',
    fileSize: '2.5 MB',
    uploadDate: new Date('2024-01-15'),
    downloadUrl: '/assets/materials/angular-components-guide.pdf',
    viewUrl: '/materials/view/1'
  },
  {
    id: 2,
    topicId: 1,
    name: 'Angular Component Architecture',
    description: 'Video tutorial explaining component architecture and communication patterns',
    fileType: 'Video',
    fileName: 'component-architecture.mp4',
    fileSize: '150 MB',
    uploadDate: new Date('2024-01-18'),
    viewUrl: '/materials/view/2'
  },
  {
    id: 3,
    topicId: 1,
    name: 'Component Lifecycle Hooks',
    description: 'Interactive article with examples of Angular lifecycle hooks implementation',
    fileType: 'Article',
    fileName: 'lifecycle-hooks.html',
    fileSize: '850 KB',
    uploadDate: new Date('2024-01-20'),
    viewUrl: '/materials/view/3'
  },
  {
    id: 4,
    topicId: 1,
    name: 'Component Best Practices Slides',
    description: 'Presentation slides covering Angular component best practices and patterns',
    fileType: 'Slides',
    fileName: 'component-best-practices.pptx',
    fileSize: '5.2 MB',
    uploadDate: new Date('2024-01-22'),
    downloadUrl: '/assets/materials/component-best-practices.pptx',
    viewUrl: '/materials/view/4'
  },
  {
    id: 5,
    topicId: 1,
    name: 'Angular CLI Overview',
    description: 'Audio guide walking through Angular CLI commands and project setup',
    fileType: 'Audio',
    fileName: 'angular-cli-overview.mp3',
    fileSize: '45 MB',
    uploadDate: new Date('2024-01-25'),
    downloadUrl: '/assets/materials/angular-cli-overview.mp3'
  },
  {
    id: 6,
    topicId: 1,
    name: 'Sample Component Code',
    description: 'Complete example component implementation with TypeScript and SCSS',
    fileType: 'Code',
    fileName: 'sample-component.zip',
    fileSize: '125 KB',
    uploadDate: new Date('2024-01-28'),
    downloadUrl: '/assets/materials/sample-component.zip'
  },
  {
    id: 7,
    topicId: 1,
    name: 'Component Knowledge Check',
    description: 'Interactive quiz to test your understanding of Angular components',
    fileType: 'Quiz',
    uploadDate: new Date('2024-01-30'),
    viewUrl: '/materials/quiz/7'
  },
  {
    id: 8,
    topicId: 2,
    name: 'Data Binding Fundamentals',
    description: 'Complete guide to Angular data binding types and implementation',
    fileType: 'PDF',
    fileName: 'data-binding-guide.pdf',
    fileSize: '3.1 MB',
    uploadDate: new Date('2024-02-01'),
    downloadUrl: '/assets/materials/data-binding-guide.pdf',
    viewUrl: '/materials/view/8'
  },
  {
    id: 9,
    topicId: 2,
    name: 'Two-Way Binding Demo',
    description: 'Video demonstration of two-way data binding in Angular forms',
    fileType: 'Video',
    fileName: 'two-way-binding-demo.mp4',
    fileSize: '89 MB',
    uploadDate: new Date('2024-02-03'),
    viewUrl: '/materials/view/9'
  },
  {
    id: 10,
    topicId: 3,
    name: 'Service Injection Patterns',
    description: 'Advanced patterns for dependency injection and service management',
    fileType: 'Article',
    fileName: 'service-injection.html',
    fileSize: '1.2 MB',
    uploadDate: new Date('2024-02-05'),
    viewUrl: '/materials/view/10'
  }
];

export function getMaterialsByTopicId(topicId: number): LearningMaterial[] {
  return mockLearningMaterials.filter(material => material.topicId === topicId);
}

export function getMaterialById(id: number): LearningMaterial | undefined {
  return mockLearningMaterials.find(material => material.id === id);
}

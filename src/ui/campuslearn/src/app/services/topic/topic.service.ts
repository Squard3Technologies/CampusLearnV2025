import { Injectable } from '@angular/core';
import { Observable, of, delay } from 'rxjs';
import { Topic } from '../../models/topic/topic.model';

@Injectable({
  providedIn: 'root'
})
export class TopicService {
  private mockTopics: Topic[] = [
    {
      id: '1',
      createdByUserId: '1',
      title: 'Introduction to Programming',
      description: 'Learn the basics of programming concepts and syntax.',
      dateCreated: new Date('2025-02-15'),
      moduleId: '1'
    },
    {
      id: '2',
      createdByUserId: '1',
      title: 'Variables and Data Types',
      description: 'Understanding different data types and how to use variables.',
      dateCreated: new Date('2025-02-20'),
      moduleId: '1'
    },
    {
      id: '3',
      createdByUserId: '2',
      title: 'Database Normalization',
      description: 'Understanding the principles of database normalization.',
      dateCreated: new Date('2025-03-05'),
      moduleId: '3'
    },
    {
      id: '4',
      createdByUserId: '2',
      title: 'SQL Queries',
      description: 'Learn how to write effective SQL queries.',
      dateCreated: new Date('2025-03-10'),
      moduleId: '3'
    }
  ];

  constructor() { }

  getAllTopics(): Observable<Topic[]> {
    return of(this.mockTopics).pipe(delay(500));
  }

  getTopicById(id: string): Observable<Topic | undefined> {
    const topic = this.mockTopics.find(t => t.id === id);
    return of(topic).pipe(delay(300));
  }

  getTopicsByModule(moduleId: string): Observable<Topic[]> {
    const topics = this.mockTopics.filter(topic => topic.moduleId === moduleId);
    return of(topics).pipe(delay(500));
  }

  getTopicsByUser(userId: string): Observable<Topic[]> {
    const topics = this.mockTopics.filter(topic => topic.createdByUserId === userId);
    return of(topics).pipe(delay(500));
  }

  createTopic(topic: Partial<Topic>): Observable<Topic> {
    const newTopic: Topic = {
      id: Date.now().toString(),
      createdByUserId: topic.createdByUserId || '',
      title: topic.title || '',
      description: topic.description || '',
      dateCreated: new Date(),
      moduleId: topic.moduleId || ''
    };

    this.mockTopics.push(newTopic);
    return of(newTopic).pipe(delay(500));
  }

  updateTopic(id: string, topic: Partial<Topic>): Observable<Topic | undefined> {
    const index = this.mockTopics.findIndex(t => t.id === id);
    if (index !== -1) {
      this.mockTopics[index] = {
        ...this.mockTopics[index],
        ...topic
      };
      return of(this.mockTopics[index]).pipe(delay(500));
    }
    return of(undefined).pipe(delay(300));
  }

  deleteTopic(id: string): Observable<boolean> {
    const initialLength = this.mockTopics.length;
    this.mockTopics = this.mockTopics.filter(t => t.id !== id);

    return of(initialLength > this.mockTopics.length).pipe(delay(500));
  }
}

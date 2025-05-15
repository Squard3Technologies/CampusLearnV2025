import { Injectable } from '@angular/core';
import { Observable, of, delay } from 'rxjs';
import { TopicSubscription } from '../../models/subscription/topic-subscription.model';
import { TutorSubscription } from '../../models/subscription/tutor-subscription.model';

@Injectable({
  providedIn: 'root'
})
export class SubscriptionService {
  private mockTopicSubscriptions: TopicSubscription[] = [
    {
      id: '1',
      topicId: '1',
      userId: '3',
      dateSubscribed: new Date('2025-02-20')
    },
    {
      id: '2',
      topicId: '3',
      userId: '3',
      dateSubscribed: new Date('2025-03-10')
    },
    {
      id: '3',
      topicId: '2',
      userId: '4',
      dateSubscribed: new Date('2025-03-05')
    }
  ];

  private mockTutorSubscriptions: TutorSubscription[] = [
    {
      id: '1',
      tutorId: '2',
      userId: '3',
      dateSubscribed: new Date('2025-02-15')
    },
    {
      id: '2',
      tutorId: '2',
      userId: '4',
      dateSubscribed: new Date('2025-03-01')
    }
  ];

  constructor() { }

  // Topic subscription methods
  getTopicSubscriptionsByUser(userId: string): Observable<TopicSubscription[]> {
    const subscriptions = this.mockTopicSubscriptions.filter(sub => sub.userId === userId);
    return of(subscriptions).pipe(delay(500));
  }

  getTopicSubscriptionsByTopic(topicId: string): Observable<TopicSubscription[]> {
    const subscriptions = this.mockTopicSubscriptions.filter(sub => sub.topicId === topicId);
    return of(subscriptions).pipe(delay(500));
  }

  subscribeToTopic(userId: string, topicId: string): Observable<TopicSubscription> {
    // Check if already subscribed
    const existingSub = this.mockTopicSubscriptions.find(
      sub => sub.userId === userId && sub.topicId === topicId
    );

    if (existingSub) {
      return of(existingSub).pipe(delay(300));
    }

    const newSubscription: TopicSubscription = {
      id: Date.now().toString(),
      topicId,
      userId,
      dateSubscribed: new Date()
    };

    this.mockTopicSubscriptions.push(newSubscription);
    return of(newSubscription).pipe(delay(500));
  }

  unsubscribeFromTopic(userId: string, topicId: string): Observable<boolean> {
    const initialLength = this.mockTopicSubscriptions.length;
    this.mockTopicSubscriptions = this.mockTopicSubscriptions.filter(
      sub => !(sub.userId === userId && sub.topicId === topicId)
    );

    return of(initialLength > this.mockTopicSubscriptions.length).pipe(delay(500));
  }

  // Tutor subscription methods
  getTutorSubscriptionsByUser(userId: string): Observable<TutorSubscription[]> {
    const subscriptions = this.mockTutorSubscriptions.filter(sub => sub.userId === userId);
    return of(subscriptions).pipe(delay(500));
  }

  getTutorSubscriptionsByTutor(tutorId: string): Observable<TutorSubscription[]> {
    const subscriptions = this.mockTutorSubscriptions.filter(sub => sub.tutorId === tutorId);
    return of(subscriptions).pipe(delay(500));
  }

  subscribeToTutor(userId: string, tutorId: string): Observable<TutorSubscription> {
    // Check if already subscribed
    const existingSub = this.mockTutorSubscriptions.find(
      sub => sub.userId === userId && sub.tutorId === tutorId
    );

    if (existingSub) {
      return of(existingSub).pipe(delay(300));
    }

    const newSubscription: TutorSubscription = {
      id: Date.now().toString(),
      tutorId,
      userId,
      dateSubscribed: new Date()
    };

    this.mockTutorSubscriptions.push(newSubscription);
    return of(newSubscription).pipe(delay(500));
  }

  unsubscribeFromTutor(userId: string, tutorId: string): Observable<boolean> {
    const initialLength = this.mockTutorSubscriptions.length;
    this.mockTutorSubscriptions = this.mockTutorSubscriptions.filter(
      sub => !(sub.userId === userId && sub.tutorId === tutorId)
    );

    return of(initialLength > this.mockTutorSubscriptions.length).pipe(delay(500));
  }

  // Utility methods
  isSubscribedToTopic(userId: string, topicId: string): Observable<boolean> {
    const isSubscribed = this.mockTopicSubscriptions.some(
      sub => sub.userId === userId && sub.topicId === topicId
    );
    return of(isSubscribed).pipe(delay(300));
  }

  isSubscribedToTutor(userId: string, tutorId: string): Observable<boolean> {
    const isSubscribed = this.mockTutorSubscriptions.some(
      sub => sub.userId === userId && sub.tutorId === tutorId
    );
    return of(isSubscribed).pipe(delay(300));
  }
}

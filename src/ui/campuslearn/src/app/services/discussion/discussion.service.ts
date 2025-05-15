import { Injectable } from '@angular/core';
import { Observable, of, delay } from 'rxjs';
import { Discussion } from '../../models/discussion/discussion.model';
import { Comment } from '../../models/comment/comment.model';

@Injectable({
  providedIn: 'root'
})
export class DiscussionService {
  private mockDiscussions: Discussion[] = [
    {
      id: '1',
      title: 'Help with programming assignment',
      createdByUserId: '3',
      dateCreated: new Date('2025-03-01'),
      topicId: '1'
    },
    {
      id: '2',
      title: 'Question about data types',
      createdByUserId: '4',
      dateCreated: new Date('2025-03-05'),
      topicId: '2'
    },
    {
      id: '3',
      title: 'SQL join performance',
      createdByUserId: '3',
      dateCreated: new Date('2025-03-15'),
      topicId: '4'
    }
  ];

  private mockComments: Comment[] = [
    {
      id: '1',
      content: 'Can someone explain how to complete the assignment?',
      createdByUserId: '3',
      createdDate: new Date('2025-03-01T10:30:00'),
      discussionId: '1'
    },
    {
      id: '2',
      content: 'I recommend starting with the requirements analysis first.',
      createdByUserId: '2',
      createdDate: new Date('2025-03-01T11:15:00'),
      discussionId: '1'
    },
    {
      id: '3',
      content: 'What\'s the difference between int and float?',
      createdByUserId: '4',
      createdDate: new Date('2025-03-05T14:20:00'),
      discussionId: '2'
    }
  ];

  constructor() { }

  getAllDiscussions(): Observable<Discussion[]> {
    return of(this.mockDiscussions).pipe(delay(500));
  }

  getDiscussionById(id: string): Observable<Discussion | undefined> {
    const discussion = this.mockDiscussions.find(d => d.id === id);
    return of(discussion).pipe(delay(300));
  }

  getDiscussionsByTopic(topicId: string): Observable<Discussion[]> {
    const discussions = this.mockDiscussions.filter(discussion => discussion.topicId === topicId);
    return of(discussions).pipe(delay(500));
  }

  getDiscussionsByUser(userId: string): Observable<Discussion[]> {
    const discussions = this.mockDiscussions.filter(discussion => discussion.createdByUserId === userId);
    return of(discussions).pipe(delay(500));
  }

  createDiscussion(discussion: Partial<Discussion>): Observable<Discussion> {
    const newDiscussion: Discussion = {
      id: Date.now().toString(),
      title: discussion.title || '',
      createdByUserId: discussion.createdByUserId || '',
      dateCreated: new Date(),
      topicId: discussion.topicId || ''
    };

    this.mockDiscussions.push(newDiscussion);
    return of(newDiscussion).pipe(delay(500));
  }

  updateDiscussion(id: string, discussion: Partial<Discussion>): Observable<Discussion | undefined> {
    const index = this.mockDiscussions.findIndex(d => d.id === id);
    if (index !== -1) {
      this.mockDiscussions[index] = {
        ...this.mockDiscussions[index],
        ...discussion
      };
      return of(this.mockDiscussions[index]).pipe(delay(500));
    }
    return of(undefined).pipe(delay(300));
  }

  deleteDiscussion(id: string): Observable<boolean> {
    const initialLength = this.mockDiscussions.length;
    this.mockDiscussions = this.mockDiscussions.filter(d => d.id !== id);

    // Also delete associated comments
    this.mockComments = this.mockComments.filter(c => c.discussionId !== id);

    return of(initialLength > this.mockDiscussions.length).pipe(delay(500));
  }

  // Comments related methods
  getCommentsByDiscussion(discussionId: string): Observable<Comment[]> {
    const comments = this.mockComments.filter(comment => comment.discussionId === discussionId);
    return of(comments).pipe(delay(500));
  }

  addComment(comment: Partial<Comment>): Observable<Comment> {
    const newComment: Comment = {
      id: Date.now().toString(),
      content: comment.content || '',
      createdByUserId: comment.createdByUserId || '',
      createdDate: new Date(),
      discussionId: comment.discussionId,
      enquiryId: comment.enquiryId
    };

    this.mockComments.push(newComment);
    return of(newComment).pipe(delay(500));
  }

  deleteComment(commentId: string): Observable<boolean> {
    const initialLength = this.mockComments.length;
    this.mockComments = this.mockComments.filter(c => c.id !== commentId);

    return of(initialLength > this.mockComments.length).pipe(delay(500));
  }
}

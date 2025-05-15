import { Injectable } from '@angular/core';
import { Observable, of, delay } from 'rxjs';
import { Enquiry, EnquiryStatus } from '../../models/enquiry/enquiry.model';
import { Comment } from '../../models/comment/comment.model';

@Injectable({
  providedIn: 'root'
})
export class EnquiryService {
  private mockEnquiries: Enquiry[] = [
    {
      id: '1',
      createdByUserId: '3',
      title: 'Question about assignment deadline',
      content: 'When is the final deadline for submitting the CS101 assignment?',
      dateCreated: new Date('2025-03-10'),
      status: EnquiryStatus.Open
    },
    {
      id: '2',
      createdByUserId: '4',
      title: 'Database connection issue',
      content: 'I\'m having trouble connecting to the SQL database for the lab exercise.',
      dateCreated: new Date('2025-03-15'),
      dateResolved: new Date('2025-03-16'),
      resolvedByUserId: '2',
      status: EnquiryStatus.Resolved
    }
  ];

  private mockComments: Comment[] = [
    {
      id: '4',
      content: 'I checked the syllabus, but couldn\'t find the deadline.',
      createdByUserId: '3',
      createdDate: new Date('2025-03-10T09:15:00'),
      enquiryId: '1'
    },
    {
      id: '5',
      content: 'The deadline is March 25th, as mentioned in the latest announcement.',
      createdByUserId: '2',
      createdDate: new Date('2025-03-10T10:30:00'),
      enquiryId: '1'
    },
    {
      id: '6',
      content: 'Are you using the correct connection string?',
      createdByUserId: '2',
      createdDate: new Date('2025-03-15T16:45:00'),
      enquiryId: '2'
    }
  ];

  constructor() { }

  getAllEnquiries(): Observable<Enquiry[]> {
    return of(this.mockEnquiries).pipe(delay(500));
  }

  getEnquiryById(id: string): Observable<Enquiry | undefined> {
    const enquiry = this.mockEnquiries.find(e => e.id === id);
    return of(enquiry).pipe(delay(300));
  }

  getEnquiriesByUser(userId: string): Observable<Enquiry[]> {
    const enquiries = this.mockEnquiries.filter(enquiry =>
      enquiry.createdByUserId === userId || enquiry.resolvedByUserId === userId
    );
    return of(enquiries).pipe(delay(500));
  }

  getOpenEnquiries(): Observable<Enquiry[]> {
    const enquiries = this.mockEnquiries.filter(enquiry => enquiry.status === EnquiryStatus.Open);
    return of(enquiries).pipe(delay(500));
  }

  createEnquiry(enquiry: Partial<Enquiry>): Observable<Enquiry> {
    const newEnquiry: Enquiry = {
      id: Date.now().toString(),
      createdByUserId: enquiry.createdByUserId || '',
      title: enquiry.title || '',
      content: enquiry.content || '',
      dateCreated: new Date(),
      status: EnquiryStatus.Open
    };

    this.mockEnquiries.push(newEnquiry);
    return of(newEnquiry).pipe(delay(500));
  }

  updateEnquiry(id: string, enquiry: Partial<Enquiry>): Observable<Enquiry | undefined> {
    const index = this.mockEnquiries.findIndex(e => e.id === id);
    if (index !== -1) {
      this.mockEnquiries[index] = {
        ...this.mockEnquiries[index],
        ...enquiry
      };
      return of(this.mockEnquiries[index]).pipe(delay(500));
    }
    return of(undefined).pipe(delay(300));
  }

  resolveEnquiry(id: string, resolvedByUserId: string): Observable<Enquiry | undefined> {
    const index = this.mockEnquiries.findIndex(e => e.id === id);
    if (index !== -1) {
      this.mockEnquiries[index] = {
        ...this.mockEnquiries[index],
        status: EnquiryStatus.Resolved,
        dateResolved: new Date(),
        resolvedByUserId
      };
      return of(this.mockEnquiries[index]).pipe(delay(500));
    }
    return of(undefined).pipe(delay(300));
  }

  linkEnquiryToTopic(id: string, topicId: string): Observable<Enquiry | undefined> {
    const index = this.mockEnquiries.findIndex(e => e.id === id);
    if (index !== -1) {
      this.mockEnquiries[index] = {
        ...this.mockEnquiries[index],
        status: EnquiryStatus.LinkedTopic,
        topicId
      };
      return of(this.mockEnquiries[index]).pipe(delay(500));
    }
    return of(undefined).pipe(delay(300));
  }

  deleteEnquiry(id: string): Observable<boolean> {
    const initialLength = this.mockEnquiries.length;
    this.mockEnquiries = this.mockEnquiries.filter(e => e.id !== id);

    // Also delete associated comments
    this.mockComments = this.mockComments.filter(c => c.enquiryId !== id);

    return of(initialLength > this.mockEnquiries.length).pipe(delay(500));
  }

  // Comments related methods
  getCommentsByEnquiry(enquiryId: string): Observable<Comment[]> {
    const comments = this.mockComments.filter(comment => comment.enquiryId === enquiryId);
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

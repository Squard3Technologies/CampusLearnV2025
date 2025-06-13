import { GenericAPIResponse } from './api.models';

export interface DiscussionAuthor {
  id: string;
  firstName: string;
  surname: string;
  emailAddress?: string;
}

export interface DiscussionComment {
  id: string;
  content: string;
  author: DiscussionAuthor;
  createdOn: Date;
}

export interface Discussion {
  id: string;
  title: string;
  content: string;
  author: DiscussionAuthor;
  createdOn: Date;
  comments?: DiscussionComment[];
}

export interface CreateDiscussionRequest {
  title: string;
  content: string;
}

export interface CreateCommentRequest {
  content: string;
}

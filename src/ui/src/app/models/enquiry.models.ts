export interface Enquiry {
  id: string;
  createdByUserId: string;
  title: string;
  description: string;
  dateCreated: Date;
  dateResolved?: Date;
  resolvedByUserId?: string;
  resolvedByUserEmail?: string;
  createdByUserEmail: string;
  resolutionAction?: number;
  resolutionResponse?: string;
  status: number;
  moduleId: string;
  linkedTopicId?: string;
  topicTitle: string;
}
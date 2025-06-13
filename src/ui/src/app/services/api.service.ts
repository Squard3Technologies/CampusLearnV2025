import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { GenericAPIResponse, SystemUser, Module } from '../models/api.models';
import { map, Observable } from 'rxjs';
import { ChatUser, ChatMessage, CreateMessageRequest, SearchUser } from '../models/chat.models';

@Injectable({
  providedIn: 'root'
})
export class ApiService {


  constructor(private httpClient: HttpClient) {
  }

  private apiUrl = '/api/v1';

  // Authentication
  login(credentials: { email: string, password: string }) {
    const loginBody = {
      username: credentials.email,
      password: credentials.password
    };
    const url = `${this.apiUrl}/user/login`;
    return this.httpClient.post(url, loginBody);
  }

  register(userData: any): Observable<GenericAPIResponse<string>> {
    const url = `${this.apiUrl}/user/register`;
    return this.httpClient.post<GenericAPIResponse<string>>(url, userData);
  }

  forgotPassword(email: { email: string }) {
    return this.httpClient.post(`${this.apiUrl}/forgot-password`, email);
  }

  // Modules & Topics
  getModules() {
    return this.httpClient.get(`${this.apiUrl}/modules`);
  }

  getTopics(moduleId: string) {
    return this.httpClient.get(`${this.apiUrl}/modules/${moduleId}/topics`);
  }

  getTopic(moduleId: string, topicId: string) {
    return this.httpClient.get(`${this.apiUrl}/modules/${moduleId}/topics/${topicId}`);
  }

  createTopic(topicData: any) {
    return this.httpClient.post(`${this.apiUrl}/topics`, topicData);
  }

  getTopicDescription(topicId: string) {
    return this.httpClient.get(`${this.apiUrl}/topic-description`, { params: { topicId } });
  }

  // Discussions & Learning Material
  getDiscussions(topicId: string) {
    return this.httpClient.get(`${this.apiUrl}/discussions`, { params: { topicId } });
  }

  getLearningMaterial(topicId: string) {
    return this.httpClient.get(`${this.apiUrl}/learning-material`, { params: { topicId } });
  }

  createLearningMaterial(materialData: any) {
    return this.httpClient.post(`${this.apiUrl}/learning-material`, materialData);
  }

  downloadLearningMaterial(fileId: string) {
    return this.httpClient.get(`${this.apiUrl}/learning-material/download`, {
      params: { fileId },
      responseType: 'blob'
    });
  }

  // Quizzes
  getQuizzes(topicId: string) {
    return this.httpClient.get(`${this.apiUrl}/quizzes/topic/${topicId}`);
  }

  getQuizDetails(quizId: string) : any {
    return this.httpClient.get(`${this.apiUrl}/quizzes/${quizId}/details`);
  }

  createQuestion(questionData: any) {
    return this.httpClient.post(`${this.apiUrl}/questions`, questionData);
  }

  updateQuestion(id: string, questionData: any) {
    return this.httpClient.put(`${this.apiUrl}/questions/${id}`, questionData);
  }

  getActiveQuizzes() : Observable<any[]> {
    return this.httpClient.get<any[]>(`${this.apiUrl}/quizzes/active`);
  }

  createQuiz(data: any) {
    return this.httpClient.post(`${this.apiUrl}/quizzes`, data);
  }

  updateQuiz(quizId: any, data: any) {
    return this.httpClient.put(`${this.apiUrl}/quizzes/${quizId}`, data);
  }

  createQuizAttempt(quizId: any) {
    return this.httpClient.post(`${this.apiUrl}/quizzes/${quizId}/attempt`, {});
  }

  assignQuizAttempt(quizId: any, assignedToUserId: any) {
    return this.httpClient.post(`${this.apiUrl}/quizzes/${quizId}/attempt?assignedByUserId=${assignedToUserId}`, {});
  }

  submitQuizAttempt(quizAttemptId: any, attemptData: any) {
    return this.httpClient.put(`${this.apiUrl}/quizzes/attempt/${quizAttemptId}`, attemptData);
  }

  getQuizHistory() {
    return this.httpClient.get(`${this.apiUrl}/quizzes/attempt-history`);
  }

  getQuizAttemptHistory(quizId: string): any {
    return this.httpClient.get(`${this.apiUrl}/quizzes/attempt-history/${quizId}`);
  }

  // Enquiries
  getEnquiries() {
    return this.httpClient.get(`${this.apiUrl}/enquiries`);
  }

  createEnquiry(enquiryData: any) {
    return this.httpClient.post(`${this.apiUrl}/enquiries`, enquiryData);
  }

  getEnquiry(id: string) {
    return this.httpClient.get(`${this.apiUrl}/enquiries/${id}`);
  }

  getActiveEnquiries() {
    return this.httpClient.get(`${this.apiUrl}/enquiries/active`);
  }

  resolveEnquiry(id: string, resolutionData: any) {
    return this.httpClient.post(`${this.apiUrl}/enquiries/${id}/resolve`, resolutionData);
  }

  getResolvedEnquiries() {
    return this.httpClient.get(`${this.apiUrl}/enquiries/resolved`);
  }
  // Chat API methods
  getChats(): Observable<ChatUser[]> {
    return this.httpClient.get<ChatUser[]>(`${this.apiUrl}/chats`);
  }

  getChatMessages(userId: string): Observable<ChatMessage[]> {
    return this.httpClient.get<ChatMessage[]>(`${this.apiUrl}/chats/user/${userId}`);
  }

  sendMessage(userId: string, messageRequest: CreateMessageRequest): Observable<string> {
    return this.httpClient.post<string>(`${this.apiUrl}/chats/user/${userId}/message`, messageRequest);
  }
  // Enhanced user search for chat (update existing method)
  searchUsers(query: string): Observable<SearchUser[]> {
    // Get all users and filter on frontend since backend doesn't support search
    return this.httpClient.get<any>(`${this.apiUrl}/user/get`).pipe(
      map((response: any) => {
        // Extract users from the API response body
        const users = response?.body || [];
        
        // Filter users based on query
        const filteredUsers = users.filter((user: any) => {
          const searchLower = query.toLowerCase();
          const nameMatch = (user.firstName?.toLowerCase() || '').includes(searchLower) ||
                           (user.surname?.toLowerCase() || '').includes(searchLower);
          const emailMatch = (user.emailAddress?.toLowerCase() || '').includes(searchLower);
          return nameMatch || emailMatch;
        });
        
        // Map to SearchUser interface
        return filteredUsers.map((user: any) => ({
          id: user.id,
          firstName: user.firstName || '',
          surname: user.surname || '',
          emailAddress: user.emailAddress || '',
          role: user.role || 4
        }));
      })
    );
  }

  // Legacy method - keeping for backward compatibility
  getUserChat(userId: string) {
    return this.httpClient.get(`${this.apiUrl}/chats/${userId}`);
  }

  // User Profile
  getUserProfile() {
    
    return this.httpClient.get(`${this.apiUrl}/user`);
  }

  updateUserProfile(userData: any) {
    return this.httpClient.put(`${this.apiUrl}/user`, userData);
  }

  changePassword(passwordData: { currentPassword: string, newPassword: string }) {
    return this.httpClient.post(`${this.apiUrl}/change-password`, passwordData);
  }

  // Subscriptions
  getTutorSubscriptions() {
    return this.httpClient.get(`${this.apiUrl}/subscriptions/tutors`);
  }

  deleteTutorSubscription(id: string) {
    return this.httpClient.delete(`${this.apiUrl}/subscriptions/tutors/${id}`);
  }

  getAvailableTutors() {
    return this.httpClient.get(`${this.apiUrl}/tutors/available`);
  }

  subscribeTutor(tutorData: any) {
    return this.httpClient.post(`${this.apiUrl}/subscriptions/tutors`, tutorData);
  }

  getTopicSubscriptions() {
    return this.httpClient.get(`${this.apiUrl}/subscriptions/topics`);
  }

  unsubscribeFromTopic(topicData: any) {
    return this.httpClient.post(`${this.apiUrl}/subscriptions/topics/unsubscribe`, topicData);
  }

  // Admin Dashboard - Registrations
  getPendingRegistrations(): Observable<GenericAPIResponse<SystemUser[]>> {
    return this.httpClient.get<GenericAPIResponse<SystemUser[]>>(`${this.apiUrl}/admin/registrations`);
  }

  processRegistration(id: string, status:string): Observable<GenericAPIResponse<string>> {
    const requestBody = {
      userId: id,
      accountStatusId: status
    };
    return this.httpClient.post<GenericAPIResponse<string>>(`${this.apiUrl}/admin/registrations/status`, requestBody);
  }

  

  // Admin Dashboard - User Management
  getAdminUsers(): Observable<GenericAPIResponse<SystemUser[]>> {    
    return this.httpClient.get<GenericAPIResponse<SystemUser[]>>(`${this.apiUrl}/admin/users` );
  }

  //Activating, deactivate, blocking & deleting user account by changing the account status
  changeUserAccountStatus(id: string, status: string) {
    const requestBody = {
      userId: id,
      accountStatusId: status
    };
    return this.httpClient.post(`${this.apiUrl}/admin/users/status`, requestBody);
  }


  deactivateUser(id: string) {
    return this.httpClient.post(`${this.apiUrl}/admin/users/${id}/deactivate`, {});
  }

  activateUser(id: string) {
    return this.httpClient.post(`${this.apiUrl}/admin/users/${id}/activate`, {});
  }

  updateUser(id: string, userData: any) {
    return this.httpClient.put(`${this.apiUrl}/admin/users/${id}`, userData);
  }

   updateUserByAdmin(userData: any):Observable<GenericAPIResponse<string>> {
    return this.httpClient.post<GenericAPIResponse<string>>(`${this.apiUrl}/admin/users/update`, userData);
  }

  // Admin Dashboard - Module Management
  getAdminModules():Observable<GenericAPIResponse<Module[]>> {
    return this.httpClient.get<GenericAPIResponse<Module[]>>(`${this.apiUrl}/admin/modules`);
  }

  createModule(moduleData: any): Observable<GenericAPIResponse<string>> {
    return this.httpClient.post<GenericAPIResponse<string>>(`${this.apiUrl}/admin/modules`, moduleData);
  }

  updateModule(id: string, moduleData: any): Observable<GenericAPIResponse<string>> {
    return this.httpClient.put<GenericAPIResponse<string>>(`${this.apiUrl}/admin/modules/`, moduleData);
  }

  deactivateModule(id: string) {
    return this.httpClient.post(`${this.apiUrl}/admin/modules/${id}/deactivate`, {});
  }

  activateModule(id: string) {
    return this.httpClient.post(`${this.apiUrl}/admin/modules/${id}/activate`, {});
  }
}

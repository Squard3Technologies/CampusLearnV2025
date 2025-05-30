import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

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

  register(userData: any) {
    const url = `${this.apiUrl}/user/createaccount`;
    return this.httpClient.post(url, userData);
  }

  forgotPassword(email: { email: string }) {
    return this.httpClient.post(`${this.apiUrl}/forgot-password`, email);
  }

  // Modules & Topics
  getModules() {
    return this.httpClient.get(`${this.apiUrl}/modules`);
  }

  getTopics(moduleId: string) {
    return this.httpClient.get(`${this.apiUrl}/topics`, { params: { moduleId } });
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
    return this.httpClient.get(`${this.apiUrl}/quizzes`, { params: { topicId } });
  }

  getQuizDetails(quizId: string) {
    return this.httpClient.get(`${this.apiUrl}/quiz-details`, { params: { quizId } });
  }

  createQuestion(questionData: any) {
    return this.httpClient.post(`${this.apiUrl}/questions`, questionData);
  }

  updateQuestion(id: string, questionData: any) {
    return this.httpClient.put(`${this.apiUrl}/questions/${id}`, questionData);
  }

  getActiveQuizzes() {
    return this.httpClient.get(`${this.apiUrl}/active-quizzes`);
  }

  submitQuizAttempt(attemptData: any) {
    return this.httpClient.post(`${this.apiUrl}/quiz-attempt`, attemptData);
  }

  getQuizHistory() {
    return this.httpClient.get(`${this.apiUrl}/quiz-history`);
  }

  getQuizAttemptHistory(quizId: string) {
    return this.httpClient.get(`${this.apiUrl}/quiz-attempt-history`, { params: { quizId } });
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

  // Chat
  getChats() {
    return this.httpClient.get(`${this.apiUrl}/chats`);
  }

  getUserChat(userId: string) {
    return this.httpClient.get(`${this.apiUrl}/chats/${userId}`);
  }

  searchUsers(query: string) {
    return this.httpClient.get(`${this.apiUrl}/users`, { params: { search: query } });
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
  getPendingRegistrations() {
    return this.httpClient.get(`${this.apiUrl}/admin/registrations/pending`);
  }

  acceptRegistration(id: string) {
    return this.httpClient.post(`${this.apiUrl}/admin/registrations/${id}/accept`, {});
  }

  rejectRegistration(id: string, reason?: string) {
    return this.httpClient.post(`${this.apiUrl}/admin/registrations/${id}/reject`, { reason });
  }

  // Admin Dashboard - User Management
  getAdminUsers() {
    return this.httpClient.get(`${this.apiUrl}/admin/users`);
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

  // Admin Dashboard - Module Management
  getAdminModules() {
    return this.httpClient.get(`${this.apiUrl}/admin/modules`);
  }

  createModule(moduleData: any) {
    return this.httpClient.post(`${this.apiUrl}/admin/modules`, moduleData);
  }

  updateModule(id: string, moduleData: any) {
    return this.httpClient.put(`${this.apiUrl}/admin/modules/${id}`, moduleData);
  }

  deactivateModule(id: string) {
    return this.httpClient.post(`${this.apiUrl}/admin/modules/${id}/deactivate`, {});
  }

  activateModule(id: string) {
    return this.httpClient.post(`${this.apiUrl}/admin/modules/${id}/activate`, {});
  }
}

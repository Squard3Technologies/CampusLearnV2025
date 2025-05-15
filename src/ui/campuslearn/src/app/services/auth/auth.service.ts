import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { User, UserRole } from '../../models/user/user.module';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor() {
    this.loadUserFromStorage();
  }

  login(email: string, password: string): Observable<User> {
    return new Observable<User>(observer => {
      // In a real application, this would make an HTTP request to the backend
      // For demo purposes, we're simulating a successful login
      setTimeout(() => {
        const user: User = {
          id: '1',
          name: 'John',
          surname: 'Doe',
          email: email,
          password: password, // Note: In a real app, never store plain passwords
          contactNumber: '1234567890',
          role: UserRole.Student
        };

        this.setCurrentUser(user);
        observer.next(user);
        observer.complete();
      }, 1000);
    });
  }

  logout(): void {
    this.clearCurrentUser();
  }

  register(user: Partial<User>): Observable<User> {
    return new Observable<User>(observer => {
      // In a real application, this would make an HTTP request to the backend
      setTimeout(() => {
        const newUser: User = {
          id: Math.random().toString(36).substring(2, 9),
          name: user.name || '',
          surname: user.surname || '',
          email: user.email || '',
          password: user.password || '',
          contactNumber: user.contactNumber || '',
          role: user.role || UserRole.Student
        };

        this.setCurrentUser(newUser);
        observer.next(newUser);
        observer.complete();
      }, 1000);
    });
  }

  isAuthenticated(): boolean {
    return !!this.currentUserSubject.value;
  }

  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  private setCurrentUser(user: User): void {
    localStorage.setItem('currentUser', JSON.stringify(user));
    this.currentUserSubject.next(user);
  }

  private clearCurrentUser(): void {
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }

  private loadUserFromStorage(): void {
    const storedUser = localStorage.getItem('currentUser');
    if (storedUser) {
      try {
        const user = JSON.parse(storedUser);
        this.currentUserSubject.next(user);
      } catch (error) {
        console.error('Error parsing stored user data:', error);
        localStorage.removeItem('currentUser');
      }
    }
  }
}

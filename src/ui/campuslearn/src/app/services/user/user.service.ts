import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { User } from '../../models/user/user';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private currentUserSubject = new BehaviorSubject<User | null>(null);

  public currentUser$: Observable<User | null> =
    this.currentUserSubject.asObservable();

  constructor() {
    this.loadUserFromStorage();
  }

  setCurrentUser(user: User): void {

    localStorage.setItem('currentUser', JSON.stringify(user));

    this.currentUserSubject.next(user);
  }

  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  clearCurrentUser(): void {
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

import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { ApiService } from './api.service';

// Simple user interface
export interface User {
  id: string;
  name: string;
  email: string;
  role: string;
}


@Injectable({
  providedIn: 'root'
})
export class UserService {
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private apiService: ApiService) {
    // Check local storage when service starts
    this.loadUserFromStorage();
  }

  // Enhanced login method that handles JWT token processing
  login(email: string, password: string): Observable<User> {
    return this.apiService.login({ email, password }).pipe(
      tap((response: any) => {
        // Process the login response and handle JWT token
        const user = this.processLoginResponse(response, email);
        this.setCurrentUser(user);
      })
    );
  }

  // Process login response and extract user data from JWT token
  private processLoginResponse(response: any, email: string): User {
    // Check if the response has a 200 status code
    if (response.statusCode && response.statusCode !== 200) {
      throw new Error(response.statusMessage || 'Login failed');
    }

    let userData: Partial<User> = {
      email: email,
      id: '',
      name: '',
      role: ''
    };
    if (response.body) {
      // Store the raw token
      this.storeAuthToken(response.body);

      // Decode the JWT token and extract user information
      const decodedToken = this.decodeJwtToken(response.body);
      if (decodedToken) {

        // Extract claims from the decoded token
        const userId = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
        const userEmail = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'];
        const firstName = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
        const lastName = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname'];
        const roles = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];


        console.log('Decoded JWT claims:', {
          userId, userEmail, firstName, lastName, roles
        });



        // Determine primary role (using first role if multiple exist)
        const primaryRole = Array.isArray(roles) ? roles[0] : roles || 'student';

        userData = {
          id: userId,
          name: `${firstName} ${lastName}`,
          email: userEmail ,
          role: primaryRole
        };
      }
    }

    return userData as User;
  }

  // Decode JWT token without verification (client-side only)
  private decodeJwtToken(token: string):  any{
    try {

      //this takes the token away from the signature after the dot
      const base64Url = token.split('.')[1];

      // Replace URL-safe characters with standard Base64 characters
      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');

      // Decode Base64 string
      const decodedToken = JSON.parse(atob(base64));

      return decodedToken;
    } catch (error) {
      console.error('Error decoding JWT token:', error);
      return null;
    }
  }

  // Store authentication token
  private storeAuthToken(token: string): void {
    localStorage.setItem('auth_token', token);
  }

  // Get stored authentication token
  getAuthToken(): string | null {
    return localStorage.getItem('auth_token');
  }

  // Check if user is authenticated (has valid token)
  isAuthenticated(): boolean {
    const token = this.getAuthToken();
    if (!token) return false;

    // Check if token is expired
    const decodedToken = this.decodeJwtToken(token);
    if (decodedToken && decodedToken.exp) {
      const currentTime = Math.floor(Date.now() / 1000);
      return decodedToken.exp > currentTime;
    }

    return true; // If we can't decode expiration, assume valid
  }

  // Set the current user after login
  setCurrentUser(user: User): void {
    localStorage.setItem('currentUser', JSON.stringify(user));
    this.currentUserSubject.next(user);
  }

  // Get the current user
  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  // Clear the user on logout
  clearCurrentUser(): void {
    localStorage.removeItem('currentUser');
    localStorage.removeItem('auth_token');
    this.currentUserSubject.next(null);
  }

  // Load user from local storage (if available)
  private loadUserFromStorage(): void {
    const storedUser = localStorage.getItem('currentUser');
    if (storedUser) {
      try {
        const user = JSON.parse(storedUser);
        // Also check if we have a valid token
        if (this.isAuthenticated()) {
          this.currentUserSubject.next(user);
        } else {
          // Clear invalid session
          this.clearCurrentUser();
        }
      } catch (error) {
        console.error('Error parsing stored user data:', error);
        this.clearCurrentUser();
      }
    }
  }
}

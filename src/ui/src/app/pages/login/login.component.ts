import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  errorMessage: string = '';

  constructor(
    private userService: UserService,
    private router: Router
  ) {}

  onSubmit(): void {
    if (!this.email || !this.password) {
      this.errorMessage = 'Please enter both email and password';
      return;
    }

    // Clear any previous error message
    this.errorMessage = '';

    console.log('Attempting login with email:', this.email);

    // Call the API login method
    this.userService.login(this.email, this.password).subscribe({
      next: (response: any) => {
        console.log('Login successful:', response);
        // If login is successful, create user object from response
        const user = {
          id: response.id || '1',
          name: response.name || 'User',
          email: this.email,
          role: response.role || 'student'
        };

        // Set the current user and navigate to home
        this.userService.setCurrentUser(user);
        this.router.navigate(['/home']);
      },
      error: (error) => {
        // Handle login error
        console.error('Login error:', error);
        console.error('Error details:', error.message);
        this.errorMessage = 'Invalid email or password. Please try again.';
      }
    });
      }
}

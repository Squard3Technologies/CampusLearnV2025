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

    // Call the enhanced login method that handles JWT processing
    this.userService.login(this.email, this.password).subscribe({
      next: (user) => {
        console.log('Login successful for user:', user);
        // Navigate to home page - user is already set in the service
        this.router.navigate(['/home']);
      },
      error: (error) => {
        // Handle login error
        console.error('Login error:', error);
        this.errorMessage = error.message || 'Invalid email or password. Please try again.';
      }
    });
  }
}

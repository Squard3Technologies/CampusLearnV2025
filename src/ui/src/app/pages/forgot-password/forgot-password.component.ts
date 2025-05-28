import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.scss'
})
export class ForgotPasswordComponent {
  email: string = '';
  errorMessage: string = '';
  successMessage: string = '';

  constructor(private router: Router) {}

  onSubmit(): void {
    // Clear previous messages
    this.errorMessage = '';
    this.successMessage = '';

    // Check if email is entered
    if (!this.email) {
      this.errorMessage = 'Please enter your email address';
      return;
    }

    // Check if email format is valid (basic check)
    if (!this.email.includes('@')) {
      this.errorMessage = 'Please enter a valid email address';
      return;
    }

    // For demo purposes, we'll just show a success message
    // In a real app, you would call an API to send the reset email
    this.successMessage = `Password reset instructions have been sent to ${this.email}`;

    // Clear the email field
    this.email = '';
  }

  goBackToLogin(): void {
    this.router.navigate(['/login']);
  }
}

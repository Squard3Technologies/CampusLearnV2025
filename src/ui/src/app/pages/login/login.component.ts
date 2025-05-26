import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
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

  forgotPassword($event: Event): void {
    $event.preventDefault();

    if (!this.email) {
      this.errorMessage = 'Please enter your email address to reset your password';
      return;
    }

    // Here you would typically call a service method to initiate password reset
    // For demo purposes, we'll just show a success message
    this.errorMessage = '';
    alert(`Password reset instructions sent to ${this.email}`);

    // In a real implementation, you would call the user service
    // this.userService.requestPasswordReset(this.email).subscribe(
    //   () => alert('Password reset email sent'),
    //   (error) => this.errorMessage = error.message
    // );
  }

  onSubmit(): void {
    if (!this.email || !this.password) {
      this.errorMessage = 'Please enter both email and password';
      return;
    }

    // For demo purposes, we'll just simulate a successful login
    const user = {
      id: '1',
      name: 'Demo User',
      email: this.email,
      role: 'admin'  // or 'student', 'teacher', etc.
    };

    this.userService.setCurrentUser(user);
    this.router.navigate(['/home']);
  }
}

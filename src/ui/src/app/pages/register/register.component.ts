import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  // Form fields to match the API requirements
  firstName: string = '';
  surname: string = '';
  emailAddress: string = '';
  password: string = '';
  confirmPassword: string = '';
  contactNumber: string = '';
  errorMessage: string = '';

  constructor(
    private router: Router,
    private apiService: ApiService
  ){}

  onSubmit(): void {
    // Check if all required fields are filled
    if (!this.firstName || !this.surname || !this.emailAddress || !this.password || !this.confirmPassword || !this.contactNumber) {
      this.errorMessage = 'Please fill in all fields';
      return;
    }

    // Check if passwords match
    if (this.password !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match';
      return;
    }

    // Check if email is valid format
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(this.emailAddress)) {
      this.errorMessage = 'Please enter a valid email address';
      return;
    }

    // Check if contact number is valid (basic check for numbers)
    if (!/^\d{10}$/.test(this.contactNumber)) {
      this.errorMessage = 'Please enter a valid 10-digit contact number';
      return;
    }

    // Clear any previous error
    this.errorMessage = '';

    // Create user object for registration
    const userData = {
      firstName: this.firstName,
      surname: this.surname,
      middleName: '', // Optional field, can be left empty
      emailAddress: this.emailAddress,
      password: this.password,
      contactNumber: this.contactNumber,
      role:1
    };

    console.log('Registration data:', userData);

    // Call API registration service
    this.apiService.register(userData).subscribe({
      next: (response) => {
        console.log('Registration successful:', response);
        alert('Registration successful! Please login.');
        this.router.navigate(['/login']);
      },
      error: (error) => {
        console.error('Registration error:', error);
        this.errorMessage = error.error?.message || 'Registration failed. Please try again.';
      }
    });
  }
}

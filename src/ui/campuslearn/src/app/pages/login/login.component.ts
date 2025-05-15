import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../services/user/user.service';
import { User, UserRole } from '../../models/user/user.module';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginError = '';
  username = '';
  password = '';
  isLoggingIn = false;

  constructor(private router: Router, private userService: UserService) {}

  onSubmit(): void {
    this.loginError = '';
    this.isLoggingIn = true;

    console.log('Login submitted:', this.username, this.password);

    // Simulate a login delay
    setTimeout(async () => {
      // Create a mock user for demonstration
      const mockUser: User = {
        id: '1',
        name: 'Mock User',
        surname: 'Student',
        email: this.username,
        password: this.password, // In a real app, never store passwords in the client
        contactNumber: '1234567890',
        role: UserRole.Student
      };

      // Log before setting the user
      console.log('Setting user in service:', mockUser);

      // Set the current user in the service
      this.userService.setCurrentUser(mockUser);

      // Navigate to home page
      this.isLoggingIn = false;
      this.router.navigate(['/home']);
    }, 1000); // Simulate network delay
  }
}

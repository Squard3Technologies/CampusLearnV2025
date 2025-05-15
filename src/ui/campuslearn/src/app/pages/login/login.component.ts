import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../services/user/user.service';
import { User, UserRole } from '../../models/user/user';


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

  constructor(private router: Router, private userService: UserService) {}

  onSubmit(): void {
    this.loginError = '';
    console.log('Login submitted:', this.username, this.password);
    const mockUser: User = {
      id: '1',
      name: 'Mock',
      surname: 'User',
      email: 'mockuser@example.com',
      role: UserRole.Admin,
      password: this.password,
      contactNumber: '1234567890'
    };

    this.userService.setCurrentUser(mockUser);
    this.router.navigate(['/home']);

  }
}

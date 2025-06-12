import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { UserService } from '../../services/user.service';
import Swal from 'sweetalert2';

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
  ) { }

  onSubmit(): void {
    if (!this.email || !this.password) {
      this.errorMessage = 'Please enter both email and password';
      return;
    }

    // Clear any previous error message
    this.errorMessage = '';

    console.log('Attempting login with email:', this.email);

    Swal.fire({
      title: 'Authenticating...',
      iconColor: '#AD0151',
      allowOutsideClick: false,
      didOpen: () => {
        Swal.showLoading();
      }
    });

    // Call the enhanced login method that handles JWT processing
    this.userService.login(this.email, this.password).subscribe({
      next: (user) => {
        Swal.close();
        console.log('Login successful for user:', user);
        this.router.navigate(['/modules']);
      },
      error: (error) => {
        this.errorMessage = error.message || 'Invalid email or password. Please try again.';
        Swal.close();
        Swal.fire({
          icon: 'error',
          iconColor: '#dc3545',
          title: 'ERROR',
          text: this.errorMessage,
          confirmButtonColor: '#dc3545',
          confirmButtonText: '<i class="fa fa-thumbs-down me-2"></i> Dismiss',
          allowOutsideClick: false,
          buttonsStyling: false,
          customClass: {
            confirmButton: 'btn btn-outline-danger me-2',
          }
        });
      }
    });
  }
}

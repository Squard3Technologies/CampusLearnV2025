import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ApiService } from '../../services/api.service';
import Swal from 'sweetalert2';

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
  ) { }


  onTestDialog(): void {



    Swal.fire({
      icon: 'success',
      iconColor: '#dc3545',
      title: 'SUCCESS',
      html: '<p class="font-13">Registration successful. <br/>Once account has been approved by admin you will recieve email notification</p>',
      //color: "#198754",
      //background: "#198754",
      confirmButtonColor: '#fafafa',
      confirmButtonText: '<i class="fa fa-thumbs-up me-2"></i> Dismiss',
      allowOutsideClick: false,
      buttonsStyling: false,
      customClass: {
        confirmButton: 'btn btn-md btn-outline-danger me-2',
      }
    }).then((result) => {

    });
  }

  onSubmit(): void {
    // Check if all required fields are filled


    if (!this.firstName || !this.surname || !this.emailAddress || !this.password || !this.confirmPassword || !this.contactNumber) {
      this.errorMessage = 'Please fill in all fields';
      Swal.fire({
        icon: 'error',
        iconColor: '#AD0151',
        title: 'Validation Error',
        text: 'Please fill in all fields',
        confirmButtonColor: '#dc3545',
        confirmButtonText: '<i class="fa fa-thumbs-down me-2"></i> Dismiss',
        allowOutsideClick: false,
        buttonsStyling: false,
        customClass: {
          confirmButton: 'btn btn-md btn-outline-danger me-2',
        }
      });
      return;
    }

    // Check if passwords match
    if (this.password !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match';
      Swal.fire({
        icon: 'error',
        iconColor: '#AD0151',
        title: 'Validation Error',
        text: 'Passwords do not match',
        confirmButtonColor: '#dc3545',
        confirmButtonText: '<i class="fa fa-thumbs-down me-2"></i> Dismiss',
        allowOutsideClick: false,
        buttonsStyling: false,
        customClass: {
          confirmButton: 'btn btn-md btn-outline-danger me-2',
        }
      });
      return;
    }

    // Check if email is valid format
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(this.emailAddress)) {
      this.errorMessage = 'Please enter a valid email address';
      Swal.fire({
        icon: 'error',
        iconColor: '#AD0151',
        title: 'Validation Error',
        text: this.errorMessage,
        confirmButtonColor: '#dc3545',
        confirmButtonText: '<i class="fa fa-thumbs-down me-2"></i> Dismiss',
        allowOutsideClick: false,
        buttonsStyling: false,
        customClass: {
          confirmButton: 'btn btn-md btn-outline-danger me-2',
        }
      });
      return;
    }

    // Check if contact number is valid (basic check for numbers)
    if (!/^\d{10}$/.test(this.contactNumber)) {
      this.errorMessage = 'Please enter a valid 10-digit contact number';
      Swal.fire({
        icon: 'error',
        iconColor: '#dc3545',
        title: 'Validation Error',
        text: this.errorMessage,
        confirmButtonColor: '#dc3545',
        confirmButtonText: '<i class="fa fa-thumbs-down me-2"></i> Dismiss',
        allowOutsideClick: false,
        buttonsStyling: false,
        customClass: {
          confirmButton: 'btn btn-md btn-outline-danger me-2',
        }
      });
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
      role: 1
    };

    console.log('Registration data:', userData);
    Swal.fire({
      title: 'Creating account...',
      allowOutsideClick: false,
      didOpen: () => {
        Swal.showLoading(); // 
      }
    });

    // Call API registration service
    this.apiService.register(userData).subscribe({
      next: (response) => {
        console.log('Registration successful:', response);
        Swal.close();
        if (response != null) {
          if (!response.status) {
            Swal.fire({
              icon: 'error',
              iconColor: '#dc3545',
              title: 'REGISTRATION ERROR',
              text: response.statusMessage,
              //background: "#fafafa",
              //color: "#fafafa",
              confirmButtonColor: '#dc3545',
              confirmButtonText: '<i class="fa fa-thumbs-down me-2"></i> Dismiss',
              allowOutsideClick: false,
              buttonsStyling: false,
              customClass: {
                confirmButton: 'btn btn-outline-danger me-2',
              }
            });
            return;
          }
          Swal.fire({
            icon: 'success',
            iconColor: '#198754',
            title: 'SUCCESS',
            html: '<p class="font-13">Registration successful. <br/>Once account has been approved by admin you will recieve email notification</p>',
            confirmButtonColor: '#fafafa',
            confirmButtonText: '<i class="fa fa-thumbs-up me-2"></i> Dismiss',
            allowOutsideClick: false,
            buttonsStyling: false,
            customClass: {
              confirmButton: 'btn btn-md btn-outline-success me-2',
            }
          }).then((result) => {
            this.router.navigate(['/login']);
          });

        }
        else {
          Swal.fire({
            icon: 'error',
            iconColor: '#dc3545',
            title: 'ERROR',
            html: '<p class="font-13">Registration failed. <br/>Interna system error encountered</p>',
            confirmButtonColor: '#dc3545',
            confirmButtonText: '<i class="fa fa-thumbs-down me-2"></i> Dismiss',
            allowOutsideClick: false,
            buttonsStyling: false,
            customClass: {
              confirmButton: 'btn btn-md btn-outline-danger me-2',
            }
          }).then((result) => {

          });
        }
        this.router.navigate(['/login']);
      },
      error: (error) => {
        console.error('Registration error:', error);
        this.errorMessage = error.error?.message || 'Registration failed. Please try again.';
        Swal.close();
        Swal.fire({
          icon: 'error',
          iconColor: '#dc3545',
          title: 'REGISTRAION ERROR',
          text: this.errorMessage,
          confirmButtonColor: '#dc3545',
          confirmButtonText: '<i class="fa fa-thumbs-down me-2"></i> Dismiss',
          allowOutsideClick: false,
          buttonsStyling: false,
          customClass: {
            confirmButton: 'btn btn-md btn-outline-danger me-2',
          }
        });
      }
    });
  }
}

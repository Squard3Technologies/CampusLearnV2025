import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ApiService } from '../../services/api.service';
import Swal from 'sweetalert2';
import { SystemUser } from '../../models/api.models';

@Component({
  selector: 'app-admin-pending-registrations',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './admin-pending-registrations.component.html',
  styleUrl: './admin-pending-registrations.component.scss'
})
export class AdminPendingRegistrationsComponent implements OnInit {
  registrations: SystemUser[] = [];
  filteredRegistrations: SystemUser[] = [];
  pagedRegistrations: SystemUser[] = [];

  selectedUserViewModel: SystemUser = null;
  showUserViewModal: boolean = false;

  searchTerm: string = '';
  roleFilter: number = 0;
  statusFilter: string = '';

  currentPage: number = 1;
  pageSize: number = 5;
  totalPages: number = 1;


  constructor(
    private router: Router,
    private apiService: ApiService) { }


  ngOnInit(): void {
    this.loadRegistrations();
    this.filterRegistrations();
  }


  loadRegistrations(): void {
    this.apiService.getPendingRegistrations().subscribe({
      next: (response) => {
        if (!response.status && response.statusCode !== 404) {
          
          Swal.fire({
            icon: 'error',
            iconColor: '#AD0151',
            title: 'ERROR',
            text: response?.statusMessage || 'Retrieving pending registrations failed. Please try again.',
            confirmButtonColor: '#dc3545',
            confirmButtonText: '<i class="fa fa-thumbs-down me-2"></i> Dismiss',
            allowOutsideClick: false,
            buttonsStyling: false,
            customClass: {
              confirmButton: 'btn btn-md btn-outline-danger me-2',
            }
          });
          this.registrations = [];
        this.filterRegistrations();
          return;
        }

        if (response.body == null && response.statusCode === 404) {
          Swal.fire({
            icon: 'warning',
            iconColor: '#AD0151',
            title: 'NO DATA',
            text: response?.statusMessage || 'Retrieving pending registrations failed. Please try again.',
            confirmButtonColor: '#dc3545',
            confirmButtonText: '<i class="fa fa-thumbs-down me-2"></i> Dismiss',
            allowOutsideClick: false,
            buttonsStyling: false,
            customClass: {
              confirmButton: 'btn btn-md btn-outline-danger me-2',
            }
          });
          
          this.registrations = [];
        this.filterRegistrations();
          return;
        }

        debugger;
        response.body.forEach(f => {
          f.roleDescription = this.toSentenceCase(this.getRoleClass(f.role));
          f.accountStatusDescription = this.toSentenceCase(f.accountStatusDescription)
        });
        this.registrations = response.body;
        this.filterRegistrations();
      },
      error: (error) => {
        Swal.fire({
          icon: 'error',
          iconColor: '#AD0151',
          title: 'ERROR',
          text: error.error?.message || 'Retrieving pending registrations failed. Please try again.',
          confirmButtonColor: '#dc3545',
          confirmButtonText: '<i class="fa fa-thumbs-down me-2"></i> Dismiss',
          allowOutsideClick: false,
          buttonsStyling: false,
          customClass: {
            confirmButton: 'btn btn-md btn-outline-danger me-2',
          }
        });
        
          this.registrations = [];
        this.filterRegistrations();
      }
    });

  }


  filterRegistrations(): void {
    let filtered = this.registrations;

    console.log(`selected role: ${this.roleFilter}`);
    console.log(`select status: ${this.statusFilter}`);

    // Apply search filter
    if (this.searchTerm) {
      const term = this.searchTerm.toLowerCase();
      filtered = filtered.filter(user =>
        user.firstName.toLowerCase().includes(term) ||
        user.emailAddress.toLowerCase().includes(term)
      );
    }

    // Apply role filter
    debugger;
    if (this.roleFilter) {
      if (this.roleFilter !== 0) {
        filtered = filtered.filter(user => user.role === this.roleFilter);
      }
    }

    // Apply status filter
    debugger;
    if (this.statusFilter) {
      filtered = filtered.filter(user => user.accountStatusId.toLowerCase() === this.statusFilter.toLowerCase());
    }

    this.filteredRegistrations = filtered;
    this.updatePagination();
  }


  updatePagination(): void {
    this.totalPages = Math.ceil(this.filteredRegistrations.length / this.pageSize);
    if (this.currentPage > this.totalPages) {
      this.currentPage = 1;
    }

    this.totalPages = Math.ceil(this.filteredRegistrations.length / this.pageSize);

    // Bound currentPage to valid range
    if (this.currentPage > this.totalPages) this.currentPage = this.totalPages || 1;
    if (this.currentPage < 1) this.currentPage = 1;

    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.pagedRegistrations = this.filteredRegistrations.slice(startIndex, endIndex);
  }



  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.updatePagination();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updatePagination();
    }
  }


  navigateToUserManagement(): void {
    this.router.navigate(['/admin/users']);
  }


  navigateToModuleManagement(): void {
    this.router.navigate(['/admin/modules']);
  }


  openViewUserModal(user: SystemUser): void {
    this.selectedUserViewModel = user;
    this.showUserViewModal = true;

  }

  processApplication(userId: string, accountStatus: string) {
    this.closeUserViewModal(false);
    console.log('edit user data: ', JSON.stringify(this.selectedUserViewModel));
    Swal.fire({
      title: 'Saving user details changes...',
      allowOutsideClick: false,
      didOpen: () => {
        Swal.showLoading();
      }
    });
    
    
    // Call API registration service
    this.apiService.processRegistration(userId, accountStatus).subscribe({
      next: (response) => {
        console.log('process registration successful:', response);
        Swal.close();
        if (response != null) {
          if (!response.status) {
            this.openViewUserModal(this.selectedUserViewModel);
            Swal.fire({
              icon: 'error',
              iconColor: '#dc3545',
              title: 'ERROR',
              text: response.statusMessage,
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
            html: response.statusMessage,
            confirmButtonColor: '#fafafa',
            confirmButtonText: '<i class="fa fa-thumbs-up me-2"></i> Dismiss',
            allowOutsideClick: false,
            buttonsStyling: false,
            customClass: {
              confirmButton: 'btn btn-md btn-outline-success me-2',
            }
          }).then((result) => {
            this.selectedUserViewModel = null
            this.loadRegistrations();
          });

        }
        else {
          this.openViewUserModal(this.selectedUserViewModel);
          Swal.fire({
            icon: 'error',
            iconColor: '#dc3545',
            title: 'ERROR',
            html: '<p class="font-13">Updating user failed. <br/>Internal system error encountered</p>',
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

      },
      error: (error) => {
        this.openViewUserModal(this.selectedUserViewModel);
        Swal.close();
        Swal.fire({
          icon: 'error',
          iconColor: '#dc3545',
          title: 'ERROR',
          text: error.error?.message || 'user update failed. Please try again.',
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

  submitDecisionModal(approved: boolean) {

    if (!this.selectedUserViewModel.accountStatusId) {
      Swal.fire({
        icon: 'error',
        iconColor: '#AD0151',
        title: 'Validation Error',
        text: 'Please select account status',
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


    this.closeUserViewModal(false);
    console.log('edit user data: ', JSON.stringify(this.selectedUserViewModel));
    Swal.fire({
      title: 'Saving user details changes...',
      allowOutsideClick: false,
      didOpen: () => {
        Swal.showLoading();
      }
    });

    const model = {
      id: this.selectedUserViewModel.id,
      firstName: this.selectedUserViewModel.accountStatusId
    }
    console.log('edit user json: ', JSON.stringify(model));
    // Call API registration service
    this.apiService.processRegistration(this.selectedUserViewModel.id, this.selectedUserViewModel.accountStatusId).subscribe({
      next: (response) => {
        console.log('process registration successful:', response);
        Swal.close();
        if (response != null) {
          if (!response.status) {
            this.openViewUserModal(this.selectedUserViewModel);
            Swal.fire({
              icon: 'error',
              iconColor: '#dc3545',
              title: 'ERROR',
              text: response.statusMessage,
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
            html: response.statusMessage,
            confirmButtonColor: '#fafafa',
            confirmButtonText: '<i class="fa fa-thumbs-up me-2"></i> Dismiss',
            allowOutsideClick: false,
            buttonsStyling: false,
            customClass: {
              confirmButton: 'btn btn-md btn-outline-success me-2',
            }
          }).then((result) => {
            this.selectedUserViewModel = null
            this.loadRegistrations();
          });

        }
        else {
          this.openViewUserModal(this.selectedUserViewModel);
          Swal.fire({
            icon: 'error',
            iconColor: '#dc3545',
            title: 'ERROR',
            html: '<p class="font-13">Updating user failed. <br/>Internal system error encountered</p>',
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

      },
      error: (error) => {
        this.openViewUserModal(this.selectedUserViewModel);
        Swal.close();
        Swal.fire({
          icon: 'error',
          iconColor: '#dc3545',
          title: 'ERROR',
          text: error.error?.message || 'user update failed. Please try again.',
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



  closeUserViewModal(clearFiels: boolean) {
    if (clearFiels) {
      this.selectedUserViewModel = null;
    }
    this.showUserViewModal = false;
  }



  approveRegistration(registration: SystemUser): void {
    if (confirm(`Are you sure you want to approve ${registration.firstName}'s registration?`)) {
      this.registrations = this.registrations.filter(r => r.id !== registration.id);
      this.filterRegistrations();
      console.log('Approved registration:', registration);
      // Implement API call to approve registration
    }
  }


  rejectRegistration(registration: SystemUser): void {
    const reason = prompt(`Please provide a reason for rejecting ${registration.firstName}'s registration:`);
    if (reason) {
      this.registrations = this.registrations.filter(r => r.id !== registration.id);
      this.filterRegistrations();
      console.log('Rejected registration:', registration, 'Reason:', reason);
      // Implement API call to reject registration
    }
  }


  getRoleClass(role: number): string {
    const classes: { [key: number]: string } = {
      1: 'Administrator',
      2: 'Lecturer',
      3: 'Tutor',
      4: 'Student'
    };
    return classes[role] || '';
  }

  getStatusClass(status: string): string {
    const classes: { [key: string]: string } = {
      'B492BB30-B073-4059-A58B-B3F6E5BE4C95': 'Active',
      '2C1904BB-07F2-4A0E-8CB4-ECB768239D19': 'Inactive',
      '114F6BEE-6EF5-47CA-B2B5-113106C1E5B2': this.toSentenceCase('DELETED'),
      'DF799A11-8237-4EEE-AC51-94FCEB369978': this.toSentenceCase('LOCKED'),
      '285DE8D3-0DA3-4D50-A32D-3502010062E7': this.toSentenceCase('REJECTED'),
    };
    return classes[status] || '';
  }


  // Summary statistics methods
  getTotalPendingCount(): number {
    return this.registrations.length;
  }


  getStudentRequestsCount(): number {
    return this.registrations.filter(reg => reg.role === 4).length;
  }


  getTutorRequestsCount(): number {
    return this.registrations.filter(reg => reg.role === 3).length;
  }




  // Add Math to component for template access
  Math = Math;


  private toSentenceCase(str: string): string {
    if (!str) return '';
    str = str.toLowerCase();
    return str.charAt(0).toUpperCase() + str.slice(1);
  }


}

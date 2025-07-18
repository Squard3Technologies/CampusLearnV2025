import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ApiService } from '../../services/api.service';
import Swal from 'sweetalert2';
import { SystemUser } from '../../models/api.models';


@Component({
  selector: 'app-admin-user-management',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './admin-user-management.component.html',
  styleUrl: './admin-user-management.component.scss'
})
export class AdminUserManagementComponent implements OnInit {
  users: SystemUser[] = [];
  filteredUsers: SystemUser[] = [];
  pagedUsers: any[] = [];
  searchTerm: string = '';
  roleFilter: number = 0;
  statusFilter: string = '';

  currentPage: number = 1;
  pageSize: number = 5;
  totalPages: number = 1;

  editUserModel: SystemUser
  showEditUserModal: boolean = false;

  constructor(
    private router: Router,
    private apiService: ApiService
  ) { }

  ngOnInit(): void {
    this.filterUsers();
    this.loadUsers();
  }

  loadUsers(): void {
    // Mock user data
    this.apiService.getAdminUsers().subscribe({
      next: (response) => {
        if (!response.status) {
          Swal.fire({
            icon: 'error',
            iconColor: '#AD0151',
            title: 'REGISTRAION ERROR',
            text: response?.statusMessage || 'Retrieving admin users failed. Please try again.',
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

        if (response.body == null) {
          Swal.fire({
            icon: 'error',
            iconColor: '#AD0151',
            title: 'REGISTRAION ERROR',
            text: response?.statusMessage || 'Retrieving admin users failed. Please try again.',
            confirmButtonColor: '#dc3545',
            confirmButtonText: '<i class="fa fa-thumbs-uo me-2"></i> Dismiss',
            allowOutsideClick: false,
            buttonsStyling: false,
            customClass: {
              confirmButton: 'btn btn-md btn-outline-success me-2',
            }
          });
          return;
        }

        debugger;
        response.body.forEach(f => {
          f.roleDescription = this.toSentenceCase(this.getRoleClass(f.role));
          f.accountStatusDescription = this.toSentenceCase(f.accountStatusDescription)
        });
        this.users = response.body;
        this.filterUsers();
      },
      error: (error) => {
        Swal.fire({
          icon: 'error',
          iconColor: '#AD0151',
          title: 'REGISTRAION ERROR',
          text: error.error?.message || 'Retrieving admin users failed. Please try again.',
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



  filterUsers(): void {
    let filtered = this.users;

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

    this.filteredUsers = filtered;
    this.updatePagination();
  }

  updatePagination(): void {
    this.totalPages = Math.ceil(this.filteredUsers.length / this.pageSize);
    if (this.currentPage > this.totalPages) {
      this.currentPage = 1;
    }

    this.totalPages = Math.ceil(this.filteredUsers.length / this.pageSize);

    // Bound currentPage to valid range
    if (this.currentPage > this.totalPages) this.currentPage = this.totalPages || 1;
    if (this.currentPage < 1) this.currentPage = 1;

    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.pagedUsers = this.filteredUsers.slice(startIndex, endIndex);
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

  navigateToPendingRegistrations(): void {
    this.router.navigate(['/admin/registrations']);
  }

  navigateToModuleManagement(): void {
    this.router.navigate(['/admin/modules']);
  }

  openEditUserModal(user: SystemUser): void {
    //console.log('Editing user:', user);
    this.editUserModel = user;
    this.showEditUserModal = true;
  }

  closeEditUserModal(clearFiels: boolean) {
    if (clearFiels) {
      this.editUserModel = null;
    }
    this.showEditUserModal = false;
  }


  submitEditUserModal() {
    if (!this.editUserModel.firstName || !this.editUserModel.surname || !this.editUserModel.emailAddress || !this.editUserModel.contactNumber
      || !this.editUserModel.role || !this.editUserModel.accountStatusId
    ) {
      //this.errorMessage = 'Please fill in all fields';
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


    this.closeEditUserModal(false);
    console.log('edit user data: ', JSON.stringify(this.editUserModel));
    Swal.fire({
      title: 'Saving user details changes...',
      allowOutsideClick: false,
      didOpen: () => {
        Swal.showLoading();
      }
    });

    const model = {
      id: this.editUserModel.id,
      firstName: this.editUserModel.firstName,
      middleName: this.editUserModel.middleName,
      lastName: this.editUserModel.surname,
      emailAddress: this.editUserModel.emailAddress,
      contactNumber: this.editUserModel.contactNumber,
      password: "",
    }
    console.log('edit user json: ', JSON.stringify(model));
    // Call API registration service
    this.apiService.updateUserByAdmin(model).subscribe({
      next: (response) => {
        console.log('module update successful:', response);
        Swal.close();
        if (response != null) {
          if (!response.status) {
            this.openEditUserModal(this.editUserModel);
            Swal.fire({
              icon: 'error',
              iconColor: '#dc3545',
              title: 'UPDATE USER ERROR',
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
            this.editUserModel = null
            this.loadUsers();
          });

        }
        else {
          this.openEditUserModal(this.editUserModel);
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
        this.openEditUserModal(this.editUserModel);
        Swal.close();
        Swal.fire({
          icon: 'error',
          iconColor: '#dc3545',
          title: 'UPDATE USER ERROR',
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


  toggleUserStatus(user: SystemUser): void {

    var tempusers = this.users.filter(f => f.id !== user.id);
    var currentStatus = user.accountStatusDescription;
    user.accountStatusDescription = currentStatus === 'Active' ? 'Inactive' : 'Active';
    user.accountStatusId = currentStatus === 'Active' ? '2C1904BB-07F2-4A0E-8CB4-ECB768239D19' : 'B492BB30-B073-4059-A58B-B3F6E5BE4C95';
    tempusers.push(user);
    this.users = tempusers;
    console.log('Toggled user status:', JSON.stringify(user));
    console.log('users:', JSON.stringify(this.users));
    // Implement API call to update user status
  }

  toggleUserStatusExt(user: SystemUser, status: string): void {
    // Implement API call to update user status
    this.apiService.changeUserAccountStatus(user.id, status).subscribe({
      next: (response) => {
        this.loadUsers();
      }
    });


  }

  deleteUser(user: SystemUser): void {
    if (confirm(`Are you sure you want to delete user ${user.firstName}?`)) {
      this.users = this.users.filter(u => u.id !== user.id);
      this.filterUsers();
      console.log('Deleted user:', user);
      // Implement API call to delete user
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
  getTotalUsersCount(): number {
    return this.users.length;
  }

  getActiveUsersCount(): number {
    return this.users.filter(user => user.accountStatusDescription === 'Active').length;
  }

  getInactiveUsersCount(): number {
    return this.users.filter(user => user.accountStatusDescription === 'Inactive').length;
  }

  getAdminUsersCount(): number {
    return this.users.filter(user => user.role === 1).length;
  }

  private toSentenceCase(str: string): string {
    if (!str) return '';
    str = str.toLowerCase();
    return str.charAt(0).toUpperCase() + str.slice(1);
  }

  // Add Math to component for template access
  Math = Math;
}

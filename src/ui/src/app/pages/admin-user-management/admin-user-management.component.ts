import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';

interface User {
  id: number;
  name: string;
  email: string;
  role: 'Student' | 'Tutor' | 'Admin';
  status: 'Active' | 'Inactive';
  lastLogin: Date;
  createdDate: Date;
}

@Component({
  selector: 'app-admin-user-management',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './admin-user-management.component.html',
  styleUrl: './admin-user-management.component.scss'
})
export class AdminUserManagementComponent implements OnInit {
  users: User[] = [];
  filteredUsers: User[] = [];
  searchTerm: string = '';
  roleFilter: string = '';
  statusFilter: string = '';
  currentPage: number = 1;
  pageSize: number = 10;
  totalPages: number = 1;

  constructor(private router: Router) {}

  ngOnInit(): void {
    this.loadUsers();
    this.filterUsers();
  }

  loadUsers(): void {
    // Mock user data
    this.users = [
      {
        id: 1,
        name: 'John Smith',
        email: 'john.smith@university.edu',
        role: 'Student',
        status: 'Active',
        lastLogin: new Date('2024-02-15T10:30:00'),
        createdDate: new Date('2024-01-10')
      },
      {
        id: 2,
        name: 'Dr. Sarah Johnson',
        email: 'sarah.johnson@university.edu',
        role: 'Tutor',
        status: 'Active',
        lastLogin: new Date('2024-02-14T16:45:00'),
        createdDate: new Date('2023-09-01')
      },
      {
        id: 3,
        name: 'Michael Brown',
        email: 'michael.brown@university.edu',
        role: 'Student',
        status: 'Inactive',
        lastLogin: new Date('2024-01-20T09:15:00'),
        createdDate: new Date('2024-01-05')
      },
      {
        id: 4,
        name: 'Admin User',
        email: 'admin@university.edu',
        role: 'Admin',
        status: 'Active',
        lastLogin: new Date('2024-02-15T08:00:00'),
        createdDate: new Date('2023-08-01')
      },
      {
        id: 5,
        name: 'Emily Davis',
        email: 'emily.davis@university.edu',
        role: 'Student',
        status: 'Active',
        lastLogin: new Date('2024-02-13T14:20:00'),
        createdDate: new Date('2024-01-15')
      },
      {
        id: 6,
        name: 'Prof. Robert Wilson',
        email: 'robert.wilson@university.edu',
        role: 'Tutor',
        status: 'Active',
        lastLogin: new Date('2024-02-12T11:30:00'),
        createdDate: new Date('2023-09-15')
      },
      {
        id: 7,
        name: 'Jessica Martinez',
        email: 'jessica.martinez@university.edu',
        role: 'Student',
        status: 'Active',
        lastLogin: new Date('2024-02-11T13:45:00'),
        createdDate: new Date('2024-01-20')
      },
      {
        id: 8,
        name: 'David Anderson',
        email: 'david.anderson@university.edu',
        role: 'Student',
        status: 'Inactive',
        lastLogin: new Date('2024-01-25T16:00:00'),
        createdDate: new Date('2024-01-08')
      }
    ];
  }

  filterUsers(): void {
    let filtered = this.users;

    // Apply search filter
    if (this.searchTerm) {
      const term = this.searchTerm.toLowerCase();
      filtered = filtered.filter(user => 
        user.name.toLowerCase().includes(term) || 
        user.email.toLowerCase().includes(term)
      );
    }

    // Apply role filter
    if (this.roleFilter) {
      filtered = filtered.filter(user => user.role === this.roleFilter);
    }

    // Apply status filter
    if (this.statusFilter) {
      filtered = filtered.filter(user => user.status === this.statusFilter);
    }

    this.filteredUsers = filtered;
    this.updatePagination();
  }

  updatePagination(): void {
    this.totalPages = Math.ceil(this.filteredUsers.length / this.pageSize);
    if (this.currentPage > this.totalPages) {
      this.currentPage = 1;
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }
  navigateToPendingRegistrations(): void {
    this.router.navigate(['/admin/registrations']);
  }

  navigateToModuleManagement(): void {
    this.router.navigate(['/admin/modules']);
  }

  editUser(user: User): void {
    console.log('Editing user:', user);
    // Implement edit user functionality
  }

  toggleUserStatus(user: User): void {
    user.status = user.status === 'Active' ? 'Inactive' : 'Active';
    console.log('Toggled user status:', user);
    // Implement API call to update user status
  }

  deleteUser(user: User): void {
    if (confirm(`Are you sure you want to delete user ${user.name}?`)) {
      this.users = this.users.filter(u => u.id !== user.id);
      this.filterUsers();
      console.log('Deleted user:', user);
      // Implement API call to delete user
    }
  }

  getRoleClass(role: string): string {
    const classes: { [key: string]: string } = {
      'Student': 'role-student',
      'Tutor': 'role-tutor',
      'Admin': 'role-admin'
    };
    return classes[role] || '';
  }

  getStatusClass(status: string): string {
    const classes: { [key: string]: string } = {
      'Active': 'status-active',
      'Inactive': 'status-inactive'
    };
    return classes[status] || '';
  }

  // Summary statistics methods
  getTotalUsersCount(): number {
    return this.users.length;
  }

  getActiveUsersCount(): number {
    return this.users.filter(user => user.status === 'Active').length;
  }

  getInactiveUsersCount(): number {
    return this.users.filter(user => user.status === 'Inactive').length;
  }

  getAdminUsersCount(): number {
    return this.users.filter(user => user.role === 'Admin').length;
  }

  // Add Math to component for template access
  Math = Math;
}

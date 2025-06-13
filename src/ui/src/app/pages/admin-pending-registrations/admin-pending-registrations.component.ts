import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';

interface Document {
  id: number;
  name: string;
  type: string;
  url: string;
}

interface PendingRegistration {
  id: number;
  fullName: string;
  email: string;
  requestedRole: 'Student' | 'Tutor';
  registrationDate: Date;
  documents: Document[];
  reason?: string;
  institution?: string;
  phoneNumber?: string;
}

@Component({
  selector: 'app-admin-pending-registrations',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './admin-pending-registrations.component.html',
  styleUrl: './admin-pending-registrations.component.scss'
})
export class AdminPendingRegistrationsComponent implements OnInit {
  registrations: PendingRegistration[] = [];
  filteredRegistrations: PendingRegistration[] = [];
  searchTerm: string = '';
  roleFilter: string = '';
  dateFilter: string = '';
  currentPage: number = 1;
  pageSize: number = 10;
  totalPages: number = 1;

  constructor(private router: Router) {}

  
  ngOnInit(): void {
    this.loadRegistrations();
    this.filterRegistrations();
  }

  loadRegistrations(): void {
    // Mock pending registration data
    this.registrations = [
      {
        id: 1,
        fullName: 'Alice Johnson',
        email: 'alice.johnson@email.com',
        requestedRole: 'Student',
        registrationDate: new Date('2024-02-14T09:30:00'),
        documents: [
          { id: 1, name: 'Student_ID.pdf', type: 'ID', url: '/docs/student_id_1.pdf' },
          { id: 2, name: 'Transcript.pdf', type: 'Academic', url: '/docs/transcript_1.pdf' }
        ],
        institution: 'Springfield University',
        phoneNumber: '+1-555-0123'
      },
      {
        id: 2,
        fullName: 'Dr. Robert Chen',
        email: 'robert.chen@university.edu',
        requestedRole: 'Tutor',
        registrationDate: new Date('2024-02-13T14:20:00'),
        documents: [
          { id: 3, name: 'PhD_Certificate.pdf', type: 'Qualification', url: '/docs/phd_cert_2.pdf' },
          { id: 4, name: 'CV.pdf', type: 'CV', url: '/docs/cv_2.pdf' },
          { id: 5, name: 'References.pdf', type: 'Reference', url: '/docs/refs_2.pdf' }
        ],
        institution: 'Tech Institute',
        phoneNumber: '+1-555-0456',
        reason: 'Looking to teach computer science courses'
      },
      {
        id: 3,
        fullName: 'Maria Garcia',
        email: 'maria.garcia@student.com',
        requestedRole: 'Student',
        registrationDate: new Date('2024-02-12T11:15:00'),
        documents: [
          { id: 6, name: 'High_School_Diploma.pdf', type: 'Academic', url: '/docs/diploma_3.pdf' }
        ],
        institution: 'Central High School',
        phoneNumber: '+1-555-0789'
      },
      {
        id: 4,
        fullName: 'Prof. Sarah Williams',
        email: 'sarah.williams@academia.org',
        requestedRole: 'Tutor',
        registrationDate: new Date('2024-02-11T16:45:00'),
        documents: [
          { id: 7, name: 'Teaching_License.pdf', type: 'License', url: '/docs/license_4.pdf' },
          { id: 8, name: 'Publications.pdf', type: 'Research', url: '/docs/pubs_4.pdf' }
        ],
        institution: 'State University',
        phoneNumber: '+1-555-0321',
        reason: 'Experienced educator seeking online teaching opportunities'
      },
      {
        id: 5,
        fullName: 'James Anderson',
        email: 'james.anderson@email.com',
        requestedRole: 'Student',
        registrationDate: new Date('2024-02-10T08:30:00'),
        documents: [],
        institution: 'Community College',
        phoneNumber: '+1-555-0654'
      },
      {
        id: 6,
        fullName: 'Lisa Thompson',
        email: 'lisa.thompson@professional.com',
        requestedRole: 'Tutor',
        registrationDate: new Date('2024-02-09T13:20:00'),
        documents: [
          { id: 9, name: 'Masters_Degree.pdf', type: 'Qualification', url: '/docs/masters_6.pdf' },
          { id: 10, name: 'Industry_Experience.pdf', type: 'Experience', url: '/docs/exp_6.pdf' }
        ],
        institution: 'Business School',
        phoneNumber: '+1-555-0987',
        reason: 'Industry professional wanting to share knowledge'
      }
    ];
  }

  filterRegistrations(): void {
    let filtered = this.registrations;

    // Apply search filter
    if (this.searchTerm) {
      const term = this.searchTerm.toLowerCase();
      filtered = filtered.filter(reg => 
        reg.fullName.toLowerCase().includes(term) || 
        reg.email.toLowerCase().includes(term)
      );
    }

    // Apply role filter
    if (this.roleFilter) {
      filtered = filtered.filter(reg => reg.requestedRole === this.roleFilter);
    }

    // Apply date filter
    if (this.dateFilter) {
      const now = new Date();
      const today = new Date(now.getFullYear(), now.getMonth(), now.getDate());
      
      filtered = filtered.filter(reg => {
        const regDate = new Date(reg.registrationDate);
        
        switch (this.dateFilter) {
          case 'today':
            return regDate >= today;
          case 'week':
            const weekAgo = new Date(today.getTime() - 7 * 24 * 60 * 60 * 1000);
            return regDate >= weekAgo;
          case 'month':
            const monthAgo = new Date(today.getTime() - 30 * 24 * 60 * 60 * 1000);
            return regDate >= monthAgo;
          default:
            return true;
        }
      });
    }

    this.filteredRegistrations = filtered;
    this.updatePagination();
  }

  updatePagination(): void {
    this.totalPages = Math.ceil(this.filteredRegistrations.length / this.pageSize);
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

  navigateToUserManagement(): void {
    this.router.navigate(['/admin/users']);
  }

  navigateToModuleManagement(): void {
    this.router.navigate(['/admin/modules']);
  }

  viewDetails(registration: PendingRegistration): void {
    console.log('Viewing details for:', registration);
    // Implement view details modal or navigate to details page
  }

  approveRegistration(registration: PendingRegistration): void {
    if (confirm(`Are you sure you want to approve ${registration.fullName}'s registration?`)) {
      this.registrations = this.registrations.filter(r => r.id !== registration.id);
      this.filterRegistrations();
      console.log('Approved registration:', registration);
      // Implement API call to approve registration
    }
  }

  rejectRegistration(registration: PendingRegistration): void {
    const reason = prompt(`Please provide a reason for rejecting ${registration.fullName}'s registration:`);
    if (reason) {
      this.registrations = this.registrations.filter(r => r.id !== registration.id);
      this.filterRegistrations();
      console.log('Rejected registration:', registration, 'Reason:', reason);
      // Implement API call to reject registration
    }
  }

  getRoleClass(role: string): string {
    const classes: { [key: string]: string } = {
      'Student': 'role-student',
      'Tutor': 'role-tutor'
    };
    return classes[role] || '';
  }

  // Summary statistics methods
  getTotalPendingCount(): number {
    return this.registrations.length;
  }

  getStudentRequestsCount(): number {
    return this.registrations.filter(reg => reg.requestedRole === 'Student').length;
  }

  getTutorRequestsCount(): number {
    return this.registrations.filter(reg => reg.requestedRole === 'Tutor').length;
  }

  getThisWeekCount(): number {
    const weekAgo = new Date();
    weekAgo.setDate(weekAgo.getDate() - 7);
    return this.registrations.filter(reg => 
      new Date(reg.registrationDate) >= weekAgo
    ).length;
  }

  // Add Math to component for template access
  Math = Math;
}

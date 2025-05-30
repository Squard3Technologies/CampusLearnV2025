import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';

interface Module {
  id: number;
  name: string;
  description: string;
  category: string;
  status: 'Active' | 'Inactive' | 'Draft';
  level: 'Beginner' | 'Intermediate' | 'Advanced';
  instructor: string;
  createdBy: string;
  createdDate: Date;
  lastModified: Date;
  topicsCount: number;
  enrolledStudents: number;
  enrollments: number;
}

@Component({
  selector: 'app-admin-module-management',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './admin-module-management.component.html',
  styleUrl: './admin-module-management.component.scss'
})
export class AdminModuleManagementComponent implements OnInit {
  modules: Module[] = [];
  filteredModules: Module[] = [];

  constructor(private router: Router) {}

  ngOnInit(): void {
    this.loadModules();
  }

  loadModules(): void {
    // No data for now - empty table
    this.modules = [];
    this.filteredModules = [];
  }

  navigateToUserManagement(): void {
    this.router.navigate(['/admin/users']);
  }
  navigateToPendingRegistrations(): void {
    this.router.navigate(['/admin/registrations']);
  }

  createModule(): void {
    console.log('Creating new module');
    // Implement create module functionality
  }
}

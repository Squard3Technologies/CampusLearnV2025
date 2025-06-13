import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { ApiService } from '../../services/api.service'; // Import your ApiService

@Component({
  selector: 'app-enquiries-management',
  standalone: true,
  imports: [RouterModule, CommonModule, FormsModule],
  templateUrl: './enquiries-management.component.html',
  styleUrl: './enquiries-management.component.scss'
})
export class EnquiriesManagementComponent implements OnInit {
  isViewModalOpen: boolean = false;
  selectedEnquiry: any = {};
  allEnquiries: any[] = []; // Renamed for clarity

  constructor(private router: Router, private api: ApiService) {}

  ngOnInit(): void {
    this.api.getEnquiries().subscribe({
      next: (data: any) => {
        // If data is an array, use it directly. If it's an object with a property, adjust accordingly.
        const enquiries = Array.isArray(data) ? data : data?.enquiries || [];
        this.allEnquiries = enquiries; // No filter, show all
      },
      error: (err) => {
        console.error('Failed to load enquiries:', err);
      }
    });
  }

  openViewModal(inquiry: any) {
    this.selectedEnquiry = { ...inquiry };
    this.isViewModalOpen = true;
  }

  closeViewModal() {
    this.isViewModalOpen = false;
  }

  navigateToHistory() {
    this.router.navigate(['/enquiries-history']);
  }
}

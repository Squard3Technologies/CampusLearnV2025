import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-enquiries-management',
  standalone: true,
  imports: [RouterModule, CommonModule, FormsModule],
  templateUrl: './enquiries-management.component.html',
  styleUrl: './enquiries-management.component.scss'
})
export class EnquiriesManagementComponent {
  isViewModalOpen: boolean = false;
  selectedEnquiry: any = {};

  constructor(private router: Router) {}

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

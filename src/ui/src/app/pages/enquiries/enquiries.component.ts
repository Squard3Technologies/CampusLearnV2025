import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { ActivatedRoute } from '@angular/router';
import { EnquiryStatus } from '../../enums/enums';

@Component({
  selector: 'app-enquiries',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './enquiries.component.html',
  styleUrls: ['./enquiries.component.scss']
})
export class EnquiriesComponent implements OnInit {
  showModal = false;
  enquiryTitle = '';
  enquiryDescription = '';

  isViewModalOpen = false;
  selectedEnquiry: any = {};
  Enquiries: any[] = [];

  moduleId: string | null = null;

  constructor(
    private api: ApiService,
    private route: ActivatedRoute // Add this
  ) {}

  ngOnInit(): void {
    this.route.queryParamMap.subscribe(params => {
      this.moduleId = params.get('moduleId');
    });
    this.loadEnquiries();
  }

  /** Loads enquiries from the backend API */
  loadEnquiries() {
    this.api.getEnquiries().subscribe({
      next: (data: any) => {
        this.Enquiries = data;
      },
      error: (err) => {
        console.error('Failed to load enquiries:', err);
      }
    });
  }

  /** Opens modal for new enquiry */
  openModal() {
    this.showModal = true;
  }

  /** Closes modal */
  closeModal() {
    this.showModal = false;
  }

  /** Submits a new enquiry to the API */
  submitEnquiry() {
  if (!this.moduleId) {
    console.error('Module ID is missing or invalid!');
    return;
  }

  // Validate GUID format (optional, but recommended)
  const guidRegex = /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/;
  if (!guidRegex.test(this.moduleId)) {
    console.error('Module ID is not a valid GUID:', this.moduleId);
    return;
  }

  if (!this.enquiryTitle.trim() || !this.enquiryDescription.trim()) {
    console.error('Title and Description are required!');
    return;
  }

  // Use PascalCase property names
  const enquiryData = {
    ModuleId: this.moduleId,
    Title: this.enquiryTitle,
    Description: this.enquiryDescription,
  };

  console.log('Submitting enquiry:', enquiryData);

  this.api.createEnquiry(enquiryData).subscribe({
    next: () => {
      console.log('Created successfully');
      this.loadEnquiries();
      this.enquiryTitle = '';
      this.enquiryDescription = '';
      this.closeModal();
    },
    error: (err) => {
      console.error('Failed to create enquiry:', err);
    }
  });
}

  /** Opens modal to view a specific enquiry */
  openViewModal(inquiry: any) {
    this.selectedEnquiry = { ...inquiry };
    this.isViewModalOpen = true;
  }

  /** Closes the view modal */
  closeViewModal() {
    this.isViewModalOpen = false;
  }

  getStatusClass(status: number): string {
    return 'status-' + this.resolveStatus(status).toLowerCase();
  }

  resolveStatus(status: number): string {
    return EnquiryStatus[status] ?? 'Unknown';
  }
}

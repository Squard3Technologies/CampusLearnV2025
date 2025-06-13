import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { Enquiry } from '../../models/enquiry.models';

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
  currentTopics: any[] = [];
  activeEnquiries: any[] = [];

  constructor(private router: Router, private apiService: ApiService) {}

  ngOnInit(): void {
    this.loadActiveEnquiries();
  }

  loadActiveEnquiries() {
    this.apiService.getActiveEnquiries().subscribe({
      next: (data: any) => {
        const enquiries = Array.isArray(data) ? data : data?.enquiries || [];
        this.activeEnquiries = enquiries; // No filter, show all
      },
      error: (err) => {
        console.error('Failed to load enquiries:', err);
      }
    });
  }

  openViewModal(enquiry: any) {
    this.currentTopics = [];
    this.selectedEnquiry = { ...enquiry };
    this.isViewModalOpen = true;

    this.apiService.getTopics(enquiry.moduleId).subscribe((x: any) =>
      this.currentTopics = x.body
    );
  }

  closeViewModal() {
    this.isViewModalOpen = false;
    this.currentTopics = [];
  }

  navigateToHistory() {
    this.router.navigate(['/enquiries/history']);
  }

  resolveEnquiry(enquiry: Enquiry) {
    if (enquiry.resolutionAction != 2)
      enquiry.linkedTopicId = null;
    
    const payload: any = {
      resolutionAction: Number(enquiry.resolutionAction),
      resolutionResponse: enquiry.resolutionResponse
    };

    if (enquiry.linkedTopicId) {
      payload.linkedTopicId = enquiry.linkedTopicId;
    }

    this.apiService.resolveEnquiry(enquiry.id, payload).subscribe(x => {
      this.loadActiveEnquiries();
      this.closeViewModal();
    });
  }
}

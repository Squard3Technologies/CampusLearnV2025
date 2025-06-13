import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { Enquiry } from '../../models/enquiry.models';

@Component({
  selector: 'app-enquiries-history',
  standalone: true,
  imports: [RouterModule, FormsModule, CommonModule ],
  templateUrl: './enquiries-history.component.html',
  styleUrl: './enquiries-history.component.scss',
})
export class EnquiriesHistoryComponent {
  showModal = false;
  enquiryTitle = '';
  enquiryDescription = '';

  isViewModalOpen = false;
  selectedEnquiry: any = {};

  resolvedEnquiries: Enquiry[] = [];

  constructor(private router: Router, private api: ApiService) {}

  ngOnInit(): void {
    this.api.getResolvedEnquiries().subscribe((x: any) => {
      this.resolvedEnquiries = x
    });
  }

  navigateToEnquiryManagement() {
    this.router.navigate(['/admin/enquiries']);
  }

  /** Opens modal to view a specific enquiry */
  openViewModal(enquiry: any) {
    this.selectedEnquiry = { ...enquiry };
    this.isViewModalOpen = true;
  }

  /** Closes the view modal */
  closeViewModal() {
    this.isViewModalOpen = false;
  }
}

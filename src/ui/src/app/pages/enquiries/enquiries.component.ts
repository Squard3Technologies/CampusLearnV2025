import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpHeaders } from '@angular/common/http';
import { ApiService } from '../../services/api.service';
import { UserService } from '../../services/user.service';
import { ActivatedRoute } from '@angular/router';

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
    private userService: UserService, 
    private route: ActivatedRoute // Add this
  ) {}

  ngOnInit(): void {
    this.route.queryParamMap.subscribe(params => {
      this.moduleId = params.get('moduleId');
    });
    this.loadEnquiries();
  }
  /** Builds Authorization headers using token from localStorage */
  getAuthHeaders(): { headers: HttpHeaders } {
    const token = localStorage.getItem('token');
    return {
      headers: new HttpHeaders({
        'Authorization': `Bearer ${token}`
      })
    };
  }

  /** Loads enquiries from the backend API */
  loadEnquiries() {
    var token = this.userService.getAuthToken() as string;

    this.api.getEnquiries(token).subscribe({
      next: (data: any) => {
        console.log('Enquiries from API:', data);
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
    const enquiryData = {
      title: this.enquiryTitle,
      description: this.enquiryDescription,
      moduleId: this.moduleId // Add moduleId to the payload
    };

    console.log('Submitting enquiry:', enquiryData);
    
    var token = this.userService.getAuthToken() as string;

    this.api.createEnquiry(enquiryData, token).subscribe({
      next: () => {
        console.log('Created successfully');
        this.loadEnquiries(); // Refresh list
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
}

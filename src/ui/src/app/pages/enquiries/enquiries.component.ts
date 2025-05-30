import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-enquiries',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './enquiries.component.html',
  styleUrl: './enquiries.component.scss'
})
export class EnquiriesComponent {
  showModal = false;
  enquiryTitle = '';
  enquiryDescription = '';

  openModal() {
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }

  submitEnquiry() {
    console.log('Enquiry submitted:', this.enquiryTitle, this.enquiryDescription);

    // Reset form and close modal
    this.enquiryTitle = '';
    this.enquiryDescription = '';
    this.showModal = false;
  }          
  
  isViewModalOpen: boolean = false;
selectedEnquiry: any = {};

//view modal



inquiries = [
  {
    id: 1,
    title: 'Need help with assignment 1',
    description: 'Details about assignment 1',
    resolutionAction: 'No Action Required',
    linkedTopic: '',
    resolutionResponse: '',
    status: 'Open',
    dateCreated: '2025-05-23'
  },
  {
    id: 2,
    title: 'Clarification on Quiz 2',
    description: 'Some details about Quiz 2',
    resolutionAction: 'Link To Existing Topic',
    linkedTopic: 'Quiz Topics',
    resolutionResponse: 'Response here',
    status: 'Resolved',
    dateCreated: '2025-05-20'
  },
  {
    id: 3,
    title: 'Lecture Slides Missing',
    description: 'Lecture slides for week 3 not uploaded',
    resolutionAction: 'New Topic',
    linkedTopic: '',
    resolutionResponse: '',
    status: 'Pending',
    dateCreated: '2025-05-18'
  }
];
 //selectedEnquiry: any = {};


openViewModal(inquiry: any) {
  this.selectedEnquiry = { ...inquiry };
  this.isViewModalOpen = true;
}

closeViewModal() {
  this.isViewModalOpen = false;
}


}




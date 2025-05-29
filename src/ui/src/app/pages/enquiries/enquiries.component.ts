import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-enquiries',
  standalone: true,
  imports: [CommonModule, FormsModule],
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

  submitInquiry() {
    console.log('Enquiry submitted:', this.enquiryTitle, this.enquiryDescription);

    // Reset form and close modal
    this.enquiryTitle = '';
    this.enquiryDescription = '';
    this.showModal = false;
  }                   

}




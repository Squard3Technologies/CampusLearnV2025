import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
@Component({
  selector: 'app-enquiries-management',
  standalone: true,
  imports: [RouterModule, CommonModule, FormsModule],
  templateUrl: './enquiries-management.component.html',
  styleUrl: './enquiries-management.component.scss'
})
export class EnquiriesManagementComponent {
  isViewModalOpen: boolean = false;
selectedInquiry: any = {};

openViewModal(inquiry: any) {
  this.selectedInquiry = { ...inquiry };
  this.isViewModalOpen = true;
}

closeViewModal() {
  this.isViewModalOpen = false;
}


}

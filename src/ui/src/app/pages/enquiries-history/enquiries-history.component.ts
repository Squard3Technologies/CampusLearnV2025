import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
@Component({
  selector: 'app-enquiries-history',
  standalone: true,
  imports: [RouterModule, FormsModule, CommonModule ],
  templateUrl: './enquiries-history.component.html',
  styleUrl: './enquiries-history.component.scss',
})
export class EnquiriesHistoryComponent {
}

import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-topics',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './topic.component.html',
  styleUrl: './topic.component.scss'
})
export class TopicsComponent {
  showModal = false;
  topicTitle = '';
  topicDescription = '';
  topicFileType = '';

  openModal() {
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }

  submitTopic() {
    console.log('Topic submitted:', this.topicTitle, this.topicDescription, this.topicFileType);

    // Reset form and close modal
    this.topicTitle = '';
    this.topicDescription = '';
    this.topicFileType = '';
    this.showModal = false;
  }

  isViewModalOpen: boolean = false;
  selectedTopic: any = {};

  Topics = [
    {
      id: 1,
      name: 'Assignment Guidelines',
      description: 'Discussion on assignment submission criteria',
      fileType: 'PDF',
      status: 'Open',
      dateCreated: '2025-05-23'
    },
    {
      id: 2,
      name: 'Quiz Preparation',
      description: 'Recommended study materials for upcoming quiz',
      fileType: 'DOCX',
      status: 'Resolved',
      dateCreated: '2025-05-20'
    },
    {
      id: 3,
      name: 'Lecture Notes',
      description: 'Slides for week 3 lectures',
      fileType: 'PPTX',
      status: 'Pending',
      dateCreated: '2025-05-18'
    }
  ];

  openViewModal(topic: any) {
    this.selectedTopic = { ...topic };
    this.isViewModalOpen = true;
  }

  closeViewModal() {
    this.isViewModalOpen = false;
  }
}
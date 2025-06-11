import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { LearningMaterial, getMaterialsByTopicId } from '../../../mock-data';

@Component({
  selector: 'app-topic-learning-material',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './topic-learning-material.component.html',
  styleUrl: './topic-learning-material.component.scss'
})
export class TopicLearningMaterialComponent implements OnInit {
  topicId!: string;
  moduleId!: string;
  moduleName!: string;
  learningMaterials: LearningMaterial[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Get route parameters
    this.route.params.subscribe(params => {
      this.topicId = params['id'];
    });

    // Get query parameters
    this.route.queryParams.subscribe(params => {
      this.moduleId = params['moduleId'];
      this.moduleName = params['moduleName'];
    });

    // Load learning materials for this topic
    this.loadLearningMaterials();
  }

  loadLearningMaterials(): void {
    this.learningMaterials = getMaterialsByTopicId(this.topicId);
  }

  navigateToTopicOverview(): void {
    this.router.navigate(['/topic', this.topicId], {
      queryParams: { 
        moduleId: this.moduleId, 
        moduleName: this.moduleName 
      }
    });
  }

  downloadMaterial(material: LearningMaterial): void {
    if (material.downloadUrl) {
      // In a real app, this would trigger a download
      console.log('Downloading:', material.name);
      window.open(material.downloadUrl, '_blank');
    }
  }

  viewMaterial(material: LearningMaterial): void {
    if (material.viewUrl) {
      // In a real app, this would open a viewer or navigate to content
      console.log('Viewing:', material.name);
      window.open(material.viewUrl, '_blank');
    }
  }

  getFileTypeIcon(fileType: string): string {
    const icons: { [key: string]: string } = {
      'PDF': 'fas fa-file-pdf',
      'Video': 'fas fa-play-circle',
      'Article': 'fas fa-newspaper',
      'Slides': 'fas fa-file-powerpoint',
      'Audio': 'fas fa-volume-up',
      'Code': 'fas fa-code',
      'Quiz': 'fas fa-question-circle'
    };
    return icons[fileType] || 'fas fa-file';
  }

  getFileTypeClass(fileType: string): string {
    const classes: { [key: string]: string } = {
      'PDF': 'text-danger',
      'Video': 'text-primary',
      'Article': 'text-info',
      'Slides': 'text-warning',
      'Audio': 'text-success',
      'Code': 'text-dark',
      'Quiz': 'text-purple'
    };
    return classes[fileType] || 'text-secondary';
  }

  // Helper methods for summary statistics
  getVideoCount(): number {
    return this.learningMaterials.filter(material => material.fileType === 'Video').length;
  }

  getPdfCount(): number {
    return this.learningMaterials.filter(material => material.fileType === 'PDF').length;
  }

  getQuizCount(): number {
    return this.learningMaterials.filter(material => material.fileType === 'Quiz').length;
  }

  getArticleCount(): number {
    return this.learningMaterials.filter(material => material.fileType === 'Article').length;
  }

  navigateToDiscussions(): void {
    this.router.navigate(['/topic', this.topicId, 'discussions'], {
      queryParams: { 
        moduleId: this.moduleId, 
        moduleName: this.moduleName 
      }
    });
  }

  navigateToQuizzes(): void {
    this.router.navigate(['/topic', this.topicId, 'quizzes'], {
      queryParams: { 
        moduleId: this.moduleId, 
        moduleName: this.moduleName 
      }
    });
  }
}

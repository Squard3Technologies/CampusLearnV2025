import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-topic',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './topic.component.html',
  styleUrl: './topic.component.scss'
})
export class TopicComponent implements OnInit {
  moduleId: string | null = null;
  moduleName: string | null = null;
  topicId: string | null = null;
  topic: any | null = null;

  constructor(private route: ActivatedRoute, private router: Router, private apiService: ApiService) {}

  ngOnInit() {
    // Get the topic ID from route parameters
    this.route.paramMap.subscribe(params => {
      this.topicId = params.get('id');
    });    // Get module information from query parameters
    this.route.queryParamMap.subscribe(queryParams => {
      this.moduleId = queryParams.get('moduleId');
      this.moduleName = queryParams.get('moduleName');
    });

    if (this.topicId && this.moduleId) {
      this.apiService.getTopic(this.moduleId, this.topicId).subscribe((x:any) => {
        this.topic = x.body
      })
    }
  }

  navigateToDiscussions() {
    if (this.topicId) {
      // Navigate to discussions with module context as query parameters
      this.router.navigate(['/topic', this.topicId, 'discussions'], {
        queryParams: {
          moduleId: this.moduleId,
          moduleName: this.moduleName
        }
      });
    }
  }
  navigateToMaterials() {
    if (this.topicId) {
      // Navigate to learning materials with module context as query parameters
      this.router.navigate(['/topic', this.topicId, 'learning-material'], {
        queryParams: {
          moduleId: this.moduleId,
          moduleName: this.moduleName
        }
      });
    }
  }

  navigateToQuizzes() {
    if (this.topicId) {
      // Navigate to quizzes with module context as query parameters
      this.router.navigate(['/topic', this.topicId, 'quizzes'], {
        queryParams: {
          moduleId: this.moduleId,
          moduleName: this.moduleName
        }
      });
    }
  }
}

import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

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

  constructor(private route: ActivatedRoute, private router: Router) {}

  ngOnInit() {
    // Get the topic ID from route parameters
    this.route.paramMap.subscribe(params => {
      this.topicId = params.get('id');
    });    // Get module information from query parameters
    this.route.queryParamMap.subscribe(queryParams => {
      this.moduleId = queryParams.get('moduleId');
      this.moduleName = queryParams.get('moduleName');
    });
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
}

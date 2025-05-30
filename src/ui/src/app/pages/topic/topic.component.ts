import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
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

  constructor(private route: ActivatedRoute) {}

  ngOnInit() {
    // Get the topic ID from route parameters
    this.route.paramMap.subscribe(params => {
      this.topicId = params.get('id');
    });

    // Get module information from query parameters
    this.route.queryParamMap.subscribe(queryParams => {
      this.moduleId = queryParams.get('moduleId');
      this.moduleName = queryParams.get('moduleName');
    });
  }
}

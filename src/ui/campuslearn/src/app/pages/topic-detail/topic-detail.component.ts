import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TopicService } from '../../services/topic/topic.service';
import { DiscussionService } from '../../services/discussion/discussion.service';
import { LearningMaterialService } from '../../services/learning-material/learning-material.service';
import { QuizService } from '../../services/quiz/quiz.service';
import { AuthService } from '../../services/auth/auth.service';
import { SubscriptionService } from '../../services/subscription/subscription.service';
import { Topic } from '../../models/topic/topic.model';
import { Discussion } from '../../models/discussion/discussion.model';
import { LearningMaterial } from '../../models/learning-material/learning-material.model';
import { Quiz } from '../../models/quiz/quiz.model';
import { UserRole } from '../../models/user/user.module';

@Component({
  selector: 'app-topic-detail',
  templateUrl: './topic-detail.component.html',
  styleUrls: ['./topic-detail.component.scss'],
  standalone: true,
  imports: [CommonModule, RouterModule]
})
export class TopicDetailComponent implements OnInit {
  topicId: string = '';
  topic: Topic | undefined;
  discussions: Discussion[] = [];
  learningMaterials: LearningMaterial[] = [];
  quizzes: Quiz[] = [];
  isLoading = true;
  isSubscribed = false;
  isAdmin = false;
  userId: string = '';

  constructor(
    private route: ActivatedRoute,
    private topicService: TopicService,
    private discussionService: DiscussionService,
    private learningMaterialService: LearningMaterialService,
    private quizService: QuizService,
    private subscriptionService: SubscriptionService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.topicId = id;
        this.loadTopicData();
      }
    });

    // Check if user is admin and get user ID
    this.authService.currentUser$.subscribe(user => {
      if (user) {
        this.isAdmin = user.role === UserRole.Admin || user.role === UserRole.Tutor;
        this.userId = user.id;

        // Check if user is subscribed to this topic
        this.subscriptionService.isSubscribedToTopic(user.id, this.topicId)
          .subscribe(isSubscribed => {
            this.isSubscribed = isSubscribed;
          });
      }
    });
  }

  loadTopicData(): void {
    // Load topic details
    this.topicService.getTopicById(this.topicId).subscribe(topic => {
      this.topic = topic;

      if (topic) {
        // Load discussions for this topic
        this.discussionService.getDiscussionsByTopic(topic.id).subscribe(discussions => {
          this.discussions = discussions;
        });

        // Load learning materials for this topic
        this.learningMaterialService.getLearningMaterialsByTopic(topic.id).subscribe(materials => {
          this.learningMaterials = materials;
        });

        // Load quizzes for this topic
        this.quizService.getQuizzesByTopic(topic.id).subscribe(quizzes => {
          this.quizzes = quizzes;
          this.isLoading = false;
        });
      }
    });
  }

  toggleSubscription(): void {
    if (!this.userId || !this.topicId) return;

    if (this.isSubscribed) {
      // Unsubscribe from topic
      this.subscriptionService.unsubscribeFromTopic(this.userId, this.topicId)
        .subscribe(() => {
          this.isSubscribed = false;
        });
    } else {
      // Subscribe to topic
      this.subscriptionService.subscribeToTopic(this.userId, this.topicId)
        .subscribe(() => {
          this.isSubscribed = true;
        });
    }
  }

  subscribeTopic(): void {
    if (!this.userId || !this.topicId) return;

    this.subscriptionService.subscribeToTopic(this.userId, this.topicId)
      .subscribe(() => {
        this.isSubscribed = true;
      });
  }

  unsubscribeTopic(): void {
    if (!this.userId || !this.topicId) return;

    this.subscriptionService.unsubscribeFromTopic(this.userId, this.topicId)
      .subscribe(() => {
        this.isSubscribed = false;
      });
  }
}

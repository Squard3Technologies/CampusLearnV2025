import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { Discussion, DiscussionComment, CreateDiscussionRequest, CreateCommentRequest } from '../../models/discussion.models';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-topic-discussions',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './topic-discussions.component.html',
  styleUrl: './topic-discussions.component.scss'
})
export class TopicDiscussionsComponent implements OnInit {
  discussions: Discussion[] = [];
  newComment: { [postId: string]: string } = {};
  topicId: string | null = null;
  moduleId: string | null = null;
  moduleName: string | null = null;
  loading: boolean = false;
  commentLoading: { [postId: string]: boolean } = {};
  errorMessage: string | null = null;
  
  // New post form
  newPost = {
    title: '',
    content: ''
  };
  showNewPostForm: boolean = false;
  postSubmitting: boolean = false;

  constructor(
    private route: ActivatedRoute, 
    private router: Router,
    private apiService: ApiService
  ) {}

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

    this.loadDiscussions();
  }
  loadDiscussions() {
    if (!this.topicId) return;
    
    this.loading = true;
    this.errorMessage = null;
    
    this.apiService.getDiscussionsByTopic(this.topicId)
      .pipe(finalize(() => this.loading = false))
      .subscribe({
        next: (discussions) => {
          this.discussions = discussions;
          // Load comments for each discussion
          discussions.forEach(discussion => {
            this.loadDiscussionComments(discussion.id);
          });
        },
        error: (error) => {
          console.error('Error loading discussions:', error);
          this.errorMessage = 'Failed to load discussions. Please try again later.';
        }
      });
  }

  createNewDiscussion() {
    if (!this.topicId) return;
    
    const discussionData: CreateDiscussionRequest = {
      title: this.newPost.title,
      content: this.newPost.content
    };
    
    this.postSubmitting = true;
    this.apiService.createDiscussion(this.topicId, discussionData)
      .pipe(finalize(() => this.postSubmitting = false))
      .subscribe({
        next: (result) => {
          // Reset form
          this.newPost = { title: '', content: '' };
          this.showNewPostForm = false;
          
          // Reload discussions to include the new one
          this.loadDiscussions();
        },
        error: (error) => {
          console.error('Error creating discussion:', error);
          this.errorMessage = 'Failed to create discussion. Please try again later.';
        }
      });
  }  addComment(discussionId: string) {
    if (!this.newComment[discussionId]?.trim()) return;
    
    const commentData: CreateCommentRequest = {
      content: this.newComment[discussionId].trim()
    };
    
    console.log('Adding comment to discussion:', discussionId);
    console.log('Comment data:', commentData);
    
    this.commentLoading[discussionId] = true;
    this.errorMessage = null; // Clear any previous errors
    
    this.apiService.addCommentToDiscussion(discussionId, commentData)
      .pipe(finalize(() => this.commentLoading[discussionId] = false))
      .subscribe({
        next: (result) => {
          console.log('Comment added successfully, result:', result);
          
          // Clear the comment input immediately for better UX
          this.newComment[discussionId] = '';
          
          // Reload comments for this discussion to show the new comment
          this.loadDiscussionComments(discussionId);
        },
        error: (error) => {
          console.error('Error adding comment:', error);
          this.errorMessage = `Failed to add comment: ${error?.error?.message || error?.message || 'Unknown error'}`;
          
          // Don't clear the comment input on error so user can retry
        }
      });
  }  loadDiscussionComments(discussionId: string) {
    console.log('Loading comments for discussion:', discussionId);
    
    this.apiService.getDiscussionComments(discussionId)
      .subscribe({
        next: (comments) => {
          console.log('Comments loaded successfully for discussion', discussionId, ':', comments);
          
          // Find the discussion and update its comments
          const discussion = this.discussions.find(d => d.id === discussionId);
          if (discussion) {
            // Ensure comments is always an array and properly typed
            discussion.comments = Array.isArray(comments) ? comments : [];
            console.log('Updated discussion comments:', discussion.comments);
          } else {
            console.error('Discussion not found in local array:', discussionId);
          }
        },
        error: (error) => {
          console.error('Error loading comments for discussion', discussionId, ':', error);
          
          // Initialize empty comments array on error to prevent template issues
          const discussion = this.discussions.find(d => d.id === discussionId);
          if (discussion && !discussion.comments) {
            discussion.comments = [];
          }
          
          // Only show error if it's not a "no content" error
          if (error.status !== 204) {
            this.errorMessage = 'Failed to load some comments. Please refresh the page.';
          }
        }
      });
  }
  toggleNewPostForm() {
    this.showNewPostForm = !this.showNewPostForm;
    if (!this.showNewPostForm) {
      // Reset form when hiding
      this.newPost = { title: '', content: '' };
    }  }

  // Track function for better performance in comment ngFor
  trackComment(index: number, comment: DiscussionComment): string {
    return comment.id;
  }

  // Navigation methods for sidebar
  navigateToTopicOverview() {
    if (this.topicId) {
      this.router.navigate(['/topic', this.topicId], { 
        queryParams: { 
          moduleId: this.moduleId, 
          moduleName: this.moduleName 
        } 
      });
    }
  }

  navigateToMaterials() {
    if (this.topicId) {
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
      this.router.navigate(['/topic', this.topicId, 'quizzes'], { 
        queryParams: { 
          moduleId: this.moduleId, 
          moduleName: this.moduleName 
        } 
      });
    }
  }
}
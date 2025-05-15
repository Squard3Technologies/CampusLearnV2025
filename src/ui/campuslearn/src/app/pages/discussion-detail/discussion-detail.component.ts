import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { DiscussionService } from '../../services/discussion/discussion.service';
import { AuthService } from '../../services/auth/auth.service';
import { Discussion } from '../../models/discussion/discussion.model';
import { Comment } from '../../models/comment/comment.model';

@Component({
  selector: 'app-discussion-detail',
  templateUrl: './discussion-detail.component.html',
  styleUrls: ['./discussion-detail.component.scss'],
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule]
})
export class DiscussionDetailComponent implements OnInit {
  discussionId: string = '';
  discussion: Discussion | undefined;
  comments: Comment[] = [];
  isLoading = true;
  commentForm: FormGroup;
  currentUserId: string = '';

  constructor(
    private route: ActivatedRoute,
    private discussionService: DiscussionService,
    private authService: AuthService,
    private fb: FormBuilder
  ) {
    this.commentForm = this.fb.group({
      content: ['', [Validators.required, Validators.minLength(2)]]
    });
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.discussionId = id;
        this.loadDiscussionData();
      }
    });

    // Get current user ID for posting comments
    this.authService.currentUser$.subscribe(user => {
      if (user) {
        this.currentUserId = user.id;
      }
    });
  }

  loadDiscussionData(): void {
    // Load discussion details
    this.discussionService.getDiscussionById(this.discussionId).subscribe(discussion => {
      this.discussion = discussion;

      // Load comments for this discussion
      if (discussion) {
        this.discussionService.getCommentsByDiscussion(discussion.id).subscribe(comments => {
          this.comments = comments;
          this.isLoading = false;
        });
      }
    });
  }

  submitComment(): void {
    if (this.commentForm.invalid || !this.discussionId || !this.currentUserId) {
      return;
    }

    const commentData: Partial<Comment> = {
      content: this.commentForm.value.content,
      createdByUserId: this.currentUserId,
      discussionId: this.discussionId
    };

    this.discussionService.addComment(commentData).subscribe(newComment => {
      this.comments.push(newComment);
      this.commentForm.reset();
    });
  }
}

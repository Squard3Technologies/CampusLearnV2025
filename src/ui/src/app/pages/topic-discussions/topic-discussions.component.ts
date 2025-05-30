import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';

interface Author {
  id: string;
  name: string;
  avatar?: string;
}

interface Comment {
  id: string;
  content: string;
  author: Author;
  timestamp: Date;
}

interface DiscussionPost {
  id: string;
  title: string;
  content: string;
  author: Author;
  timestamp: Date;
  likes?: number;
  comments?: Comment[];
}

@Component({
  selector: 'app-topic-discussions',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './topic-discussions.component.html',
  styleUrl: './topic-discussions.component.scss'
})
export class TopicDiscussionsComponent implements OnInit {
  discussionPosts: DiscussionPost[] = [];
  newComment: { [postId: string]: string } = {};
  topicId: string | null = null;
  moduleId: string | null = null;
  moduleName: string | null = null;

  constructor(private route: ActivatedRoute, private router: Router) {}

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

    this.loadDiscussionPosts();
  }

  loadDiscussionPosts() {
    // Mock data for demonstration
    this.discussionPosts = [
      {
        id: '1',
        title: 'Question about the assignment',
        content: 'I\'m having trouble understanding the requirements for the final project. Can someone help clarify what we need to submit?',
        author: { id: '1', name: 'John Smith' },
        timestamp: new Date(Date.now() - 3600000), // 1 hour ago
        likes: 3,
        comments: [
          {
            id: '1',
            content: 'I had the same question! The professor mentioned we need to include a written report along with the code.',
            author: { id: '2', name: 'Mary Johnson' },
            timestamp: new Date(Date.now() - 1800000) // 30 minutes ago
          },
          {
            id: '2',
            content: 'Don\'t forget to include the UML diagrams as well. They\'re worth 20% of the grade.',
            author: { id: '3', name: 'David Brown' },
            timestamp: new Date(Date.now() - 900000) // 15 minutes ago
          }
        ]
      },
      {
        id: '2',
        title: 'Study group for midterm?',
        content: 'Anyone interested in forming a study group for the upcoming midterm exam? We could meet this weekend.',
        author: { id: '4', name: 'Sarah Wilson' },
        timestamp: new Date(Date.now() - 7200000), // 2 hours ago
        likes: 5,
        comments: [
          {
            id: '3',
            content: 'I\'m interested! Saturday afternoon works best for me.',
            author: { id: '5', name: 'Mike Davis' },
            timestamp: new Date(Date.now() - 3600000) // 1 hour ago
          }
        ]
      },
      {
        id: '3',
        title: 'Recommended resources',
        content: 'Here are some additional resources I found helpful for understanding the concepts covered in this topic.',
        author: { id: '6', name: 'Lisa Garcia' },
        timestamp: new Date(Date.now() - 86400000), // 1 day ago
        likes: 8,
        comments: []
      }
    ];
  }

  addComment(postId: string) {
    const commentText = this.newComment[postId];
    if (commentText && commentText.trim()) {
      const post = this.discussionPosts.find(p => p.id === postId);
      if (post) {
        if (!post.comments) {
          post.comments = [];
        }
        
        const newComment: Comment = {
          id: Date.now().toString(),
          content: commentText.trim(),
          author: { id: 'current-user', name: 'Current User' }, // This would come from user service
          timestamp: new Date()
        };
          post.comments.push(newComment);
        this.newComment[postId] = ''; // Clear the input
      }
    }
  }
  navigateToTopicOverview() {
    if (this.topicId) {
      // Navigate back to topic overview with module context as query parameters
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

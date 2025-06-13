import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { LearningMaterial, getMaterialsByTopicId } from '../../../mock-data';
import { ApiService } from '../../services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-topic-learning-material',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './topic-learning-material.component.html',
  styleUrl: './topic-learning-material.component.scss'
})
export class TopicLearningMaterialComponent implements OnInit {
  showModal = false;
  topicId!: string;
  moduleId!: string;
  moduleName!: string;
  learningMaterials: LearningMaterial[] = [];

  fileType: string = '';
  selectedFile: File | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private apiService: ApiService
  ) { }

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


  //#region 

  showDialog() {
    this.showModal = true;
  }

  closeDialog(clearFields: boolean) {
    if (clearFields) {

    }
    this.showModal = false;
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
    }
  }


  onUpload(): void {

    if (!this.selectedFile || !this.fileType) {
      Swal.fire({
        icon: 'error',
        iconColor: '#AD0151',
        title: 'Validation Error',
        text: 'Please fill in all fields',
        confirmButtonColor: '#dc3545',
        confirmButtonText: '<i class="fa fa-thumbs-down me-2"></i> Dismiss',
        allowOutsideClick: false,
        buttonsStyling: false,
        customClass: {
          confirmButton: 'btn btn-md btn-outline-danger me-2',
        }
      });
      return;
    }


    this.closeDialog(false);
    Swal.fire({
      title: 'uploading learning material...',
      allowOutsideClick: false,
      didOpen: () => {
        Swal.showLoading();
      }
    });

    const formData = new FormData();
    formData.append('TopicId', this.topicId);
    formData.append('FileType', this.fileType);
    formData.append('FileData', this.selectedFile, this.selectedFile.name);

    console.log('edit user json: ', JSON.stringify(formData));
    
    // Call API registration service
    this.apiService.uploadLearningMaterial(formData).subscribe({
      next: (response) => {
        console.log('material uploaded successful:', response);
        Swal.close();
        if (response != null) {
          if (!response.status) {
            this.showDialog();
            Swal.fire({
              icon: 'error',
              iconColor: '#dc3545',
              title: 'ERROR',
              text: response.statusMessage,
              confirmButtonColor: '#dc3545',
              confirmButtonText: '<i class="fa fa-thumbs-down me-2"></i> Dismiss',
              allowOutsideClick: false,
              buttonsStyling: false,
              customClass: {
                confirmButton: 'btn btn-outline-danger me-2',
              }
            });
            return;
          }
          Swal.fire({
            icon: 'success',
            iconColor: '#198754',
            title: 'SUCCESS',
            html: response.statusMessage,
            confirmButtonColor: '#fafafa',
            confirmButtonText: '<i class="fa fa-thumbs-up me-2"></i> Dismiss',
            allowOutsideClick: false,
            buttonsStyling: false,
            customClass: {
              confirmButton: 'btn btn-md btn-outline-success me-2',
            }
          }).then((result) => {
            this.fileType = ''
            this.selectedFile = null
            //this.loadUsers();
          });

        }
        else {
          this.showDialog();
          Swal.fire({
            icon: 'error',
            iconColor: '#dc3545',
            title: 'ERROR',
            html: '<p class="font-13">uploading learning material failed. <br/>Internal system error encountered</p>',
            confirmButtonColor: '#dc3545',
            confirmButtonText: '<i class="fa fa-thumbs-down me-2"></i> Dismiss',
            allowOutsideClick: false,
            buttonsStyling: false,
            customClass: {
              confirmButton: 'btn btn-md btn-outline-danger me-2',
            }
          }).then((result) => {

          });
        }

      },
      error: (error) => {
        this.showDialog();
        Swal.close();
        Swal.fire({
          icon: 'error',
          iconColor: '#dc3545',
          title: 'ERROR',
          text: error.error?.message || 'uploading learning material failed. Please try again.',
          confirmButtonColor: '#dc3545',
          confirmButtonText: '<i class="fa fa-thumbs-down me-2"></i> Dismiss',
          allowOutsideClick: false,
          buttonsStyling: false,
          customClass: {
            confirmButton: 'btn btn-md btn-outline-danger me-2',
          }
        });
      }

    });


  }

  //#endregion


}

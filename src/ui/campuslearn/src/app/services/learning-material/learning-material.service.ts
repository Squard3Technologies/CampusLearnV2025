import { Injectable } from '@angular/core';
import { Observable, of, delay } from 'rxjs';
import { LearningMaterial } from '../../models/learning-material/learning-material.model';

@Injectable({
  providedIn: 'root'
})
export class LearningMaterialService {
  private mockLearningMaterials: LearningMaterial[] = [
    {
      id: '1',
      fileType: 'pdf',
      filePath: '/assets/materials/programming_intro.pdf',
      uploadedByUserId: '1',
      uploadedDate: new Date('2025-02-15'),
      topicId: '1'
    },
    {
      id: '2',
      fileType: 'video',
      filePath: '/assets/materials/variables_tutorial.mp4',
      uploadedByUserId: '1',
      uploadedDate: new Date('2025-02-20'),
      topicId: '2'
    },
    {
      id: '3',
      fileType: 'pdf',
      filePath: '/assets/materials/database_normalization.pdf',
      uploadedByUserId: '2',
      uploadedDate: new Date('2025-03-05'),
      topicId: '3'
    }
  ];

  constructor() { }

  getAllLearningMaterials(): Observable<LearningMaterial[]> {
    return of(this.mockLearningMaterials).pipe(delay(500));
  }

  getLearningMaterialById(id: string): Observable<LearningMaterial | undefined> {
    const material = this.mockLearningMaterials.find(m => m.id === id);
    return of(material).pipe(delay(300));
  }

  getLearningMaterialsByTopic(topicId: string): Observable<LearningMaterial[]> {
    const materials = this.mockLearningMaterials.filter(material => material.topicId === topicId);
    return of(materials).pipe(delay(500));
  }

  uploadLearningMaterial(material: Partial<LearningMaterial>): Observable<LearningMaterial> {
    const newMaterial: LearningMaterial = {
      id: Date.now().toString(),
      fileType: material.fileType || '',
      filePath: material.filePath || '',
      uploadedByUserId: material.uploadedByUserId || '',
      uploadedDate: new Date(),
      topicId: material.topicId || ''
    };

    this.mockLearningMaterials.push(newMaterial);
    return of(newMaterial).pipe(delay(1000)); // Longer delay to simulate file upload
  }

  deleteLearningMaterial(id: string): Observable<boolean> {
    const initialLength = this.mockLearningMaterials.length;
    this.mockLearningMaterials = this.mockLearningMaterials.filter(m => m.id !== id);

    return of(initialLength > this.mockLearningMaterials.length).pipe(delay(500));
  }
}

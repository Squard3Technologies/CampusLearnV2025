import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TopicLearningMaterialComponent } from './topic-learning-material.component';

describe('TopicLearningMaterialComponent', () => {
  let component: TopicLearningMaterialComponent;
  let fixture: ComponentFixture<TopicLearningMaterialComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TopicLearningMaterialComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TopicLearningMaterialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

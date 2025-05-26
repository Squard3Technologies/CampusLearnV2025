import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EnquiriesHistoryComponent } from './enquiries-history.component';

describe('EnquiriesHistoryComponent', () => {
  let component: EnquiriesHistoryComponent;
  let fixture: ComponentFixture<EnquiriesHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EnquiriesHistoryComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EnquiriesHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

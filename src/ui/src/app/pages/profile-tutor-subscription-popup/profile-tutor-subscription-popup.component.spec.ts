import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProfileTutorSubscriptionPopupComponent } from './profile-tutor-subscription-popup.component';
import { By } from '@angular/platform-browser';

describe('ProfileTutorSubscriptionPopupComponent', () => {
  let component: ProfileTutorSubscriptionPopupComponent;
  let fixture: ComponentFixture<ProfileTutorSubscriptionPopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProfileTutorSubscriptionPopupComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(ProfileTutorSubscriptionPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the popup component', () => {
    expect(component).toBeTruthy();
  });

  it('should display the popup when showPopup is true', () => {
    component.showPopup = true;
    fixture.detectChanges();
    const modal = fixture.debugElement.query(By.css('.modal'));
    expect(modal).toBeTruthy();
  });

  it('should not display the popup when showPopup is false', () => {
    component.showPopup = false;
    fixture.detectChanges();
    const modal = fixture.debugElement.query(By.css('.modal'));
    expect(modal).toBeFalsy();
  });

  it('should call closePopup when close button is clicked', () => {
    component.showPopup = true;
    spyOn(component, 'closePopup');
    fixture.detectChanges();
    const closeBtn = fixture.debugElement.query(By.css('.close-btn'));
    closeBtn.nativeElement.click();
    expect(component.closePopup).toHaveBeenCalled();
  });

  it('should call confirmPopup when confirm button is clicked', () => {
    component.showPopup = true;
    spyOn(component, 'confirmPopup');
    fixture.detectChanges();
    const confirmBtn = fixture.debugElement.query(By.css('.confirm-btn'));
    confirmBtn.nativeElement.click();
    expect(component.confirmPopup).toHaveBeenCalled();
  });

// ...existing code...

it('should call closePopup when cancel button is clicked', () => {
  component.showPopup = true;
  spyOn(component, 'closePopup');
  fixture.detectChanges();
  const cancelBtn = fixture.debugElement.query(By.css('.cancel-btn'));
  expect(cancelBtn).toBeTruthy(); // Ensure the button exists
  cancelBtn.nativeElement.click();
  expect(component.closePopup).toHaveBeenCalled();
});

describe('ProfileTutorSubscriptionPopupComponent', () => {
  let component: ProfileTutorSubscriptionPopupComponent;
  let fixture: ComponentFixture<ProfileTutorSubscriptionPopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProfileTutorSubscriptionPopupComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(ProfileTutorSubscriptionPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the popup component', () => {
    expect(component).toBeTruthy();
  });

  it('should display the popup when showPopup is true', () => {
    component.showPopup = true;
    fixture.detectChanges();
    const modal = fixture.debugElement.query(By.css('.modal'));
    expect(modal).toBeTruthy();
  });

  it('should not display the popup when showPopup is false', () => {
    component.showPopup = false;
    fixture.detectChanges();
    const modal = fixture.debugElement.query(By.css('.modal'));
    expect(modal).toBeFalsy();
  });

  it('should call closePopup when close button is clicked', () => {
    component.showPopup = true;
    spyOn(component, 'closePopup');
    fixture.detectChanges();
    const closeBtn = fixture.debugElement.query(By.css('.close-btn'));
    closeBtn.nativeElement.click();
    expect(component.closePopup).toHaveBeenCalled();
  });

  it('should call confirmPopup when confirm button is clicked', () => {
    component.showPopup = true;
    spyOn(component, 'confirmPopup');
    fixture.detectChanges();
    const confirmBtn = fixture.debugElement.query(By.css('.confirm-btn'));
    confirmBtn.nativeElement.click();
    expect(component.confirmPopup).toHaveBeenCalled();
  });

  it('should call closePopup when cancel button is clicked', () => {
    component.showPopup = true;
    spyOn(component, 'closePopup');
    fixture.detectChanges();
    const cancelBtn = fixture.debugElement.query(By.css('.cancel-btn'));
    cancelBtn.nativeElement.click();
    expect(component.closePopup).toHaveBeenCalled();
  });
    it('should call the popup action when confirmPopup is called', () => {
        const mockAction = jasmine.createSpy('mockAction');
        component.openPopup('Test Title', 'Test Message', mockAction);
        component.confirmPopup();
        expect(mockAction).toHaveBeenCalled();
    });
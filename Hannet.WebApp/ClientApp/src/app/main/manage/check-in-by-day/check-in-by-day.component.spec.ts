import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckInByDayComponent } from './check-in-by-day.component';

describe('CheckInByDayComponent', () => {
  let component: CheckInByDayComponent;
  let fixture: ComponentFixture<CheckInByDayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CheckInByDayComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CheckInByDayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

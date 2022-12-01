import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AScheduleDeviceDetailComponent } from './a-schedule-device-detail.component';

describe('AScheduleDeviceDetailComponent', () => {
  let component: AScheduleDeviceDetailComponent;
  let fixture: ComponentFixture<AScheduleDeviceDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AScheduleDeviceDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AScheduleDeviceDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

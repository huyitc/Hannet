import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TDeviceComponent } from './t-device.component';

describe('TDeviceComponent', () => {
  let component: TDeviceComponent;
  let fixture: ComponentFixture<TDeviceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TDeviceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TDeviceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

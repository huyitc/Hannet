import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TDevicetypeComponent } from './t-devicetype.component';

describe('TDevicetypeComponent', () => {
  let component: TDevicetypeComponent;
  let fixture: ComponentFixture<TDevicetypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TDevicetypeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TDevicetypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

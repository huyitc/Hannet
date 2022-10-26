import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TEmployeeComponent } from './t-employee.component';

describe('TEmployeeComponent', () => {
  let component: TEmployeeComponent;
  let fixture: ComponentFixture<TEmployeeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TEmployeeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TEmployeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

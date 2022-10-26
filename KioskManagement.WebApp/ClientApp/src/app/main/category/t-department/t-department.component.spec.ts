import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TDepartmentComponent } from './t-department.component';

describe('TDepartmentComponent', () => {
  let component: TDepartmentComponent;
  let fixture: ComponentFixture<TDepartmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TDepartmentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TDepartmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

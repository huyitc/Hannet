import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TGroupaccessComponent } from './t-groupaccess.component';

describe('TGroupaccessComponent', () => {
  let component: TGroupaccessComponent;
  let fixture: ComponentFixture<TGroupaccessComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TGroupaccessComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TGroupaccessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

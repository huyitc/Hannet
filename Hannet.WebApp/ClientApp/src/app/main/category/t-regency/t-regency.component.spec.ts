import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TRegencyComponent } from './t-regency.component';

describe('TRegencyComponent', () => {
  let component: TRegencyComponent;
  let fixture: ComponentFixture<TRegencyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TRegencyComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TRegencyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

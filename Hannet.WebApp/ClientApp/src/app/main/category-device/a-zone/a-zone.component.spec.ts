import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AZoneComponent } from './a-zone.component';

describe('AZoneComponent', () => {
  let component: AZoneComponent;
  let fixture: ComponentFixture<AZoneComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AZoneComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AZoneComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

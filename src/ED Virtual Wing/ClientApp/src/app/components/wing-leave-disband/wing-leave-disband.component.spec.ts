import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WingLeaveDisbandComponent } from './wing-leave-disband.component';

describe('WingLeaveDisbandComponent', () => {
  let component: WingLeaveDisbandComponent;
  let fixture: ComponentFixture<WingLeaveDisbandComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WingLeaveDisbandComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WingLeaveDisbandComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

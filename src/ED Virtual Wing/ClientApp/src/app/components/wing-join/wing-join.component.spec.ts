import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WingJoinComponent } from './wing-join.component';

describe('WingJoinComponent', () => {
  let component: WingJoinComponent;
  let fixture: ComponentFixture<WingJoinComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WingJoinComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WingJoinComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WingComponent } from './wing.component';

describe('WingComponent', () => {
  let component: WingComponent;
  let fixture: ComponentFixture<WingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

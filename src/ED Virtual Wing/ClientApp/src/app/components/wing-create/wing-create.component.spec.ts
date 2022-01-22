import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WingCreateComponent } from './wing-create.component';

describe('WingCreateComponent', () => {
  let component: WingCreateComponent;
  let fixture: ComponentFixture<WingCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WingCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WingCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

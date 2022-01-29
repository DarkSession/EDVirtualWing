import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FdevAuthComponent } from './fdev-auth.component';

describe('FdevAuthComponent', () => {
  let component: FdevAuthComponent;
  let fixture: ComponentFixture<FdevAuthComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FdevAuthComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FdevAuthComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

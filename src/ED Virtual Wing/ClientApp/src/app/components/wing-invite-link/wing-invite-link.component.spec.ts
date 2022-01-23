import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WingInviteLinkComponent } from './wing-invite-link.component';

describe('WingInviteLinkComponent', () => {
  let component: WingInviteLinkComponent;
  let fixture: ComponentFixture<WingInviteLinkComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WingInviteLinkComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WingInviteLinkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

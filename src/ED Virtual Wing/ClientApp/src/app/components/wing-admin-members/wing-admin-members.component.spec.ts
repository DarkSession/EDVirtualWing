import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WingAdminMembersComponent } from './wing-admin-members.component';

describe('WingAdminMembersComponent', () => {
  let component: WingAdminMembersComponent;
  let fixture: ComponentFixture<WingAdminMembersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WingAdminMembersComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WingAdminMembersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

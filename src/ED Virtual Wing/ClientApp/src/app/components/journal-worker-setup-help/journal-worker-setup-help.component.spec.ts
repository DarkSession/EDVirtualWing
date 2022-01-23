import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JournalWorkerSetupHelpComponent } from './journal-worker-setup-help.component';

describe('JournalWorkerSetupHelpComponent', () => {
  let component: JournalWorkerSetupHelpComponent;
  let fixture: ComponentFixture<JournalWorkerSetupHelpComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ JournalWorkerSetupHelpComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(JournalWorkerSetupHelpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

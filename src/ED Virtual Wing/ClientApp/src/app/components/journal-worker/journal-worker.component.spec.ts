import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JournalWorkerComponent } from './journal-worker.component';

describe('JournalWorkerComponent', () => {
  let component: JournalWorkerComponent;
  let fixture: ComponentFixture<JournalWorkerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ JournalWorkerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(JournalWorkerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';

import { JournalWorkerService } from './journal-worker.service';

describe('JournalWorkerService', () => {
  let service: JournalWorkerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(JournalWorkerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

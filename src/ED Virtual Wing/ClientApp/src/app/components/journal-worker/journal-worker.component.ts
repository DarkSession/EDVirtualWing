import { Component } from '@angular/core';
import { JournalWorkerService } from 'src/app/journal-worker.service';

@Component({
  selector: 'app-journal-worker',
  templateUrl: './journal-worker.component.html',
  styleUrls: ['./journal-worker.component.css']
})
export class JournalWorkerComponent {  
  public constructor(
    public readonly journalWorkerService: JournalWorkerService
  ) { }

}

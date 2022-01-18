import { Component, OnInit } from '@angular/core';
import { JournalWorkerService } from 'src/app/journal-worker.service';

@Component({
  selector: 'app-journal-worker',
  templateUrl: './journal-worker.component.html',
  styleUrls: ['./journal-worker.component.css']
})
export class JournalWorkerComponent implements OnInit {  
  public constructor(
    public readonly journalWorkerService: JournalWorkerService
  ) { }

  public ngOnInit(): void {
  }

}

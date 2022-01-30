import { Component, Inject, OnInit } from '@angular/core';
import { JournalWorkerHelpData } from 'src/app/interfaces/journal-worker-help-data';
import { OVERLAY_DATA } from 'src/app/injector/overlay-data';

@Component({
  selector: 'app-journal-worker-setup-help',
  templateUrl: './journal-worker-setup-help.component.html',
  styleUrls: ['./journal-worker-setup-help.component.css']
})
export class JournalWorkerSetupHelpComponent implements OnInit {
  public showIndex: number = 0;
  public onlyShowFinalStep: boolean = false;
  private interval: any = null;
  public maxSteps: number = 5;
  public steps: number[] = [];

  public constructor(
    @Inject(OVERLAY_DATA) public readonly data: JournalWorkerHelpData
  ) { }

  public ngOnInit(): void {
    this.onlyShowFinalStep = this.data.onlyShowFinalStep;
    if (this.onlyShowFinalStep) {
      this.showIndex = 5;
    }
    else {
      for (let i = 0; i <= this.maxSteps; i++) {
        this.steps.push(i);
      }
      this.interval = setInterval(() => {
        this.showIndex += 1;
        if (this.showIndex > this.maxSteps) {
          this.showIndex = 0;
        }
      }, 5000);
    }
  }

  public next(): void {
    this.showIndex++;
    if (this.interval !== null) {
      clearInterval(this.interval);
      this.interval = null;
    }
  }

  public previos(): void {
    this.showIndex--;
    if (this.interval !== null) {
      clearInterval(this.interval);
      this.interval = null;
    }
  }

  public selectIndex(index: number): void {
    this.showIndex = index;
    if (this.interval !== null) {
      clearInterval(this.interval);
      this.interval = null;
    }
  }
}

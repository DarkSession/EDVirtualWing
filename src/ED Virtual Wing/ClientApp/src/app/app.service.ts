import { EventEmitter, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AppService {
  private loading: boolean = false;
  public loadingChanged: EventEmitter<boolean> = new EventEmitter<boolean>();

  public constructor() { }

  public setLoading(loading: boolean): void {
    if (this.loading != loading) {
      this.loading = loading;
      this.loadingChanged.emit(loading);
    }
  }

  public get isLoading(): boolean {
    return this.loading;
  }
}

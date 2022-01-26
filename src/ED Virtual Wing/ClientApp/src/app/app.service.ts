import { EventEmitter, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AppService {
  private loading: boolean = false;
  public loadingChanged: EventEmitter<boolean> = new EventEmitter<boolean>();
  public darkMode: boolean = false;

  public constructor() {
    const darkMode = localStorage.getItem('darkMode');
    console.log("darkMode", darkMode);
    this.darkMode = (darkMode === "1");
  }

  public setLoading(loading: boolean): void {
    if (this.loading !== loading) {
      this.loading = loading;
      this.loadingChanged.emit(loading);
    }
  }

  public get isLoading(): boolean {
    return this.loading;
  }
}

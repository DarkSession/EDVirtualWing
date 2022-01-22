import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CmdrTestComponent } from './cmdr-test.component';

describe('CmdrTestComponent', () => {
  let component: CmdrTestComponent;
  let fixture: ComponentFixture<CmdrTestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CmdrTestComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CmdrTestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddGraduatesComponent } from './add-graduates.component';

describe('AddGraduatesComponent', () => {
  let component: AddGraduatesComponent;
  let fixture: ComponentFixture<AddGraduatesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddGraduatesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddGraduatesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

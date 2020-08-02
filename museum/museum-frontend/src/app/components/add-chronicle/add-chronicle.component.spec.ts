import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddChronicleComponent } from './add-chronicle.component';

describe('AddChronicleComponent', () => {
  let component: AddChronicleComponent;
  let fixture: ComponentFixture<AddChronicleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddChronicleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddChronicleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

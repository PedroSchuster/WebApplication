import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PostTLComponent } from './post-tl.component';

describe('PostTLComponent', () => {
  let component: PostTLComponent;
  let fixture: ComponentFixture<PostTLComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PostTLComponent]
    });
    fixture = TestBed.createComponent(PostTLComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

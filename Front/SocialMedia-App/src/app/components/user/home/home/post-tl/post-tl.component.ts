import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Post } from '@app/models/identity/Post';
import { User } from '@app/models/identity/User';
import { AccountService } from '@app/services/account.service';
import { PostService } from '@app/services/post.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-post-tl',
  templateUrl: './post-tl.component.html',
  styleUrls: ['./post-tl.component.scss']
})
export class PostTLComponent implements OnInit {

  @Input() userName: string;
  @Input() body: string;
  @Input() date: string;
  @Input() id: number;

  user: User;
  constructor(
    private router: Router
  ) { }

  public ngOnInit(): void {
  }

  public postDetails(id: number): void{
    this.router.navigate([`home/post/${id}`])
  }


}

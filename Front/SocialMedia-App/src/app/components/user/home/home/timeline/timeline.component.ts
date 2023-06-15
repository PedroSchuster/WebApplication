import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Post } from '@app/models/identity/Post';
import { User } from '@app/models/identity/User';
import { AccountService } from '@app/services/account.service';
import { PostService } from '@app/services/post.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-timeline',
  templateUrl: './timeline.component.html',
  styleUrls: ['./timeline.component.scss']
})
export class TimelineComponent implements OnInit {

  @Input() userName: string;
  @Input() body: string;
  @Input() date: string;

  user: User;
  constructor(
    private accountService: AccountService
  ) { }

  public ngOnInit(): void {
  }



}

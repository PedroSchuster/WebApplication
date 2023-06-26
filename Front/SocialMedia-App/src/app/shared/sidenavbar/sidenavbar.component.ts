import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-sidenavbar',
  templateUrl: './sidenavbar.component.html',
  styleUrls: ['./sidenavbar.component.scss']
})
export class SidenavbarComponent implements OnInit {

  public userName: string;
  constructor( ){}
  ngOnInit(): void {
    this.userName = JSON.parse(localStorage.getItem('user'))['userName'];
  }
}

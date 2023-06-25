import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { Subject, debounceTime } from 'rxjs';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit{

  userNameChanged: Subject<string> = new Subject<string>();
  public searchTerm: any;
  public usersFound: UserUpdate[] = [];

  constructor(
    private accountService: AccountService,
    private router: Router
  ){}

  ngOnInit(): void {
    this.resetOnNavigation();
  }

  private resetOnNavigation():void{
      this.router.events.subscribe((event) => {
        if (event instanceof NavigationEnd) {
          this.searchTerm = null;
          this.usersFound = [];
        }
    })
  }

  public filter(event: any): void{
    if (event == null || event == undefined) return;
    if (this.userNameChanged.observers.length === 0){
      this.userNameChanged.pipe(debounceTime(500)).subscribe(
        (value: string) => {
          this.accountService.getUsersByFilter(value).subscribe(
            (response: UserUpdate[]) => this.usersFound = response,
            (error: any) => {
              this.usersFound = [];
              console.error(error)
            }
          );
        }
      );
    }
    this.userNameChanged.next(event);
  }
}

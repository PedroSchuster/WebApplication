
import { AppRoutingModule } from './app-routing.module';
import { RouterLinkActive } from '@angular/router';

import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { CollapseModule } from 'ngx-bootstrap/collapse';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TabsModule } from 'ngx-bootstrap/tabs';

import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { ptBrLocale } from 'ngx-bootstrap/locale';

import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from 'ngx-spinner';

import { AppComponent } from './app.component';




import { DateTimeFormatPipe } from './helpers/DateTimeFormat.pipe';
import { AccountService } from './services/account.service';
import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { UpperbarComponent } from './shared/uppebar/upperbar/sidenavbar/upperbar/upperbar.component';
import { SidenavbarComponent } from './shared/sidenavbar/sidenavbar.component';
import { HomeComponent } from './components/user/home/home/home.component';
import { SearchComponent } from './components/user/search/search.component';
import { TimelineComponent } from './components/user/home/home/timeline/timeline.component';
import { PostDetailsComponent } from './components/user/home/home/post-details/post-details.component';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { PostService } from './services/post.service';
import { PostTLComponent } from './components/user/home/home/post-tl/post-tl.component';
import { UserComponent } from './components/user/user/user.component';
import { LoginComponent } from './components/user/user/login/login.component';
import { ProfileComponent } from './components/user/user/profile/profile.component';
import { RegistrationComponent } from './components/user/user/registration/registration.component';
import { ProfileCardComponent } from './components/user/user/profile/profile-card/profile-card.component';
import { MatDialogModule, MatDialogRef} from '@angular/material/dialog';
import { UserRelationsComponent } from './components/user/user/profile/user-relations/user-relations.component';
import { ProfilePageComponent } from './components/user/user/profile/profile-page/profile-page.component';
import { ChatComponent } from './components/user/chat/chat.component';
import { ChatDetailsComponent } from './components/user/chat/chat-details/chat-details.component';
import { SignalrService } from './services/signalr.service';
defineLocale('pt-br', ptBrLocale);

@NgModule({
  declarations: [
    AppComponent,
    DateTimeFormatPipe,
    UserComponent,
    LoginComponent,
    RegistrationComponent,
    UpperbarComponent,
    SidenavbarComponent,
    HomeComponent,
    SearchComponent,
    TimelineComponent,
    PostDetailsComponent,
    ProfileComponent,
    PostTLComponent,
    ProfileCardComponent,
    UserRelationsComponent,
    ProfilePageComponent,
    ChatComponent,
    ChatDetailsComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    MatDialogModule,
    RouterLinkActive,
    ReactiveFormsModule ,
    InfiniteScrollModule,
    HttpClientModule,
    PaginationModule.forRoot(),
    BrowserAnimationsModule,
    CollapseModule.forRoot(),
    TooltipModule.forRoot(),
    BsDropdownModule.forRoot(),
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot(),
    TabsModule.forRoot(),
    ModalModule.forRoot(),
    ToastrModule.forRoot({
      timeOut: 3000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true
    }),
    NgxSpinnerModule,
  ],
  providers: [
    PostService,
    AccountService,
    SignalrService,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}

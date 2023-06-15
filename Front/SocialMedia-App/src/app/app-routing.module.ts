import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { UserComponent } from "./components/user/user.component";
import { LoginComponent } from "./components/user/login/login.component";
import { RegistrationComponent } from "./components/user/registration/registration.component";
import { AuthGuard } from "./guard/auth.guard";
import { HomeComponent } from "./components/user/home/home/home.component";
import { TimelineComponent } from "./components/user/home/home/timeline/timeline.component";
import { PostDetailsComponent } from "./components/user/home/home/post-details/post-details.component";


const routes: Routes = [
  {path: '', redirectTo: 'timeline', pathMatch: 'full'},
  {path: 'home', redirectTo: 'timeline'},
  {path: 'timeline', component: TimelineComponent},
  {path: 'post', component: PostDetailsComponent}
  ,
{
  path: 'user', component: UserComponent,
  children: [
    {path: 'login', component: LoginComponent},
    {path: 'registration', component: RegistrationComponent}
  ]
}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule {}

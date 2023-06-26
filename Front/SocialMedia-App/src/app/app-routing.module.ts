import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthGuard } from "./guard/auth.guard";
import { HomeComponent } from "./components/user/home/home/home.component";
import { TimelineComponent } from "./components/user/home/home/timeline/timeline.component";
import { PostDetailsComponent } from "./components/user/home/home/post-details/post-details.component";
import { LoginComponent } from "./components/user/user/login/login.component";
import { RegistrationComponent } from "./components/user/user/registration/registration.component";
import { UserComponent } from "./components/user/user/user.component";
import { ProfileComponent } from "./components/user/user/profile/profile.component";
import { UserRelationsComponent } from "./components/user/user/profile/user-relations/user-relations.component";
import { ProfilePageComponent } from "./components/user/user/profile/profile-page/profile-page.component";
import { SearchComponent } from "./components/user/search/search.component";

const routes: Routes = [
  { path: '', redirectTo: 'home/timeline', pathMatch: 'full' },
  { path: 'user', redirectTo: 'user/login', pathMatch: 'full' },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        component: HomeComponent,
        children: [
          { path: 'home/timeline', component: TimelineComponent },
          { path: 'home/post/:id', component: PostDetailsComponent },
        ],
      },
    ],
  },
  {
    path: 'user',
    component: UserComponent,
    children: [
      { path: 'login', component: LoginComponent },
      { path: 'registration', component: RegistrationComponent },
      { path: 'profile/:userName', component: ProfilePageComponent },
      { path: 'profile/:userName/:type', component: UserRelationsComponent },
    ],
  },
  {path:'search', component: SearchComponent},
  { path: '**', redirectTo: 'home/timeline', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { onSameUrlNavigation: 'reload' })],
  exports: [RouterModule],
})
export class AppRoutingModule {}

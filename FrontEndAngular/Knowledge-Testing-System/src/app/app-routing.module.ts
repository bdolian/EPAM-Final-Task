import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { CreateTestComponent } from "./create-test/create-test.component";
import { HomeComponent } from "./home/home.component";
import { LoginComponent } from "./login/login.component";
import { RegisterComponent } from "./register/register.component";
import { TestsComponent } from "./tests/tests.component";
import { UserProfileComponent } from "./user-profile/user-profile.component";

const routes: Routes = [
    {path: '', component: HomeComponent},
    {path: 'register', component: RegisterComponent},
    {path: 'login', component: LoginComponent},
    {path: 'tests/:id', component: TestsComponent},
    {path: 'test/create', component: CreateTestComponent},
    {path: 'me', component:UserProfileComponent}
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})

export class AppRoutingModule { }
export const routingComponents = [
    HomeComponent,
    RegisterComponent,
    LoginComponent,
    TestsComponent,
    CreateTestComponent,
    UserProfileComponent
]
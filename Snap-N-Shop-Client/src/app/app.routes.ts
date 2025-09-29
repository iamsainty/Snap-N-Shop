import { Routes } from '@angular/router';
import { RouterModule } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { NgModule } from '@angular/core';
import { AuthComponent } from './pages/auth/auth.component';

export const routes: Routes = [
    {path : '', component : HomeComponent},
    {path : 'auth', component : AuthComponent}
];

@NgModule({
    imports : [RouterModule.forRoot(routes)],
    exports : [RouterModule]
})
export class AppRoutesModule {}

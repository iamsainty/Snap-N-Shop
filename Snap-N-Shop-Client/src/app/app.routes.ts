import { Routes } from '@angular/router';
import { RouterModule } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { NgModule } from '@angular/core';
import { AuthComponent } from './pages/auth/auth.component';
import { BrowseComponent } from './pages/browse/browse.component';

export const routes: Routes = [
    {path : '', component : HomeComponent},
    {path : 'auth', component : AuthComponent},
    {path : 'browse', component : BrowseComponent}
];

@NgModule({
    imports : [RouterModule.forRoot(routes)],
    exports : [RouterModule]
})
export class AppRoutesModule {}

import { Routes } from '@angular/router';
import { RouterModule } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { NgModule } from '@angular/core';
import { AuthComponent } from './pages/auth/auth.component';
import { BrowseComponent } from './pages/browse/browse.component';
import { CategoryComponent } from './pages/browse/category/category.component';
import { CartComponent } from './pages/cart/cart.component';
import { CheckoutComponent } from './pages/checkout/checkout.component';

export const routes: Routes = [
    {path : '', component : HomeComponent},
    {path : 'auth', component : AuthComponent},
    {path : 'browse', component : BrowseComponent},
    {path : 'browse/:category', component : CategoryComponent},
    {path : 'cart', component : CartComponent},
    {path : 'checkout', component : CheckoutComponent}
];

@NgModule({
    imports : [RouterModule.forRoot(routes)],
    exports : [RouterModule]
})
export class AppRoutesModule {}

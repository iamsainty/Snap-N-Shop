import { Routes } from '@angular/router';
import { RouterModule } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { NgModule } from '@angular/core';
import { AuthComponent } from './pages/auth/auth.component';
import { BrowseComponent } from './pages/browse/browse.component';
import { CategoryComponent } from './pages/browse/category/category.component';
import { CartComponent } from './pages/cart/cart.component';
import { CheckoutComponent } from './pages/checkout/checkout.component';
import { PaymentComponent } from './pages/payment/payment.component';
import { OrdersComponent } from './pages/orders/orders.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
    {path : '', component : HomeComponent},
    {path : 'auth', component : AuthComponent},
    {path : 'browse', component : BrowseComponent, canActivate : [authGuard]},
    {path : 'browse/:category', component : CategoryComponent, canActivate : [authGuard]},
    {path : 'cart', component : CartComponent, canActivate : [authGuard]},
    {path : 'checkout', component : CheckoutComponent, canActivate : [authGuard]},
    {path : 'payment', component : PaymentComponent, canActivate : [authGuard]},
    {path : 'orders', component : OrdersComponent, canActivate : [authGuard]}
];

@NgModule({
    imports : [RouterModule.forRoot(routes)],
    exports : [RouterModule]
})
export class AppRoutesModule {}

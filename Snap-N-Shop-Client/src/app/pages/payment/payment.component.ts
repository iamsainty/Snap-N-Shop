import { Component, inject } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-payment',
  imports: [MatIconModule, CommonModule],
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.css'
})
export class PaymentComponent {

  serverUrl : string = 'https://snap-n-shop.onrender.com';
  router: Router = inject(Router);

  ngOnInit(): void {
    this.loadCustomer();
  }

  public loadCustomer() {
    const customerToken = localStorage.getItem('customerToken');
    if(!customerToken){
      this.router.navigate(['/auth']);
    }
  }


  public goToCart() {
    this.router.navigate(['/cart']);
  }

  public async placeOrder() {
    const customerToken = localStorage.getItem('customerToken');
    if(!customerToken) {
      this.router.navigate(['/auth']);
      return;
    }
    const url = `${this.serverUrl}/order/place-order`;
    const response = await fetch(url, {
      method : "POST",
      headers : {
        'Content-Type' : 'application/json',
        'Authorization' : `Bearer ${customerToken}`
      }
    });
    const data = await response.json();
    console.log(data);
    if(data.success) {
      this.router.navigate(['/orders']);
    } else {
      alert(data.message);
    }
  }



  goToOrders() {
    this.router.navigate(['/orders']);
  }

  logout() {
    localStorage.removeItem('customerToken');
    this.router.navigate(['/auth']);
  }
}

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

  serverUrl : string = 'http://0.0.0.0:80';
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

  public placeOrder() {
    this.router.navigate(['/orders']);
  }
}

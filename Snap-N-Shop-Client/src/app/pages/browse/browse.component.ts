import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';

@Component({
  selector: 'app-browse',
  imports: [ MatIconModule, CommonModule ],
  templateUrl: './browse.component.html',
  styleUrl: './browse.component.css'
})
export class BrowseComponent {
  products: any[] = [];
  cartItems: any[] = [];
  serverUrl : string = 'http://0.0.0.0:80';

  constructor(private router: Router) {}

    ngOnInit() {
    this.getProducts().then(() => {
      console.log(this.products);
      this.getCartItems();
    });
  }
  async getProducts() {
    const url = `${this.serverUrl}/product/all-product`;

    const response = await fetch(url, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json'
      }
    });
    const data: any = await response.json();
    this.products = data.products;
  }

  async getCartItems() {
    const customerToken = localStorage.getItem('customerToken');
    if(!customerToken) {
      this.router.navigate(['/auth']);
      return;
    }
    console.log(customerToken)
    const url = `${this.serverUrl}/cart/fetch-cart`;
    const response = await fetch(url, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${customerToken}`
      }
    });
    const data = await response.json();
    console.log(data);
    if(data.success) {
      this.cartItems = data.cartItems.map((item: any) => item.productId);
      console.log(this.cartItems);
    }
  }

  navigateToCategory(category: string) {
    if(category === 'all') {
      this.router.navigate(['/browse']);
      return;
    }
    this.router.navigate(['/browse', category]);
  }

  async addToCart(productId: number) {
    const customerToken = localStorage.getItem('customerToken');
    if(!customerToken) {
      this.router.navigate(['/auth']);
      return;
    }
    const url = `${this.serverUrl}/cart/add-to-cart`;
    const response = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${customerToken}`
      },
      body: JSON.stringify({ productId: productId })
    });
    const data = await response.json();
    console.log(data);
    if(data.success) {
      this.getCartItems();
    }
  }

  goToCart() {
    this.router.navigate(['/cart']);
  }

  goToOrders() {
    this.router.navigate(['/orders']);
  }

  logout() {
    localStorage.removeItem('customerToken');
    this.router.navigate(['/auth']);
  }
}

import { Component, inject } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-orders',
  imports: [MatIconModule, CommonModule],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.css'
})
export class OrdersComponent {

  router: Router = inject(Router);
  serverUrl: string = 'http://0.0.0.0:80';

  orders: any[] = [];
  products: any[] = [];

  ngOnInit(): void {
    this.loadOrders();
  }

  // ✅ Fetch all products
  async getProducts() {
    const url = `${this.serverUrl}/product/all-product`;

    try {
      const response = await fetch(url, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json'
        }
      });

      const data: any = await response.json();
      this.products = data.products || [];
    } catch (error) {
      console.error('Error fetching products:', error);
    }
  }

  // ✅ Fetch customer orders + merge with products
  async loadOrders() {
    const customerToken = localStorage.getItem('customerToken');
    if (!customerToken) {
      this.router.navigate(['/auth']);
      return;
    }

    const url = `${this.serverUrl}/order/fetch-orders`;

    try {
      const response = await fetch(url, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${customerToken}`
        }
      });

      const data = await response.json();
      if (data.success) {
        this.orders = data.orders || [];

        // fetch products only once
        await this.getProducts();

        // enrich each order with its product details
        this.orders = this.orders.map(order => {
          const product = this.products.find(p => p.productId === order.productId);
          return { ...order, product };
        });

        console.log("Orders with products:", this.orders);
      }
    } catch (error) {
      console.error('Error fetching orders:', error);
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
import { Component, inject } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';
import { CommonModule, CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-cart',
  imports: [MatIconModule, CommonModule, CurrencyPipe],
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent {
  cartItems: any[] = [];
  products: any[] = [];
  productCart: any[] = [];

  totalItems: number = 0;
  totalPrice: number = 0;
  totalTax: number = 0;
  totalAmount: number = 0;
  expectedDeliveryDate: string = '';

  router: Router = inject(Router);
  serverUrl: string = 'http://0.0.0.0:80';

  ngOnInit(): void {
    this.loadCart();
  }

  async loadCart() {
    await Promise.all([this.getCartItems(), this.getProducts()]);

    const productMap = new Map(this.products.map(p => [p.productId, p]));
    this.productCart = this.cartItems
      .map(item => {
        const product = productMap.get(item.productId);
        return product ? { ...item, ...product } : null;
      })
      .filter(Boolean);

    this.totalItems = this.productCart.reduce((acc: number, item: any) => acc + item.quantity, 0);
    this.totalPrice = this.productCart.reduce((acc: number, item: any) => acc + item.price * 100 * item.quantity, 0);
    this.totalTax = this.productCart.reduce((acc: number, item: any) => acc + (item.price * 100 * item.quantity) * 0.12, 0);
    this.totalAmount = this.productCart.reduce((acc: number, item: any) => acc + item.price * 100 * item.quantity + (item.price * 100 * item.quantity) * 0.12, 0);
    this.expectedDeliveryDate = new Date(new Date().getTime() + 4 * 24 * 60 * 60 * 1000).toLocaleDateString('en-US', { day: '2-digit', month: '2-digit', year: 'numeric' });
  }

  private async getCartItems() {
    const customerToken = localStorage.getItem('customerToken');
    if (!customerToken) {
      this.router.navigate(['/auth']);
      return;
    }
    const url = `${this.serverUrl}/cart/fetch-cart`;
    const response = await fetch(url, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${customerToken}`
      }
    });
    const data = await response.json();
    if (data.success) {
      this.cartItems = data.cartItems;
    }
  }

  private async getProducts() {
    const url = `${this.serverUrl}/product/all-product`;
    const response = await fetch(url, { method: 'GET' });
    const data: any = await response.json();
    this.products = data.products;
  }

  public async removeItem(productId: number) {
    const customerToken = localStorage.getItem('customerToken');
    if (!customerToken) {
      this.router.navigate(['/auth']);
      return;
    }

    const url = `${this.serverUrl}/cart/remove-from-cart`;
    const response = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${customerToken}`
      },
      body: JSON.stringify({ productId: productId })
    });
    const data = await response.json();
    if (data.success) {
      this.loadCart();
    }
  }

  public async increaseQuantity(productId: number) {
    const customerToken = localStorage.getItem('customerToken');
    if (!customerToken) {
      this.router.navigate(['/auth']);
      return;
    }

    const url = `${this.serverUrl}/cart/update-cart-item`;
    const response = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${customerToken}`
      },
      body: JSON.stringify({ productId: productId, incQty: true })
    });
    const data = await response.json();
    if (data.success) {
      this.loadCart();
    }
  }

  public async decreaseQuantity(productId: number) {
    const customerToken = localStorage.getItem('customerToken');
    if (!customerToken) {
      this.router.navigate(['/auth']);
      return;
    }

    const url = `${this.serverUrl}/cart/update-cart-item`;
    const response = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${customerToken}`
      },
      body: JSON.stringify({ productId: productId, incQty: false })
    });
    const data = await response.json();
    if (data.success) {
      this.loadCart();
    }
  }

  public goToBrowse() {
    this.router.navigate(['/browse']);
  }

  public goToCheckout() {
    this.router.navigate(['/checkout']);
  }

}
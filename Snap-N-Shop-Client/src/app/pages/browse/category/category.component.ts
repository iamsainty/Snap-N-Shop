import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-category',
  imports: [MatIconModule, CommonModule],
  templateUrl: './category.component.html',
  styleUrl: './category.component.css'
})
export class CategoryComponent implements OnInit {

  constructor(private route: ActivatedRoute, private router: Router) {}

  serverUrl : string = 'https://snap-n-shop.onrender.com';
  category: string = '';
  cartItems: any[] = [];

  allowedCategories: string[] = ['groceries', 'smartphones', 'sports', 'kitchen', 'clothing', 'footwear'];
  
  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.category = params['category'].toLowerCase();
      if(!this.allowedCategories.includes(this.category)) {
        this.router.navigate(['/browse']);
      }
      console.log(this.category);
      this.getProducts();
      console.log(this.products);
      this.getCartItems();
    })
  }

  products: any[] = [];
  async getProducts() {
    const url = `${this.serverUrl}/product/category-product`;
    const response = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({ categoryName: this.category })
    });
    const data: any = await response.json();
    this.products = data.products;
  }


  navigateToCategory(category: string) {
    if(category === 'all') {
      this.router.navigate(['/browse']);
      return;
    }
    this.router.navigate(['/browse', category]);
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

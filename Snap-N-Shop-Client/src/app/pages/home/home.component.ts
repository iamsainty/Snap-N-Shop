import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-home',
  imports: [MatIconModule, CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  products: any[] = [];
  cartItems: any[] = [];
  serverUrl : string = 'https://snap-n-shop.onrender.com';

  searchQuery: string = '';

  async onSearch(e: Event) {
    const target = e.target as HTMLInputElement;
    this.searchQuery = target.value.trim();

    await this.getProducts();
    if(this.searchQuery.length > 0) {
      this.products = this.products.filter((product) => product.productName.toLowerCase().includes(this.searchQuery.toLowerCase()) || product.productDescription.toLowerCase().includes(this.searchQuery.toLowerCase()) );
    }
  }

  constructor(private router: Router) {}

    ngOnInit() {
    this.getProducts().then(() => {
      console.log(this.products);
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

  getIn() {
    this.router.navigate(['/auth']);
  }
}

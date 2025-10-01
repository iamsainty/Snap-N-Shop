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

  constructor(private router: Router) {}

    ngOnInit() {
    this.getProducts().then(() => {
      console.log(this.products);
      this.uniqueCategories();
      this.uniqueCategoriesWithFrequency();
    });
  }
  async getProducts() {
    const serverUrl = 'http://0.0.0.0:80';
    const url = `${serverUrl}/product/all-product`;

    const response = await fetch(url, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json'
      }
    });
    const data: any = await response.json();
    this.products = data.products;
  }

  // get all unique categories with their frequency
  uniqueCategoriesWithFrequency() {
    this.categories = [...new Set(this.products.map(product => product.categoryName))];
    this.categories.forEach(category => {
      const frequency = this.products.filter(product => product.categoryName === category).length;
      console.log(category, frequency);
    });
  }

  categories: string[] = [];

  uniqueCategories() {
    this.categories = [...new Set(this.products.map(product => product.categoryName))];
    console.log(this.categories);
  }

  navigateToCategory(category: string) {
    if(category === 'all') {
      this.router.navigate(['/browse']);
      return;
    }
    this.router.navigate(['/browse', category]);
  }

}

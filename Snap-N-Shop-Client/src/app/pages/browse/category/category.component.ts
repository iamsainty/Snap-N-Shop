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

  category: string = '';

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
    })
  }

  products: any[] = [];
  async getProducts() {
    const serverUrl = 'http://0.0.0.0:80';
    const url = `${serverUrl}/product/category-product`;
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
}

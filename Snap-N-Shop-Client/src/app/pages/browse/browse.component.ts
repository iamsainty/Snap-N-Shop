import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-browse',
  imports: [ MatIconModule, CommonModule ],
  templateUrl: './browse.component.html',
  styleUrl: './browse.component.css'
})
export class BrowseComponent {
  products: any[] = [];
  ngOnInit() {
    this.getProducts().then(() => {
      console.log(this.products);
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
}

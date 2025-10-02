import { Component, inject } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  imports: [MatIconModule, CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  router: Router = inject(Router);
  products: any[] = [];
  serverUrl: string = 'http://0.0.0.0:80';

  ngOnInit(): void {
    this.loadProducts();
  }

  async loadProducts() {
    const url = `${this.serverUrl}/product/all-product`;
    const response = await fetch(url, { method: 'GET' });
    const data: any = await response.json();
    this.products = data.products;
  }

  goToSignIn() {
    this.router.navigate(['/auth']);
  }
}

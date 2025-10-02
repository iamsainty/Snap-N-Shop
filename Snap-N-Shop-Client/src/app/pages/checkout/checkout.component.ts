import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';

@Component({
  selector: 'app-checkout',
  imports: [MatIconModule, FormsModule, ReactiveFormsModule],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.css'
})
export class CheckoutComponent {

  router: Router = inject(Router);
  form: FormGroup = new FormGroup({
    displayName: new FormControl(''),
    addressLine1: new FormControl(''),
    addressLine2: new FormControl(''),
    city: new FormControl(''),
    state: new FormControl(''),
    pinCode: new FormControl(''),
    country: new FormControl(''),
    phone: new FormControl(''),
  });
  serverUrl: string = 'http://0.0.0.0:80';
  customer: any;

  ngOnInit(): void {
    this.loadCustomer();
    console.log(this.form.value);
    this.form.patchValue({
      displayName: this.customer.displayName || '',
      addressLine1: this.customer.addressLine1 || '',
      addressLine2: this.customer.addressLine2 || '',
      city: this.customer.city || '',
      state: this.customer.state || '',
      pinCode: this.customer.pinCode || '',
      country: this.customer.country || '',
      phone: this.customer.phone || '',
    });
    this.form.updateValueAndValidity();
  }

  public async loadCustomer() {
    const customerToken = localStorage.getItem('customerToken');
    if(!customerToken) {
      this.router.navigate(['/auth']);
      return;
    }
    const url = `${this.serverUrl}/customer/fetch-customer`;
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
      this.customer = data;
    }
  }

  public goToCart() {
    this.router.navigate(['/cart']);
  }

  public validateForm() : boolean {
    console.log(this.form.value);
    if(this.form.value.displayName === '' || this.form.value.addressLine1 === '' || this.form.value.city === '' || this.form.value.state === '' || this.form.value.pinCode === '' || this.form.value.country === '' || this.form.value.phone === '') {
      return false;
    }
    return true;
  }

  public async goToPayment() {
    if(!this.validateForm()) {
      alert("Please fill all fields");
      return;
    }
    const customerToken = localStorage.getItem('customerToken');
    if(!customerToken) {
      this.router.navigate(['/auth']);
      return;
    }
    const url = `${this.serverUrl}/customer/update-profile`;
    const response = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${customerToken}`
      },
      body: JSON.stringify( {
        displayName: this.form.value.displayName,
        phone: this.form.value.phone,
        addressLine1: this.form.value.addressLine1,
        addressLine2: this.form.value.addressLine2,
        city: this.form.value.city,
        state: this.form.value.state,
        pinCode: this.form.value.pinCode,
        country: this.form.value.country,
      })
    });
    const data = await response.json();
    console.log(data);
    if(data.success) {
      this.router.navigate(['/payment']);
    } else {
      alert(data.message);
    }
  }
}

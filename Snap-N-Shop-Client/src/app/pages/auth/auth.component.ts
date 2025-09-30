import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  imports: [FormsModule, CommonModule]
})
export class AuthComponent {
  email: string = '';
  otp: string = '';
  otpStep: boolean = false;
  loading: boolean = false;

  serverUrl: string = 'http://0.0.0.0:80';

  constructor(private router: Router) {}

  async sendOtp() {
    this.loading = true;
    if (!this.email) {
      alert("Please enter your email");
      return;
    }

    const url = `${this.serverUrl}/customer/send-otp`;

    const response = await fetch(url, {
      method: 'POST',
      headers : {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({ email: this.email })
    });

    const data = await response.json();
    console.log(data);

    if(data.success) {
      this.otpStep = true;
    } else {
      alert(data.message);
    }

    this.loading = false;
  }

  async verifyOtp() {
    this.loading = true;
    if (!this.otp || this.otp.length !== 6) {
      alert("Please enter valid 6 digit OTP");
      return;
    }

    const url = `${this.serverUrl}/customer/verify-otp`;

    const response = await fetch(url, {
      method: 'POST',
      headers : {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({ email: this.email, otp: this.otp })
    });

    const data = await response.json();
    console.log(data);

    if(data.success) {
      localStorage.setItem('customerToken', data.customerToken);
      this.router.navigate(['/browse']);
    } else {
      alert(data.message);
    }

    this.loading = false;
  }
}
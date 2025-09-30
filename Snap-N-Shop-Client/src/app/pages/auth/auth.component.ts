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

  constructor(private router: Router) {}

  sendOtp() {
    if (!this.email) {
      alert("Please enter your email");
      return;
    }

    // 👉 Call your backend API here to send OTP
    console.log("Sending OTP to:", this.email);

    // On success:
    this.otpStep = true;
  }

  verifyOtp() {
    if (!this.otp || this.otp.length !== 6) {
      alert("Please enter valid 6 digit OTP");
      return;
    }

    // 👉 Call your API to verify OTP here
    console.log("Verifying OTP:", this.otp);

    // On success:
    this.router.navigate(['/browse']);
  }
}
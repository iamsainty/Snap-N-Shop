import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-auth',
  imports: [FormsModule],
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent {
  email: string = '';
  otp: string = '';
  otpStep: boolean = false; // false = email phase, true = otp phase

  constructor(private router: Router) {}

  sendOtp() {
    if (!this.email) {
      alert("Please enter your email");
      return;
    }

    // 👉 Call your API to send OTP here
    console.log("Sending OTP to:", this.email);

    // If success:
    this.otpStep = true;
  }

  verifyOtp() {
    if (!this.otp || this.otp.length !== 6) {
      alert("Please enter valid 6 digit OTP");
      return;
    }

    // 👉 Call your API to verify OTP here
    console.log("Verifying OTP:", this.otp);

    // If success:
    this.router.navigate(['/browse']);
  }
}
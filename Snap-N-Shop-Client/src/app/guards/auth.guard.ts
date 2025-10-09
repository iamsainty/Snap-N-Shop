import { CanActivateFn } from '@angular/router';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
export const authGuard: CanActivateFn = async (route, state) => {

  const router = inject(Router);
  
  const customerToken = localStorage.getItem('customerToken');
  if(!customerToken) {
    router.navigate(['/auth']);
    return false;
  }
  const serverUrl = 'https://snap-n-shop.onrender.com';
  const url = `${serverUrl}/customer/fetch-customer`;
  

  const response = await fetch(url, {
    method: 'GET',
    headers : {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${customerToken}`
    }
  });

  console.log(response);

  const data = await response.json();

  console.log(data);
  if(data.success) {
    return true;
  }
  router.navigate(['/']);
  return false;
};

import { CanActivateFn } from '@angular/router';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
export const authGuard: CanActivateFn = async (route, state) => {

  const http = inject(HttpClient);

  const router = inject(Router);
  
  const customerToken = localStorage.getItem('customerToken');
  if(!customerToken) {
    router.navigate(['/auth']);
    return false;
  }
  const serverUrl = 'http://0.0.0.0:80';
  const url = `${serverUrl}/customer/fetch-customer`;
  

  const response: any = await firstValueFrom(
    http.get(url, {
      headers : {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${customerToken}`
      }
    })
  )

  console.log(response);

  const data = await response.json();

  console.log(data);
  if(data.success) {
    return true;
  }
  return false;
};

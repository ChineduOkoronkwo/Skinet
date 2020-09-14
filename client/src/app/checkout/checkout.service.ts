import { IOrderToCreate } from './../shared/models/orderToCreate';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from './../../environments/environment';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';

import { IDeliveryMethod } from './../shared/models/deliverymethod';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  createOrder(order: IOrderToCreate): Observable<any> {
    return this.http.post<IOrderToCreate>(`${this.baseUrl}orders`, order);
  }

  getDeliveryMethods(): Observable<IDeliveryMethod[]> {
    return this.http.get<IDeliveryMethod[]>(`${this.baseUrl}orders/deliveryMethods`).pipe(
      map(dm => {
        return dm.sort((a, b) => b.price - a.price);
      })
    );
  }
}

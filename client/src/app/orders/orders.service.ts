import { IOrder } from './../shared/models/orderToCreate';
import { HttpClient } from '@angular/common/http';
import { environment } from './../../environments/environment';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getOrdersForUser(): Observable<IOrder[]> {
    return this.http.get<IOrder[]>(`${this.baseUrl}orders`);
  }
}

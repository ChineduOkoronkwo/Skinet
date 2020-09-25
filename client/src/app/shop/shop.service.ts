import { IProduct } from './../shared/models/product';
import { ShopParams } from './../shared/models/shopParams';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { IBrand } from '../shared/models/productBrand';
import { IPagination } from '../shared/models/pagination';
import { IType } from '../shared/models/productType';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) { }

  getProducts(shopParams: ShopParams): Observable<IPagination> {
    let params = new HttpParams();
    // params = params.append('pageSize', '50');
    if (shopParams.brandId !== 0) {
      params = params.append('brandId', shopParams.brandId.toString());
    }
    if (shopParams.typeId !== 0) {
      params = params.append('typeId', shopParams.typeId.toString());
    }

    params = params.append('sort', shopParams.sort);
    params = params.append('pageIndex', shopParams.pageNumber.toString());
    params = params.append('pageSize', shopParams.pageSize.toString());

    if (shopParams.search) {
      params = params.append('search', shopParams.search);
    }

    return this.http.get<IPagination>(`${this.baseUrl}products`, {observe: 'response', params})
    .pipe(
      map(response => {
        return response.body;
      })
    );
  }

  getProduct(id: number): Observable<IProduct> {
    return this.http.get<IProduct>(`${this.baseUrl}products/${id}`);
  }

  getBrands(): Observable<IBrand[]> {
    return this.http.get<IBrand[]>(`${this.baseUrl}products/brands`);
  }

  getTypes(): Observable<IType[]> {
    return this.http.get<IType[]>(`${this.baseUrl}products/types`);
  }
}

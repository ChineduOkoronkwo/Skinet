import { environment } from './../../environments/environment';
import { Pagination } from './../shared/models/pagination';
import { IProduct } from './../shared/models/product';
import { ShopParams } from './../shared/models/shopParams';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';

import { IBrand } from '../shared/models/productBrand';
import { IPagination } from '../shared/models/pagination';
import { IType } from '../shared/models/productType';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = environment.apiUrl;
  products: IProduct[] = [];
  brands: IBrand[] = [];
  types: IType[] = [];
  paginattion = new Pagination();
  shopParams = new ShopParams();

  constructor(private http: HttpClient) { }

  getProducts(useCache: boolean): Observable<IPagination> {
    if (!useCache) {
      this.products = [];
    }

    if (this.products.length > 0 && useCache === true) {
      const pagesReceived = Math.ceil (this.products.length / this.shopParams.pageSize);

      if (this.shopParams.pageNumber <= pagesReceived) {
        this.paginattion.data =
          this.products.slice((this.shopParams.pageNumber - 1) * this.shopParams.pageSize,
            this.shopParams.pageNumber * this.shopParams.pageSize);
        return of(this.paginattion);
      }
    }

    let params = new HttpParams();
    // params = params.append('pageSize', '50');
    if (this.shopParams.brandId !== 0) {
      params = params.append('brandId', this.shopParams.brandId.toString());
    }
    if (this.shopParams.typeId !== 0) {
      params = params.append('typeId', this.shopParams.typeId.toString());
    }

    params = params.append('sort', this.shopParams.sort);
    params = params.append('pageIndex', this.shopParams.pageNumber.toString());
    params = params.append('pageSize', this.shopParams.pageSize.toString());

    if (this.shopParams.search) {
      params = params.append('search', this.shopParams.search);
    }

    return this.http.get<IPagination>(`${this.baseUrl}products`, {observe: 'response', params})
    .pipe(
      map(response => {
        this.products = [...this.products, ...response.body.data];
        this.paginattion =  response.body;
        return this.paginattion;
      })
    );
  }

  setShopParams(shopParams: ShopParams): void {
    this.shopParams = shopParams;
  }

  getShopParams(): ShopParams {
    return this.shopParams;
  }

  getProduct(id: number): Observable<IProduct> {
    const product = this.products.find(p => p.id === id);
    if (product) {
      return of(product);
    }
    return this.http.get<IProduct>(`${this.baseUrl}products/${id}`);
  }

  getBrands(): Observable<IBrand[]> {
    if (this.brands.length > 0) {
      return of(this.brands);
    }
    return this.http.get<IBrand[]>(`${this.baseUrl}products/brands`);
  }

  getTypes(): Observable<IType[]> {
    if (this.types.length > 0) {
      return of(this.types);
    }

    return this.http.get<IType[]>(`${this.baseUrl}products/types`);
  }
}

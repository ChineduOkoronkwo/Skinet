import { IProduct } from './../shared/models/product';
import { HttpClient } from '@angular/common/http';
import { IBasket, IBasketItem, Basket, IBasketTotals } from './../shared/models/basket';
import { environment } from './../../environments/environment';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl = environment.apiUrl;
  private basketSource = new BehaviorSubject<IBasket>(null);
  basket$ = this.basketSource.asObservable();
  private basketTotalSource = new BehaviorSubject<IBasketTotals>(null);
  basketTotal$ = this.basketTotalSource.asObservable();

  constructor(private http: HttpClient) { }

  getBasket(id: string): Observable<any> {
    return this.http.get(`${this.baseUrl}basket?id=${id}`)
      .pipe(
        map((basket: IBasket) => {
          this.basketSource.next(basket);
          this.calculateTotal();
        })
      );
  }

  setBasket(basket: IBasket): any {
    return this.http.post(`${this.baseUrl}basket`, basket).subscribe((respone: IBasket) => {
      this.basketSource.next(respone);
      this.calculateTotal();
    }, error => {
      console.log(error);
    });
  }

  deleteBasket(id: string): void {
    this.http.delete(`${this.baseUrl}basket?id=${id}`).subscribe(() => {
      this.basketSource.next(null);
      this.basketTotalSource.next(null);
      localStorage.removeItem('basket_id');
    }, error => {
      console.log(error);
    });
  }

  getCurrentBasketValue(): IBasket {
    return this.basketSource.value;
  }

   addItemToBasket(item: IProduct, quantity: number): void {
    const itemtoAdd: IBasketItem = this.mapProductToBasketItem(item, quantity);
    const basket = this.getCurrentBasketValue() ?? this.createbasket();

    const existingItem = basket.items.find(i => i.id === itemtoAdd.id);
    if (existingItem) {
      existingItem.quantity += quantity;
    } else {
      basket.items.push(itemtoAdd);
    }

    this.setBasket(basket);
  }

  private createbasket(): IBasket {
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }

  private mapProductToBasketItem(item: IProduct, quantity: number): IBasketItem {
    return {
      id: item.id,
      productName: item.name,
      price: item.price,
      pictureUrl: item.pictureUrl,
      quantity,
      brand: item.productBrand,
      type: item.productType
    };
  }

  private calculateTotal(): void {
    const basket = this.getCurrentBasketValue();
    const shipping = 0;
    const tax = 0;
    const subtotal = basket.items.reduce((a, b) => (b.price * b.quantity) + a, 0);
    const total = shipping + tax + subtotal;
    this.basketTotalSource.next({shipping, tax, subtotal, total});
  }

  updateItemQuantity(item: IBasketItem, quantity: number): void {
    const basket = this.getCurrentBasketValue();
    const index = basket.items.findIndex(x => x.id === item.id);
    if (index !== -1) {
      basket.items[index].quantity += quantity;
      if (item.quantity === 0) {
        this.removeItem(item);
      } else {
        this.setBasket(basket);
      }
    }
  }

  removeItem(item: IBasketItem): void {
    // Get reference to the basket
    const basket = this.getCurrentBasketValue();

    // Find index of item and use it to validate that item exists
    const index = basket.items.findIndex(x => x.id === item.id);
    if (index > -1) {
      // Pick items with id not equal to current item
      basket.items = basket.items.filter(x => x.id !== item.id);
    }

    if (basket.items.length === 0) {
      // Basket is empty, delete it from the server
      this.deleteBasket(basket.id);
    } else {
      // Update basket to current state
      this.setBasket(basket);
    }
  }
}

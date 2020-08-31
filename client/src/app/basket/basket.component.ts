import { IBasketItem } from './../shared/models/basket';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BasketService } from './basket.service';
import { IBasket } from '../shared/models/basket';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.scss']
})
export class BasketComponent implements OnInit {
  basket$: Observable<IBasket>;

  constructor(private basketService: BasketService) { }

  ngOnInit(): void {
    this.getBasket();
  }

  private getBasket(): void {
    this.basket$ = this.basketService.basket$;
  }

  removeItem(item: IBasketItem): void {
    this.basketService.removeItem(item);
  }

  incrementQuantity(item: IBasketItem): void {
    this.basketService.updateItemQuantity(item, 1);
  }

  decrementQuantity(item: IBasketItem): void {
    this.basketService.updateItemQuantity(item, -1);
  }
}

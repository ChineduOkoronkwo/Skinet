import { BasketService } from './../../../basket/basket.service';
import { Observable } from 'rxjs';
import { Basket, IBasket, IBasketItem } from './../../models/basket';
import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-basket-summary',
  templateUrl: './basket-summary.component.html',
  styleUrls: ['./basket-summary.component.scss']
})
export class BasketSummaryComponent implements OnInit {
  basket$: Observable<IBasket>;
  @Output() decrement: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();
  @Output() increment: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();
  @Output() remove: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();
  @Input() isBasket = true;

  constructor(private basketService: BasketService) { }

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
  }

  decrementItemQuantity(item: IBasketItem): any {
    this.decrement.emit(item);
  }

  incrementItemQuantity(item: IBasketItem): any {
    this.increment.emit(item);
  }

  removeItem(item: IBasketItem): any {
    this.remove.emit(item);
  }

}

import { IDeliveryMethod } from './../../shared/models/deliverymethod';
import { BasketService } from './../../basket/basket.service';
import { FormGroup } from '@angular/forms';
import { CheckoutService } from './../checkout.service';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-checkout-delivery',
  templateUrl: './checkout-delivery.component.html',
  styleUrls: ['./checkout-delivery.component.scss']
})
export class CheckoutDeliveryComponent implements OnInit {
  @Input() checkoutForm: FormGroup;
  deliveryMethods: IDeliveryMethod[];

  constructor(private checkoutService: CheckoutService, private basketService: BasketService) { }

  ngOnInit(): void {
    this.getDeliveryMethod();
  }

  getDeliveryMethod(): void {
    this.checkoutService.getDeliveryMethods().subscribe(response => {
      this.deliveryMethods = response;
    }, error => {
      console.log(error);
    });

  }

  setShippingPrice(deliveryMethod: IDeliveryMethod): void {
    this.basketService.setShippingPrice(deliveryMethod);
  }

}

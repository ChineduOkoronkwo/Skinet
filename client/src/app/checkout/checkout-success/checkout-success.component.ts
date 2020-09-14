import { Router, NavigationExtras } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { IOrder } from 'src/app/shared/models/orderToCreate';

@Component({
  selector: 'app-checkout-success',
  templateUrl: './checkout-success.component.html',
  styleUrls: ['./checkout-success.component.scss']
})
export class CheckoutSuccessComponent implements OnInit {
  order: IOrder;

  constructor(private router: Router) {
    const navigation = this.router.getCurrentNavigation();
    const state = navigation && navigation.extras && navigation.extras.state;
    if (state) {
      this.order = state as IOrder;
    }
   }

  ngOnInit(): void {
  }

  viewOrder(): void {
    const navigationExtras: NavigationExtras = {state: this.order};
    this.router.navigate(['order/orderDetails'], navigationExtras);
  }

}

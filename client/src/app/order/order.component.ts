import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { OrderService } from './order.service';
import { Component, OnInit } from '@angular/core';
import { IOrder } from '../shared/models/orderToCreate';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss']
})
export class OrderComponent implements OnInit {
  orders: IOrder[];
  constructor(
    private orderService: OrderService,
    private toastr: ToastrService,
    private router: Router,
  ) { }

  ngOnInit(): void {
    this.getOrderForUsers();
  }

  getOrderForUsers(): void {
    this.orderService.getOrdersForUser().subscribe(orders => {
      this.orders = orders;
    }, error => {
      console.log(error);
      this.toastr.error(error.message);
    });
  }

  viewOrder(order: IOrder): void {
    const navigationExtras: NavigationExtras = {state: order};
    this.router.navigate(['orders/orderDetails'], navigationExtras);
  }
}

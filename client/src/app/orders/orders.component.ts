import { OrdersService } from './orders.service';
import { Component, OnInit } from '@angular/core';
import { IOrder } from '../shared/models/orderToCreate';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {

  orders: IOrder[];
  constructor(
    private ordersService: OrdersService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.getOrderForUsers();
  }

  getOrderForUsers(): void {
    this.ordersService.getOrdersForUser().subscribe(orders => {
      this.orders = orders;
      console.log('orders loaded');
      console.log(orders);
    }, error => {
      console.log(error);
      this.toastr.error(error.message);
    });
  }

  viewOrder(order: IOrder): void {
  }

}

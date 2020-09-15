import { OrderDetailsComponent } from './../order/order-details/order-details.component';
import { Routes, RouterModule } from '@angular/router';
import { OrdersComponent } from './orders.component';
import { NgModule } from '@angular/core';


const routes: Routes = [
  {path: '', component: OrdersComponent},
  {path: 'orderDetails', component: OrderDetailsComponent}
];
@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class OrdersRoutingModule { }

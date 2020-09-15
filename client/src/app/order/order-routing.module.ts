import { OrderDetailsComponent } from './order-details/order-details.component';
import { OrderComponent } from './order.component';
import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';

const routes: Routes = [
  {path: '', component: OrderComponent},
  {path: 'orderDetails', component: OrderDetailsComponent}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class OrderRoutingModule { }


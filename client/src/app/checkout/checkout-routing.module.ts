import { CheckoutComponent } from './checkout.component';
import { Routes, RouterModule } from '@angular/router';
import { NgModule, Component } from '@angular/core';

const routes: Routes = [
  {path: '', component: CheckoutComponent}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class CheckoutRoutingModule { }
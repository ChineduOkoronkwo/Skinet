import { ShopService } from './../shop.service';
import { IProduct } from './../../shared/models/product';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styles: [
  ]
})
export class ProductDetailsComponent implements OnInit {
  product: IProduct;

  constructor(
    private shopService: ShopService,
    private activatedRoute: ActivatedRoute,
    private bcService: BreadcrumbService
    )
    {
      this.bcService.set('@productDetails', '');
    }

  ngOnInit(): void {
    this.getProduct();
  }

  getProduct(): void {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    this.shopService.getProduct(+id).subscribe(response => {
        this.product = response;
        this.bcService.set('@productDetails', this.product.name);
      },
      error => {
        console.log(error);
      });
  }

}

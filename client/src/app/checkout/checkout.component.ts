import { IBasketTotals } from './../shared/models/basket';
import { Observable } from 'rxjs';
import { BasketService } from './../basket/basket.service';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from './../account/account.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {
  checkoutForm: FormGroup;
  basketTotals$: Observable<IBasketTotals>;

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private toastr: ToastrService,
    private basketService: BasketService
  ) { }

  ngOnInit(): void {
    this.createCheckoutForm();
    this.getAddressFormValues();
    this.getDeliveryMethodFormValue();
    this.basketTotals$ = this.basketService.basketTotal$;
  }

  createCheckoutForm(): void {
    this.checkoutForm = this.fb.group({
      addressForm: this.fb.group ({
        firstName: [null, Validators.required],
        lastName: [null, Validators.required],
        street: [null, Validators.required],
        city: [null, Validators.required],
        state: [null, Validators.required],
        zipcode: [null, Validators.required]
      }),
      deliveryForm: this.fb.group ({
        deliveryMethod: [null, Validators.required]
      }),
      paymentForm: this.fb.group ({
        nameOnCard: [null, Validators.required]
      })
    });
  }

  getAddressFormValues(): void {
    this.accountService.getUserAddress().subscribe(address => {
      if (address) {
        this.checkoutForm.get('addressForm').patchValue(address);
      }
    }, error => {
      this.toastr.error(error.message);
      console.log(error);
    });
  }

  getDeliveryMethodFormValue(): void {
    const basket = this.basketService.getCurrentBasketValue();
    if (basket && basket.deliveryMethodId) {
      this.checkoutForm.get('deliveryForm').get('deliveryMethod')
        .patchValue(basket.deliveryMethodId.toString());
    }
  }

}

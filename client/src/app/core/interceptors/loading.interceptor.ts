import { delay, finalize } from 'rxjs/operators';
import { BusyService } from './../services/busy.service';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
    constructor(private busyService: BusyService) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (req.url.includes('emailExists') || (req.url.includes('orders') && req.method === 'POST')) {
            return next.handle(req);
        }

        this.busyService.busy();

        return next.handle(req).pipe(
            // delay(1000),
            finalize(() => {
                this.busyService.idle();
            })
        );
    }

}

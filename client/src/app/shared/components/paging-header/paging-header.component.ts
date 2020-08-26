import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-paging-header',
  templateUrl: './paging-header.component.html',
  styleUrls: ['./paging-header.component.scss']
})
export class PagingHeaderComponent implements OnInit {
  @Input() pageNumber;
  @Input() pageSize;
  @Input() totalCount;

  constructor() { }

  ngOnInit(): void {
  }

}

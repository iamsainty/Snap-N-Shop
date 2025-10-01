import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-category',
  imports: [],
  templateUrl: './category.component.html',
  styleUrl: './category.component.css'
})
export class CategoryComponent implements OnInit {

  constructor(private route: ActivatedRoute) {}

  category: string = '';

  ngOnInit(): void {
    this.category = this.route.snapshot.params['category'];
    console.log(this.category);
  }

}

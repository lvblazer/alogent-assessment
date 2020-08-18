import { Component, OnInit } from '@angular/core';
import { Board } from '../models/board';
import { PostIt } from '../models/post-it';
import { HttpClient } from '@angular/common/http';
import { error } from 'protractor';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {

  boards: Board[];
  url: = "http://localhost:5000/api";
  valid: boolean = true;
  selectedBoardIndex: number;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.http.get<Board[]>(this.url.concat('/boards')).subscribe(data => {    
      this.boards = data;
    });
  }

  onSubmit(f: NgForm, index: number) {
   

    var post: PostIt = { id: boardId, text: f.controls.posttext.value };
 
    var boardId = this.boards[index].id;
    this.selectedBoardIndex = index;
    this.valid = (post.text.trim().length > 0);


    if (this.valid) {
      var u = `${this.url}/boards/${boardId}/post-its`;
      this.http.post<PostIt>(u, post).subscribe(value => { this.boards[index].postIts.push(value) }, error => { console.log(error) });
    }

  }
}

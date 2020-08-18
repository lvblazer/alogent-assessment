using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Assessment.Web.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;

namespace Assessment.Web.Controllers
{
    [Route("api/[controller]")]

    public class BoardsController : Controller
    {
        public IBoardRepository boards;

        public BoardsController(IBoardRepository boards)
        {
            this.boards = boards;
        }

        [HttpGet]
        public IEnumerable<Board> GetAll()
        {
            return boards.GetAll();
        }


        [HttpPost]
        public Board Add([FromBody] Board board) {

            board.Id = this.boards.GetAll().Count<Board>();
            while (!boards.Add(board)) {
                board.Id++;
            }

            return board;

        }

        [HttpDelete("{id}")]
        public void Delete([FromRoute]int id)
        {

            var board = this.boards.Find(id);

            if (board != null)
            {
                this.boards.Delete(board);
            }

        }

        [HttpPost("{id}/post-its")]
        public PostIt AddPostIt([FromRoute] int id, [FromBody] PostIt postIt) {

            var board = this.boards.Find(id);

            if (board != null)
            {
                if (string.IsNullOrWhiteSpace(postIt.Text)) {
                    throw new ArgumentException("The text cannot be empty or whitespace only.");
                }


                postIt.CreatedAt = DateTime.Now;
                postIt.Id = board.PostIts.Count() + 1;
                
                while (this.boards.FindPostIt(board, postIt.Id) != null) {
                    postIt.Id++;
                }
                
                board.PostIts.Add(postIt);

                return postIt;
            }

            return null;
        }



        [HttpGet("{id}/post-its")]
        public IList<PostIt> GetPins([FromRoute]int id) {

            var board = this.boards.Find(id);

            if (board != null)
            {
                return board.PostIts;
            }

            return new List<PostIt>();
        }


        [HttpDelete("{id}/post-its/{postItId}")]
        public void DeletePostIt([FromRoute] int id, [FromRoute]  int postItId) {

            var board = this.boards.Find(id);
           
            if (board.PostIts != null) {
                var p = board.PostIts.FirstOrDefault(x => x.Id == postItId);
                if (p != null) { board.PostIts.Remove(p); }

            }            

        }


        [HttpGet("{id}")]
        public Board Find(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Board ID must be greater than zero.");

            return boards.Find(id);
        }
    }
}

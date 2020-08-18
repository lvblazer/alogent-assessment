using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Assessment.Web.Models
{
    public interface IBoardRepository
    {
        IQueryable<Board> GetAll();
        Board Find(int id);
        PostIt FindPostIt(Board board, int postId);
        bool Add(Board board);
        bool Delete(Board board);
    }

    public class BoardRepository : IBoardRepository
    {
        private List<Board> boards;

        public BoardRepository()
        {
            boards = GetBoardsFromFile();

        }

        private List<Board> GetBoardsFromFile()
        {
            var filePath = Application.Configuration["DataFile"];
            if (!Path.IsPathRooted(filePath)) filePath = Path.Combine(Directory.GetCurrentDirectory(), filePath);

            var json = System.IO.File.ReadAllText(filePath);

            return JsonConvert.DeserializeObject<List<Board>>(json);
        }

        public IQueryable<Board> GetAll()
        {
            return boards.AsQueryable();
        }

        public Board Find(int id)
        {
            return boards.FirstOrDefault(x => x.Id == id);
        }

        public bool Add(Board board)
        {
            if (Find(board.Id) != null) return false;

            boards.Add(board);

            return true;
        }

        public PostIt FindPostIt(Board board, int postItId) {
            

            if (board != null && board.PostIts != null) {
                return (board.PostIts.FirstOrDefault<PostIt>(p => p.Id == postItId));
            }
            return null;
        }

        public bool Delete(Board board)
        {
            if (Find(board.Id) == null) return false;
            return boards.Remove(board);
        }
    }
}

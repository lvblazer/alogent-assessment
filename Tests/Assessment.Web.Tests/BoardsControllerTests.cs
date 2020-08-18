using System;
using System.Collections.Generic;
using Assessment.Web.Controllers;
using Assessment.Web.Models;
using Moq;
using NUnit.Framework;

namespace Assessment.Web.Tests
{
    [TestFixture]
    class BoardsControllerTests
    {
        [Test]
        public void Constructor_CreatesController()
        {
            var boardRepo = Mock.Of<IBoardRepository>();
            var controller = new BoardsController(boardRepo);
            Assert.NotNull(controller);
        }

        [Test]
        public void GetAll_DoesLookupThroughRepository()
        {
            var boardRepo = new Mock<IBoardRepository>();
            var controller = new BoardsController(boardRepo.Object);

            controller.GetAll();

            boardRepo.Verify(x => x.GetAll(), Times.Once);
        }

        [Test]
        public void Find_NegativeId_ThrowsOutOfRangeException()
        {
            var boardRepo = Mock.Of<IBoardRepository>();
            var controller = new BoardsController(boardRepo);
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                controller.Find(-1);
            });
        }

        [Test]
        public void Find_ZeroId_ThrowsOutOfRangeException()
        {
            var boardRepo = Mock.Of<IBoardRepository>();
            var controller = new BoardsController(boardRepo);
            Assert.Throws<ArgumentOutOfRangeException>(() => 
            {
                controller.Find(0);
            });
        }

        [Test]
        public void Find_ValidId_DoesLookupThroughRepository()
        {
            var boardRepo = new Mock<IBoardRepository>();
            boardRepo.Setup(x => x.Find(It.IsAny<int>())).Returns(new Board());

            var controller = new BoardsController(boardRepo.Object);

            controller.Find(1);

            boardRepo.Verify(x => x.Find(1), Times.Once);
        }

        [Test]
        public void Add_ValidPostIt()
        {
            var boardRepo = new Mock<IBoardRepository>();

            var board = new Board() { Id = 1, Name = "Test" };

            boardRepo.Setup(x => x.Find(It.IsAny<int>())).Returns(board);
            boardRepo.Setup(x => x.FindPostIt(It.IsAny<Board>(), It.Is<int>(pid => pid == 1))).Returns(new PostIt());

            var controller = new BoardsController(boardRepo.Object);

            var p = new PostIt { Text = "Sample" };
            controller.AddPostIt(1, p);

            boardRepo.Verify(x => x.FindPostIt(board, 2), Times.Once);
        }


        [Test]
        public void Add_InvalidPostIt_MissingText() {

            var boardRepo = new Mock<IBoardRepository>();
            boardRepo.Setup(x => x.Find(It.IsAny<int>())).Returns(new Board());

            var controller = new BoardsController(boardRepo.Object);

            Assert.Throws<ArgumentException>(() =>
            {
                controller.AddPostIt(1, new PostIt());
            });

        }


        [Test]
        public void Get_PostIts()
        {

            var board = new Board();            

            var boardRepo = new Mock<IBoardRepository>();
            boardRepo.Setup(x => x.Find(It.IsAny<int>())).Returns(new Board()               
                {
                    PostIts = new List<PostIt>() { new PostIt(), new PostIt() }
                });

            var controller = new BoardsController(boardRepo.Object);
            var results = controller.GetPins(1);

            Assert.AreEqual(results.Count, 2);
            
        }


        [Test]
        public void Get_PostIts_BoardNotFound()
        {

            //var board = new Board();

            var boardRepo = new Mock<IBoardRepository>();
            //boardRepo.Setup(x => x.Find(It.IsAny<int>())).Returns(new Board()
            //{
            //    PostIts = new List<PostIt>() { new PostIt(), new PostIt() }
            //});

            var controller = new BoardsController(boardRepo.Object);
            var results = controller.GetPins(1);

            Assert.AreEqual(results.Count, 0);

        }



        [Test]
        public void Delete_PostIt_DoesLookupThroughRepository()
        {
            var boardRepo = new Mock<IBoardRepository>();

            var board = new Board() { Id = 1, Name = "Sample"};

            boardRepo.Setup(x => x.Find(It.IsAny<int>())).Returns(new Board());
            boardRepo.Setup(x => x.FindPostIt(It.IsAny<Board>(), It.Is<int>(i => i == 1))).Returns(
                    new PostIt() { Id = 1, Text = "Sample"}
                );

            var controller = new BoardsController(boardRepo.Object);
            controller.DeletePostIt(1, 1);
                       

            boardRepo.Verify(x => x.FindPostIt(board, 1), Times.Never);
        }

    }
}

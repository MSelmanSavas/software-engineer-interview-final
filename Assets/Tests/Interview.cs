using System.Linq;
using NUnit.Framework;

namespace Tests
{
    public class Interview
    {
        [Test]
        public void Test_BoardTraverser()
        {
            var boardHelper = BoardHelper.Instance;
            var board = boardHelper.CreateBoard(4, 4);

            /*
             * 4 1 2 1
             * 3 4 3 2
             * 2 3 2 3
             * 1 2 3 4
             */
            var matchableTypes = new[]
            {
                1, 2, 3, 4,
                2, 3, 4, 1,
                3, 2, 3, 2,
                4, 3, 2, 1
            };

            boardHelper.FillBoardWithMatchableTypes(matchableTypes);
            var enumeratorMatchableTypes = BoardHelper.GetBoardTraverser(board, TraverseCorner.BottomLeft).Select(boardItem => boardItem.MatchableType).ToList();
            Assert.AreEqual(16, enumeratorMatchableTypes.Count);

            Assert.That(new[] { 1, 2, 3, 4, 2, 3, 4, 1, 3, 2, 3, 2, 4, 3, 2, 1 }, Is.EquivalentTo(enumeratorMatchableTypes));

            enumeratorMatchableTypes = BoardHelper.GetBoardTraverser(board, TraverseCorner.BottomRight).Select(boardItem => boardItem.MatchableType).ToList();
            Assert.AreEqual(16, enumeratorMatchableTypes.Count);

            Assert.That(new[] { 4, 3, 2, 1, 3, 2, 3, 2, 2, 3, 4, 1, 1, 2, 3, 4 }, Is.EquivalentTo(enumeratorMatchableTypes));

            enumeratorMatchableTypes = BoardHelper.GetBoardTraverser(board, TraverseCorner.TopLeft).Select(boardItem => boardItem.MatchableType).ToList();
            Assert.AreEqual(16, enumeratorMatchableTypes.Count);

            Assert.That(new[] { 4, 3, 2, 1, 1, 4, 3, 2, 2, 3, 2, 3, 1, 2, 3, 4 }, Is.EquivalentTo(enumeratorMatchableTypes));

            enumeratorMatchableTypes = BoardHelper.GetBoardTraverser(board, TraverseCorner.TopRight).Select(boardItem => boardItem.MatchableType).ToList();
            Assert.AreEqual(16, enumeratorMatchableTypes.Count);

            Assert.That(new[] { 1, 2, 3, 4, 2, 3, 2, 3, 1, 4, 3, 2, 4, 3, 2, 1 }, Is.EquivalentTo(enumeratorMatchableTypes));
        }

        [Test]
        public void Test_CheckMatchExist_FindMatch()
        {
            var boardHelper = BoardHelper.Instance;
            boardHelper.CreateBoard(4, 4);

            var matchableTypes = new[]
            {
                1, 2, 3, 4,
                2, 3, 4, 1,
                1, 2, 3, 4,
                4, 3, 2, 1
            };

            boardHelper.FillBoardWithMatchableTypes(matchableTypes);

            Assert.IsTrue(boardHelper.CheckMatchExists(new BoardPosition(1, 0), new BoardPosition(1, 1)));
        }

        [Test]
        public void Test_CheckMatchExist_FindMatch_2()
        {
            var boardHelper = BoardHelper.Instance;
            boardHelper.CreateBoard(4, 4);

            var matchableTypes = new[]
            {
                1, 2, 3, 4,
                2, 3, 4, 1,
                1, 2, 3, 4,
                4, 3, 2, 1
            };

            boardHelper.FillBoardWithMatchableTypes(matchableTypes);

            Assert.IsTrue(boardHelper.CheckMatchExists(new BoardPosition(1, 2), new BoardPosition(1, 3)));
        }

        [Test]
        public void Test_CheckMatchExist_CantFindMatch()
        {
            var boardHelper = BoardHelper.Instance;
            boardHelper.CreateBoard(4, 4);

            var matchableTypes = new[]
            {
                1, 2, 3, 4,
                2, 3, 4, 1,
                1, 2, 3, 4,
                4, 3, 2, 1
            };

            boardHelper.FillBoardWithMatchableTypes(matchableTypes);

            Assert.IsFalse(boardHelper.CheckMatchExists(new BoardPosition(0, 0), new BoardPosition(0, 1)));
        }

        [Test]
        public void Test_CheckMatchExist_CantFindMatch_2()
        {
            var boardHelper = BoardHelper.Instance;
            boardHelper.CreateBoard(4, 4);

            var matchableTypes = new[]
            {
                1, 2, 3, 4,
                2, 3, 4, 1,
                1, 2, 3, 4,
                4, 3, 2, 1
            };

            boardHelper.FillBoardWithMatchableTypes(matchableTypes);

            Assert.IsFalse(boardHelper.CheckMatchExists(new BoardPosition(2, 0), new BoardPosition(2, 1)));
        }

        [Test]
        public void Test_FindAllPossibleMatches()
        {
            var boardHelper = BoardHelper.Instance;
            boardHelper.CreateBoard(4, 4);

            var matchableTypes = new[]
            {
                1, 2, 3, 4,
                2, 3, 4, 1,
                3, 2, 3, 2,
                4, 3, 2, 1
            };

            boardHelper.FillBoardWithMatchableTypes(matchableTypes);
            var matches = boardHelper.FindAllPossibleMatches();

            Assert.AreEqual(14, matches.Count);
        }

        [Test]
        public void Test_FindAllPossibleMatches_8Match()
        {
            var boardHelper = BoardHelper.Instance;
            boardHelper.CreateBoard(4, 4);

            var matchableTypes = new[]
            {
                1, 2, 3, 4,
                2, 3, 4, 1,
                1, 2, 3, 4,
                4, 3, 2, 1
            };

            boardHelper.FillBoardWithMatchableTypes(matchableTypes);
            var matches = boardHelper.FindAllPossibleMatches();

            Assert.AreEqual(8, matches.Count);
        }

        [Test]
        public void Test_FindAllPossibleMatches_4Match()
        {
            var boardHelper = BoardHelper.Instance;
            boardHelper.CreateBoard(4, 4);

            var matchableTypes = new[]
            {
                1, 2, 3, 4,
                2, 3, 4, 1,
                3, 4, 1, 2,
                4, 1, 2, 1
            };

            boardHelper.FillBoardWithMatchableTypes(matchableTypes);
            var matches = boardHelper.FindAllPossibleMatches();

            Assert.AreEqual(4, matches.Count);
        }


        [Test]
        public void Test_FindAllPossibleMatches_NoMatch()
        {
            var boardHelper = BoardHelper.Instance;
            boardHelper.CreateBoard(4, 4);

            var matchableTypes = new[]
            {
                1, 2, 3, 4,
                2, 3, 4, 1,
                3, 4, 1, 2,
                4, 1, 2, 3
            };

            boardHelper.FillBoardWithMatchableTypes(matchableTypes);
            var matches = boardHelper.FindAllPossibleMatches();

            Assert.AreEqual(0, matches.Count);
        }
    }
}
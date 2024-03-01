using System.Collections.Generic;
using UnityEngine;

public class BoardHelper
{
    private static BoardHelper _instance;
    public static BoardHelper Instance => _instance ?? new BoardHelper();

    private Board _board;

    public Board CreateBoard(int width, int height)
    {
        _board = new Board(width, height);
        return _board;
    }

    public void FillBoardWithMatchableTypes(int[] matchableTypes)
    {
        _board.FillBoard(matchableTypes);
    }

    public bool CheckMatchExists(BoardPosition first, BoardPosition second)
    {
        // TODO: Task 2
        IMatchChecker matchChecker = new OrthagonalMatchChecker(_board);
        return matchChecker.CheckMatchExists(first, second);
    }

    public IList<MatchData> FindAllPossibleMatches()
    {
        // TODO: Task 3
        IMatchFinder matchFinder = new OrthagonalMatchFinder(_board);
        return matchFinder.FindAllPossibleMatches();
    }

    public static IEnumerable<BoardItem> GetBoardTraverser(Board board, TraverseCorner corner)
    {
        return new BoardTraverser(board, corner);
    }
}
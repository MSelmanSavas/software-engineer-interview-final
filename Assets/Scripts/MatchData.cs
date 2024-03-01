using System.Collections.Generic;

public struct MatchData
{
    public BoardPosition First { get; }
    public BoardPosition Second { get; }
    public ISet<BoardItem> MatchedItems { get; }

    public MatchData(BoardPosition first, BoardPosition second, ISet<BoardItem> matchedItems)
    {
        First = first;
        Second = second;
        MatchedItems = matchedItems;
    }
}
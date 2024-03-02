using ArrayExtensions;
using UnityEngine;

public class Board
{
    private readonly BoardItem[] _boardItems;

    public int Width { get; }
    public int Height { get; }

    public Board(int width, int height)
    {
        _boardItems = new BoardItem[width * height];
        Width = width;
        Height = height;

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                _boardItems[x * width + y] = new BoardItem();
            }
        }
    }

    public void FillBoard(int[] matchableTypes)
    {
        var i = 0;
        foreach (var boardItem in _boardItems)
        {
            boardItem.MatchableType = matchableTypes[i++];
        }
    }

    public bool TryGetBoardItemByIndex(int xIndex, int yIndex, out BoardItem boardItem)
    {
        boardItem = null;

        if (xIndex < 0 || xIndex >= Width)
            return false;

        if (yIndex < 0 || yIndex >= Height)
            return false;

        boardItem = _boardItems[xIndex * Width + yIndex];
        return true;
    }

    public bool TryGetBoardItemByIndex(Vector2Int index, out BoardItem boardItem)
    {
        return TryGetBoardItemByIndex(index.x, index.y, out boardItem);
    }

    public bool TryGetBoardItemByIndex(BoardPosition position, out BoardItem boardItem)
    {
        return TryGetBoardItemByIndex(position.X, position.Y, out boardItem);
    }

    public bool IsInsideBoard(int x, int y)
    {
        if (x < 0 || x >= Width)
            return false;

        if (y < 0 || y >= Height)
            return false;

        return true;
    }

    public bool IsInsideBoard(Vector2Int index) => IsInsideBoard(index.x, index.y);
    public bool IsInsideBoard(BoardPosition position) => IsInsideBoard(position.X, position.Y);

    public string DebugBoard()
    {
        System.Text.StringBuilder stringBuilder = new(Width * Height);
        stringBuilder.AppendLine();
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                stringBuilder.Append(_boardItems.Get(new Vector2Int(x, y), Height).MatchableType + ",");
            }

            stringBuilder.AppendLine();
        }

        return stringBuilder.ToString();
    }
}
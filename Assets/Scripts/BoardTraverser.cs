using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTraverser : IEnumerable<BoardItem>
{
    public BoardItem Current { get; private set; }
    private Board _board;
    private TraverseCorner _traverseCorner;

    public BoardTraverser(Board board, TraverseCorner corner)
    {
        _board = board;
        _traverseCorner = corner;
    }

    Vector2Int GetStartCorner(Vector2Int boardSize, TraverseCorner corner)
    {
        return corner switch
        {
            TraverseCorner.BottomLeft => Vector2Int.zero,
            TraverseCorner.TopLeft => new Vector2Int(0, boardSize.y - 1),
            TraverseCorner.BottomRight => new Vector2Int(boardSize.x - 1, 0),
            TraverseCorner.TopRight => new Vector2Int(boardSize.x - 1, boardSize.y - 1),
            _ => Vector2Int.zero,
        };
    }

    Vector2Int GetIterationDirectionVector(TraverseCorner corner)
    {
        return corner switch
        {
            TraverseCorner.BottomLeft => new Vector2Int(1, 1),
            TraverseCorner.TopLeft => new Vector2Int(1, -1),
            TraverseCorner.BottomRight => new Vector2Int(-1, 1),
            TraverseCorner.TopRight => new Vector2Int(-1, -1),
            _ => Vector2Int.zero,
        };
    }

    public IEnumerator<BoardItem> GetEnumerator()
    {
        Vector2Int startIndex = GetStartCorner(new Vector2Int(_board.Width, _board.Height), _traverseCorner);
        Vector2Int iterationDirection = GetIterationDirectionVector(_traverseCorner);

        for (int xIndex = 0; xIndex < _board.Width; xIndex++)
        {
            for (int yIndex = 0; yIndex < _board.Height; yIndex++)
            {
                Vector2Int iterationIndex = new Vector2Int(iterationDirection.x * xIndex, iterationDirection.y * yIndex);
                Vector2Int offsetIndex = startIndex + iterationIndex;

                if (!_board.TryGetBoardItemByIndex(offsetIndex, out BoardItem boardItem))
                {
                    Debug.LogError($"Cannot get board item at : {offsetIndex}! Skipping to continue traversing...");
                    continue;
                }

                yield return boardItem;
            }
        }
        // TODO: Task 1
        yield break;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

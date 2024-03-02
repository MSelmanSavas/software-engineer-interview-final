using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

sealed class OrthagonalMatchFinder : IMatchFinder
{
    Board _board;

    List<Vector2Int> _orthagonalDirections = new()
    {
        Vector2Int.right,
        Vector2Int.left,
        Vector2Int.up,
        Vector2Int.down,
    };

    List<Vector2Int> _horizontalDirections = new()
    {
        Vector2Int.right,
        Vector2Int.left,
    };

    List<Vector2Int> _verticalDirections = new()
    {
        Vector2Int.up,
        Vector2Int.down,
    };

    int _minToMatchCount = 3;

    public OrthagonalMatchFinder(Board board)
    {
        _board = board;
    }

    public IList<MatchData> FindAllPossibleMatches()
    {
        List<MatchData> matchDatas = new();

        int width = _board.Width;
        int height = _board.Width;

        HashSet<(BoardPosition, BoardPosition)> alreadyCheckedPositionPairs = HashSetPool<(BoardPosition, BoardPosition)>.Get();

        for (int xIndex = 0; xIndex < width; xIndex++)
            for (int yIndex = 0; yIndex < height; yIndex++)
            {
                BoardPosition firstPosition = new BoardPosition(xIndex, yIndex);

                foreach (var direction in _orthagonalDirections)
                {
                    BoardPosition secondPosition = new BoardPosition(xIndex + direction.x, yIndex + direction.y);

                    if (!_board.IsInsideBoard(secondPosition))
                        continue;

                    if (alreadyCheckedPositionPairs.Contains((firstPosition, secondPosition)))
                        continue;

                    TryFindMatches(firstPosition, secondPosition, matchDatas);

                    alreadyCheckedPositionPairs.Add((firstPosition, secondPosition));
                    alreadyCheckedPositionPairs.Add((secondPosition, firstPosition));
                }
            }

        HashSetPool<(BoardPosition, BoardPosition)>.Release(alreadyCheckedPositionPairs);
        return matchDatas;
    }

    public bool TryFindMatches(BoardPosition first, BoardPosition second, List<MatchData> foundMatchDatas)
    {
        HashSet<BoardItem> foundMatchedBoardItems = HashSetPool<BoardItem>.Get();

        if (first.X == second.X && first.Y == second.Y)
            return false;

        if (!_board.TryGetBoardItemByIndex(first, out BoardItem firstBoardItem))
            return false;

        if (!_board.TryGetBoardItemByIndex(second, out BoardItem secondBoardItem))
            return false;

        Vector2Int firstBoardItemIndex = new Vector2Int(first.X, first.Y);
        Vector2Int secondBoardItemIndex = new Vector2Int(second.X, second.Y);

        #region First Item Match Check with horizontal directions

        foundMatchedBoardItems.Add(firstBoardItem);
        // First board item match checks for horizontal directions
        FindMatchesInDirections(firstBoardItem, secondBoardItemIndex, secondBoardItem, firstBoardItemIndex, _horizontalDirections, foundMatchedBoardItems);

        if (foundMatchedBoardItems.Count >= _minToMatchCount)
        {
            HashSet<BoardItem> foundItems = new HashSet<BoardItem>(foundMatchedBoardItems);
            foundMatchDatas.Add(new MatchData(first, second, foundItems));
            foundMatchDatas.Add(new MatchData(second, first, foundItems));
        }

        #endregion

        #region First Item Match Check with vertical directions

        foundMatchedBoardItems.Clear();

        foundMatchedBoardItems.Add(firstBoardItem);
        // First board item match checks for vertical directions
        FindMatchesInDirections(firstBoardItem, secondBoardItemIndex, secondBoardItem, firstBoardItemIndex, _verticalDirections, foundMatchedBoardItems);

        if (foundMatchedBoardItems.Count >= _minToMatchCount)
        {
            HashSet<BoardItem> foundItems = new HashSet<BoardItem>(foundMatchedBoardItems);
            foundMatchDatas.Add(new MatchData(first, second, foundItems));
            foundMatchDatas.Add(new MatchData(second, first, foundItems));
        }

        #endregion

        #region Second Item Match Check with horizontal directions

        foundMatchedBoardItems.Clear();

        foundMatchedBoardItems.Add(secondBoardItem);
        // Second board item match checks for horizontal directions
        FindMatchesInDirections(secondBoardItem, firstBoardItemIndex, firstBoardItem, secondBoardItemIndex, _horizontalDirections, foundMatchedBoardItems);

        if (foundMatchedBoardItems.Count >= _minToMatchCount)
        {
            HashSet<BoardItem> foundItems = new HashSet<BoardItem>(foundMatchedBoardItems);
            foundMatchDatas.Add(new MatchData(first, second, foundItems));
            foundMatchDatas.Add(new MatchData(second, first, foundItems));
        }

        #endregion

        #region Second Item Match Check with vertical directions

        foundMatchedBoardItems.Clear();

        foundMatchedBoardItems.Add(secondBoardItem);
        // Second board item match checks for vertical directions
        FindMatchesInDirections(secondBoardItem, firstBoardItemIndex, firstBoardItem, secondBoardItemIndex, _verticalDirections, foundMatchedBoardItems);

        if (foundMatchedBoardItems.Count >= _minToMatchCount)
        {
            HashSet<BoardItem> foundItems = new HashSet<BoardItem>(foundMatchedBoardItems);
            foundMatchDatas.Add(new MatchData(first, second, foundItems));
            foundMatchDatas.Add(new MatchData(second, first, foundItems));
        }

        #endregion

        HashSetPool<BoardItem>.Release(foundMatchedBoardItems);
        return foundMatchDatas.Count > 0;
    }

    /// <summary>
    /// Finds Matches by given parameters
    /// </summary>
    /// <param name="checkItem">Item that wants to be matched</param>
    /// <param name="checkFromIndex">Item index if that item was swapped</param>
    /// <param name="swappedItem">Item that theoratically swapped with check item</param>
    /// <param name="swappedItemIndex">Item index that theoratically swapped with check item</param>
    /// <param name="directionsToCheck">Directions to check matches</param>
    /// <param name="matchingBoardItems">Found matching board items</param>
    private void FindMatchesInDirections(
        BoardItem checkItem,
        Vector2Int checkFromIndex,
        BoardItem swappedItem,
        Vector2Int swappedItemIndex,
        List<Vector2Int> directionsToCheck,
        HashSet<BoardItem> matchingBoardItems)
    {
        foreach (var checkDirection in directionsToCheck)
        {
            Vector2Int offsetIndex = checkFromIndex + checkDirection;

            while (_board.IsInsideBoard(offsetIndex))
            {
                //Checking swappedItemIndex here because we don't want to change board
                //data just to check if two items can be swapped
                if (offsetIndex == swappedItemIndex)
                {
                    if (swappedItem.MatchableType != checkItem.MatchableType)
                        break;

                    if (!matchingBoardItems.Contains(swappedItem))
                        matchingBoardItems.Add(swappedItem);

                    offsetIndex += checkDirection;
                }
                else
                {
                    if (!_board.TryGetBoardItemByIndex(offsetIndex, out BoardItem foundBoardItem))
                        continue;

                    if (foundBoardItem.MatchableType != checkItem.MatchableType)
                        break;

                    if (!matchingBoardItems.Contains(foundBoardItem))
                        matchingBoardItems.Add(foundBoardItem);

                    offsetIndex += checkDirection;
                }
            }
        }
    }
}

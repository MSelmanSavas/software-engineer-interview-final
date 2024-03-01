using System.Collections.Generic;
using UnityEngine;

sealed class OrthagonalMatchChecker : IMatchChecker
{
    Board _board;

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

    public OrthagonalMatchChecker(Board board)
    {
        _board = board;
    }

    public bool CheckMatchExists(BoardPosition first, BoardPosition second)
    {
        if (first.X == second.X && first.Y == second.Y)
            return false;

        if (!_board.TryGetBoardItemByIndex(first, out BoardItem firstBoardItem))
            return false;

        if (!_board.TryGetBoardItemByIndex(second, out BoardItem secondBoardItem))
            return false;

        Vector2Int firstBoardItemIndex = new Vector2Int(first.X, first.Y);
        int firstBoardItemType = firstBoardItem.MatchableType;

        Vector2Int secondBoardItemIndex = new Vector2Int(second.X, second.Y);
        int secondBoardItemType = secondBoardItem.MatchableType;

        bool anyMatchFound = false;
        int matchCount = 1;

        // First board item match checks for horizontal directions
        CheckMatchesInDirections(firstBoardItemType, secondBoardItemIndex, secondBoardItemType, firstBoardItemIndex, ref anyMatchFound, ref matchCount, _horizontalDirections);

        if (anyMatchFound)
            return true;

        matchCount = 1;

        // First board item match checks for vertical directions
        CheckMatchesInDirections(firstBoardItemType, secondBoardItemIndex, secondBoardItemType, firstBoardItemIndex, ref anyMatchFound, ref matchCount, _verticalDirections);

        if (anyMatchFound)
            return true;

        matchCount = 1;

        // Second board item match checks for horizontal directions
        CheckMatchesInDirections(secondBoardItemType, firstBoardItemIndex, firstBoardItemType, secondBoardItemIndex, ref anyMatchFound, ref matchCount, _horizontalDirections);

        if (anyMatchFound)
            return true;

        matchCount = 1;

        // Second board item match checks for vertical directions
        CheckMatchesInDirections(secondBoardItemType, firstBoardItemIndex, firstBoardItemType, secondBoardItemIndex, ref anyMatchFound, ref matchCount, _verticalDirections);

        return anyMatchFound;
    }

    /// <summary>
    /// Checks matches by given parameters
    /// </summary>
    /// <param name="checkItemType">Item that wants to be matched</param>
    /// <param name="checkFromIndex">Item index if that item was swapped</param>
    /// <param name="swappedItemType">Item that theoratically swapped with check item</param>
    /// <param name="swappedItemIndex">Item index that theoratically swapped with check item</param>
    /// <param name="anyMatchFound"></param>
    /// <param name="matchCount"></param>
    /// <param name="directionsToCheck">Directions to check matches</param>
    private void CheckMatchesInDirections(
        int checkItemType,
        Vector2Int checkFromIndex,
        int swappedItemType,
        Vector2Int swappedItemIndex,
        ref bool anyMatchFound,
        ref int matchCount,
        List<Vector2Int> directionsToCheck)
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
                    if (swappedItemType != checkItemType)
                        break;

                    matchCount++;

                    if (matchCount >= _minToMatchCount)
                    {
                        anyMatchFound = true;
                        break;
                    }

                    offsetIndex += checkDirection;
                }
                else
                {
                    if (!_board.TryGetBoardItemByIndex(offsetIndex, out BoardItem foundBoardItem))
                        continue;

                    if (foundBoardItem.MatchableType != checkItemType)
                        break;

                    matchCount++;

                    if (matchCount >= _minToMatchCount)
                    {
                        anyMatchFound = true;
                        break;
                    }

                    offsetIndex += checkDirection;
                }
            }
        }
    }
}
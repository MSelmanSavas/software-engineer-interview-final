using System.Collections.Generic;
using System.Linq;
using ArrayExtensions;
using UnityEngine;
using UnityEngine.Pool;

public class RandomBoardCreator : IBoardCreator
{
    int _minMatchCount = 3;

    List<int> _itemTypes = new()
    {
        1,2,3,4,
    };

    List<ValidMatchPattern> _validMatchPatterns = new()
    {
        new ValidMatchPattern
        {
            MatchSize = new Vector2Int(2,3),
            matchPlacements = new int[]
            {
                1,0,0,
                0,1,1
            },
        },
        new ValidMatchPattern
        {
            MatchSize = new Vector2Int(2,3),
            matchPlacements = new int[]
            {
                0,0,1,
                1,1,0
            },
        },
        new ValidMatchPattern
        {
            MatchSize = new Vector2Int(2,3),
            matchPlacements = new int[]
            {
                0,1,1,
                1,0,0
            },
        },
        new ValidMatchPattern
        {
            MatchSize = new Vector2Int(2,3),
            matchPlacements = new int[]
            {
                1,1,0,
                0,0,1
            },
        },
        new ValidMatchPattern
        {
            MatchSize = new Vector2Int(3,2),
            matchPlacements = new int[]
            {
                1,0,
                1,0,
                0,1
            },
        },
        new ValidMatchPattern
        {
            MatchSize = new Vector2Int(3,2),
            matchPlacements = new int[]
            {
                0,1,
                1,0,
                1,0
            },
        },
        new ValidMatchPattern
        {
            MatchSize = new Vector2Int(3,2),
            matchPlacements = new int[]
            {
                1,0,
                0,1,
                0,1
            },
        },
        new ValidMatchPattern
        {
            MatchSize = new Vector2Int(3,2),
            matchPlacements = new int[]
            {
                0,1,
                0,1,
                1,0
            },
        },
    };



    public Board CreateBoard(int width, int height)
    {
        Board board = new Board(width, height);
        int[] items = new int[width * height];

        PlaceOneValidMatchOnBoard(width, height, ref items);
        PopulateBoardItemsWithRandomTypes(width, height, ref board, ref items);

        board.FillBoard(items);
        return board;
    }

    void PlaceOneValidMatchOnBoard(int width, int height, ref int[] items)
    {
        ValidMatchPattern matchPattern = _validMatchPatterns[UnityEngine.Random.Range(0, _validMatchPatterns.Count)];
        Vector2Int boardSize = new Vector2Int(width, height);
        Vector2Int placementAreaSize = boardSize - matchPattern.MatchSize;

        Vector2Int randomPlacementPivotPoint = new()
        {
            x = Random.Range(0, placementAreaSize.x),
            y = Random.Range(0, placementAreaSize.y),
        };

        int matchType = GetRandomItem();

        for (int x = 0; x < matchPattern.MatchSize.x; x++)
            for (int y = 0; y < matchPattern.MatchSize.y; y++)
            {
                Vector2Int matchPatternIndex = new(x, y);

                if (matchPattern.matchPlacements.Get(matchPatternIndex, matchPattern.MatchSize.y) <= 0)
                    continue;

                Vector2Int offsetIndex = matchPatternIndex + randomPlacementPivotPoint;

                items.Set(matchType, offsetIndex, width);
            }

        return;
    }

    void PopulateBoardItemsWithRandomTypes(int width, int height, ref Board board, ref int[] items)
    {
        List<int> _excludedItems = ListPool<int>.Get();

        for (int xIndex = 0; xIndex < width; xIndex++)
            for (int yIndex = 0; yIndex < height; yIndex++)
            {
                Vector2Int currentItemIndex = new Vector2Int(xIndex, yIndex);

                if (items.Get(currentItemIndex, height) > 0)
                    continue;

                _excludedItems.Clear();

                #region Checking Horizontal for Matching 

                int sameTypeOnHorizontalCount = 0;
                int horizontalCheckItemType = -1;

                Vector2Int leftSideCheckIndex = currentItemIndex;

                for (int i = 0; i < _minMatchCount - 1; i++)
                {
                    leftSideCheckIndex += Vector2Int.left;

                    if (!board.IsInsideBoard(leftSideCheckIndex))
                        break;

                    int itemType = items.Get(leftSideCheckIndex, height);

                    if (horizontalCheckItemType == -1)
                    {
                        horizontalCheckItemType = itemType;
                        sameTypeOnHorizontalCount++;
                        continue;
                    }

                    if (horizontalCheckItemType == itemType)
                    {
                        sameTypeOnHorizontalCount++;
                        continue;
                    }

                    break;
                }

                Vector2Int rightSideCheckIndex = currentItemIndex;

                for (int i = 0; i < _minMatchCount - 1; i++)
                {
                    rightSideCheckIndex += Vector2Int.right;

                    if (!board.IsInsideBoard(rightSideCheckIndex))
                        break;

                    int itemType = items.Get(rightSideCheckIndex, height);

                    if (horizontalCheckItemType == -1)
                    {
                        horizontalCheckItemType = itemType;
                        sameTypeOnHorizontalCount++;
                        continue;
                    }

                    if (horizontalCheckItemType == itemType)
                    {
                        sameTypeOnHorizontalCount++;
                        continue;
                    }

                    break;
                }

                if (sameTypeOnHorizontalCount >= _minMatchCount - 1)
                    if (!_excludedItems.Contains(horizontalCheckItemType))
                        _excludedItems.Add(horizontalCheckItemType);

                #endregion

                #region Checking Vertical for Matching 

                int sameTypeOnVerticalCount = 0;
                int verticalCheckItemType = -1;

                Vector2Int downSideCheckIndex = currentItemIndex;

                for (int i = 0; i < _minMatchCount - 1; i++)
                {
                    downSideCheckIndex += Vector2Int.down;

                    if (!board.IsInsideBoard(downSideCheckIndex))
                        break;

                    int itemType = items.Get(downSideCheckIndex, height);

                    if (verticalCheckItemType == -1)
                    {
                        verticalCheckItemType = itemType;
                        sameTypeOnVerticalCount++;
                        continue;
                    }

                    if (verticalCheckItemType == itemType)
                    {
                        sameTypeOnVerticalCount++;
                        continue;
                    }

                    break;
                }

                if (sameTypeOnVerticalCount >= _minMatchCount - 1)
                    _excludedItems.Add(verticalCheckItemType);

                Vector2Int upSideCheckIndex = currentItemIndex;

                for (int i = 0; i < _minMatchCount - 1; i++)
                {
                    upSideCheckIndex += Vector2Int.up;

                    if (!board.IsInsideBoard(upSideCheckIndex))
                        break;

                    int itemType = items.Get(upSideCheckIndex, height);

                    if (verticalCheckItemType == -1)
                    {
                        verticalCheckItemType = itemType;
                        sameTypeOnVerticalCount++;
                        continue;
                    }

                    if (verticalCheckItemType == itemType)
                    {
                        sameTypeOnVerticalCount++;
                        continue;
                    }

                    break;
                }

                if (sameTypeOnVerticalCount >= _minMatchCount - 1)
                    if (!_excludedItems.Contains(verticalCheckItemType))
                        _excludedItems.Add(verticalCheckItemType);

                #endregion


                if (!TryGetRandomItemWithExclusions(out int foundItem, _excludedItems))
                {
                    Debug.LogError("With given types, there is no unmatchable type found to create a board! Cannot continue creating board!");
                    return;
                }

                items.Set(foundItem, currentItemIndex, width);
            }

        ListPool<int>.Release(_excludedItems);
    }

    int GetRandomItem() => _itemTypes[UnityEngine.Random.Range(0, _itemTypes.Count)];

    bool TryGetRandomItemWithExclusions(out int foundItem, params int[] excludedItemTypes)
    {
        foundItem = GetRandomItem();

        if (!excludedItemTypes.Contains(foundItem))
            return true;

        int itemTypesCount = _itemTypes.Count;

        int indexOfFoundItem = _itemTypes.IndexOf(foundItem);

        for (int i = 0; i < itemTypesCount; i++)
        {
            indexOfFoundItem = (indexOfFoundItem + 1) % itemTypesCount;

            if (excludedItemTypes.Contains(_itemTypes[indexOfFoundItem]))
                continue;

            foundItem = _itemTypes[indexOfFoundItem];
            return true;
        }

        foundItem = -1;
        return false;
    }

    bool TryGetRandomItemWithExclusions(out int foundItem, List<int> excludedItemTypes)
    {
        foundItem = GetRandomItem();

        if (!excludedItemTypes.Contains(foundItem))
            return true;

        int itemTypesCount = _itemTypes.Count;

        int indexOfFoundItem = _itemTypes.IndexOf(foundItem);

        for (int i = 0; i < itemTypesCount; i++)
        {
            indexOfFoundItem = (indexOfFoundItem + 1) % itemTypesCount;

            if (excludedItemTypes.Contains(_itemTypes[indexOfFoundItem]))
                continue;

            foundItem = _itemTypes[indexOfFoundItem];
            return true;
        }

        foundItem = -1;
        return false;
    }
}
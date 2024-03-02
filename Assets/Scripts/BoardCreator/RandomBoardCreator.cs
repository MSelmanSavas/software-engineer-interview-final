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

    public Board CreateBoard(int width, int height)
    {
        Board board = new Board(width, height);
        int[] items = new int[width * height];

        List<int> _excludedItems = ListPool<int>.Get();

        for (int xIndex = 0; xIndex < width; xIndex++)
            for (int yIndex = 0; yIndex < height; yIndex++)
            {
                Vector2Int currentItemIndex = new Vector2Int(xIndex, yIndex);

                _excludedItems.Clear();

                int sameTypeOnLeftSideCount = 0;
                int leftSideCheckItemType = -1;

                Vector2Int leftSideCheckIndex = currentItemIndex;

                for (int i = 0; i < _minMatchCount - 1; i++)
                {
                    leftSideCheckIndex += Vector2Int.left;

                    if (!board.IsInsideBoard(leftSideCheckIndex))
                        break;

                    int itemType = items.Get(leftSideCheckIndex, width);

                    if (leftSideCheckItemType == -1)
                    {
                        leftSideCheckItemType = itemType;
                        sameTypeOnLeftSideCount++;
                        continue;
                    }

                    if (leftSideCheckItemType == itemType)
                    {
                        sameTypeOnLeftSideCount++;
                        continue;
                    }

                    break;
                }

                if (sameTypeOnLeftSideCount >= _minMatchCount - 1)
                    _excludedItems.Add(leftSideCheckItemType);

                int sameTypeOnDownSideCount = 0;
                int downSideCheckItemType = -1;

                Vector2Int downSideCheckIndex = currentItemIndex;

                for (int i = 0; i < _minMatchCount - 1; i++)
                {
                    downSideCheckIndex += Vector2Int.down;

                    if (!board.IsInsideBoard(downSideCheckIndex))
                        break;

                    int itemType = items.Get(downSideCheckIndex, width);

                    if (downSideCheckItemType == -1)
                    {
                        downSideCheckItemType = itemType;
                        sameTypeOnDownSideCount++;
                        continue;
                    }

                    if (downSideCheckItemType == itemType)
                    {
                        sameTypeOnDownSideCount++;
                        continue;
                    }

                    break;
                }

                if (sameTypeOnDownSideCount >= _minMatchCount - 1)
                    _excludedItems.Add(downSideCheckItemType);

                if (!TryGetRandomItemWithExclusions(out int foundItem, _excludedItems))
                {
                    Debug.LogError("With given types, there is no unmatchable type found to create a board! Cannot continue creating board!");
                    return board;
                }

                items.Set(foundItem, currentItemIndex, width);
            }

        ListPool<int>.Release(_excludedItems);

        board.FillBoard(items);
        return board;
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
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

    List<Vector2Int> _leftSideCheckOffsets = new()
    {
        Vector2Int.left,
        Vector2Int.left + Vector2Int.left,
    };

    List<Vector2Int> _downSideCheckOffsets = new()
    {
        Vector2Int.down,
        Vector2Int.down + Vector2Int.down,
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

                bool leftSideHasPotentialMatch = false;
                foreach (var leftSideOffset in _leftSideCheckOffsets)
                {
                    Vector2Int leftSideCheckIndex = currentItemIndex + leftSideOffset;

                    if (!board.IsInsideBoard(leftSideCheckIndex))
                        break;

                    int itemType = items.Get(leftSideCheckIndex, width);


                }
            }

        ListPool<int>.Release(_excludedItems);
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

            if (excludedItemTypes.Contains(foundItem))
                continue;

            foundItem = _itemTypes[indexOfFoundItem];
            return true;
        }

        foundItem = -1;
        return false;
    }
}
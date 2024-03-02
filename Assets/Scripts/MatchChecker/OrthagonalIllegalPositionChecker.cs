using System.Collections.Generic;
using ArrayExtensions;
using UnityEngine;

public class OrthagonalIllegalPositionChecker : IIllegalPositionChecker
{
    List<ValidMatchPattern> _matchPatternsToCheck = new()
    {
        //Vertical Check Pattern
        new ValidMatchPattern()
        {
            MatchSize = new Vector2Int(1, 3),
            matchPlacements = new int[]
            {
                1,1,1,
            }
        },

        //Horizontal Check Pattern
        new ValidMatchPattern
        {
            MatchSize = new Vector2Int(3, 1),
            matchPlacements = new int[]
            {
                1,
                1,
                1,
            }
        },
    };

    public bool CheckAnyIllegalPosition(Board board)
    {
        foreach (var matchPattern in _matchPatternsToCheck)
        {
            Vector2Int boardSize = new Vector2Int(board.Width, board.Height);
            Vector2Int matchPatternSize = matchPattern.MatchSize;
            Vector2Int iterationAreaSize = boardSize - matchPatternSize;

            for (int x = 0; x <= iterationAreaSize.x; x++)
            {
                for (int y = 0; y <= iterationAreaSize.y; y++)
                {
                    Vector2Int checkPivotPoint = new Vector2Int(x, y);
                    BoardItem boardItemToCheck = null;
                    bool allPatternIndexesAreSame = true;

                    for (int matchPatternX = 0; matchPatternX < matchPatternSize.x; matchPatternX++)
                    {
                        for (int matchPatternY = 0; matchPatternY < matchPatternSize.y; matchPatternY++)
                        {
                            Vector2Int matchPatternIndex = new Vector2Int(matchPatternX, matchPatternY);

                            if (matchPattern.matchPlacements.Get(matchPatternIndex, matchPatternSize.y) <= 0)
                                continue;

                            Vector2Int checkIndex = checkPivotPoint + matchPatternIndex;

                            if (!board.TryGetBoardItemByIndex(checkIndex, out BoardItem foundBoardItem))
                                continue;

                            if (boardItemToCheck == null)
                            {
                                boardItemToCheck = foundBoardItem;
                                continue;
                            }

                            if (boardItemToCheck.MatchableType == foundBoardItem.MatchableType)
                                continue;

                            allPatternIndexesAreSame = false;
                            break;
                        }
                    }

                    if (allPatternIndexesAreSame)
                        return true;
                }
            }
        }
        return false;
    }
}
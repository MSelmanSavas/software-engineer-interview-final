# Software Engineer Interview

You are going to implement basic match 3 mechanics for a game.
Our game includes basic `Board` implementation which creates a 
game board using a `width` and a `height`. Our `Board` consists of
`BoardItem`'s which has a `MatchableType` which indicates the different
kind of `BoardItem`'s in a given `Board`.

When 3 or more `BoardItem`'s with same `MatchableType` is side by side
vertically or horizontally we can say the board includes a match.

For example, for the given board:

```
1 | 2 | 3
1 | 3 | 2
1 | 2 | 3
```
  
It has one match with the `BoardItem`'s which have `MatchableType` 1.

Also when we create the `Board` we start its indexing from the `BottomLeft`
corner of the `Board` meaning `(x, y) = (0, 0)` is the `BottomLeft` corner 
of the `Board` and `(x, y) = (width - 1, height - 1)` is the `TopRight` corner
of the `Board`.

```
  TopLeft             TopRight
  (0, h-1)            (w-1, h-1)
  1 | 2 | 3 | 4 | 5 | 6 | 7
  7 | 1 | 2 | 3 | 4 | 5 | 6
  6 | 7 | 1 | 2 | 3 | 4 | 5 
  5 | 6 | 7 | 1 | 2 | 3 | 4
  4 | 5 | 6 | 7 | 1 | 2 | 3
  3 | 4 | 5 | 6 | 7 | 1 | 2
  2 | 3 | 4 | 5 | 6 | 7 | 1
  BottomLeft          (BottomRight)
  (0, 0)              (w-1, 0)
```

## Task 1: Create a `BoardTraverser`

We want to traverse our created `Board` from different corners of it so,
you should create an `IEnumerator<BoardItem>` implementation for a `Board`
starting from different corners. When you are traversing the `Board` you 
should traverse it vertically and then horizontally whatever the corner is.

e.g. for the given board:
```
  1 | 2 | 3
  3 | 1 | 2
  2 | 3 | 1
```

When we traverse it from `TopLeft` corner, we should traverse it with sequence
`[1, 3, 2, 2, 1, 3, 3, 2, 1]` or when we traverse it from `BottomRight` corner,
we should traverse it with sequence `[1, 2, 3, 3, 1, 2, 2, 3, 1]`.

## Task 2: Checking a Given Swap Creates a Match

Our game needs a way to check swapping two given `BoardPosition`'s creates a
match or not. Please implement the given functionality in class `BoardHelper`.

The function signature is:
```c#
public bool CheckMatchExists(BoardPosition first, BoardPosition second);
```

## Task 3: Finding All Possible Matches

Our game needs a way to check and find all the possible swaps creating a
match and its data as a struct.

```c#
public struct MatchData
{
    public BoardPosition first;
    public BoardPosition second;
    public ISet<BoardItem> matchedItems;
}
```

When one swap is creating a match, it means its alternative is also creating
a match and should be counted as a possible match. 

e.g.
When `first = BoardPosition(0, 1)` and `second = BoardPosition(1, 1)` creates
a match,`first = BoardPosition(1, 1)` and `second = BoardPosition(0, 1)` is
also need to be counted as a possible match.

Please implement the given functionality in class `BoardHelper`.

The function signature is:
```c# 
public IList<MatchData> FindAllPossibleMatches();
```

## Bonus Task: Creating a Randomized Board

Implement a function to create a randomized `Board` which needs to have at least
1 possible swap and no formed matches.

## Additional Information

We tested our code using Unity version `2021.3.10f1` but any other version 
should also be ok to use.

We included some tests for you to test your implementation but you are
encouraged to create some other tests too. You can run the tests using
`Window > General > Test Runner` window on Unity Editor application.
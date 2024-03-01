using System.Collections.Generic;

interface IMatchFinder
{
    public bool TryFindMatches(BoardPosition first, BoardPosition second, List<MatchData> foundMatchData);
    public IList<MatchData> FindAllPossibleMatches();
}
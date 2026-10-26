using System;
using System.Collections.Generic;
using System.Linq;

namespace VotePreferentiel;

public class VoteCounter : IVoteCounter
{
    public HashSet<string> GetCandidates(VoteModel voteModel) =>
    new(voteModel.Votes.SelectMany(v => v.Choices));
    public string GetWinner(VoteModel voteModel) => throw new NotImplementedException();
    public List<string> GetAllFirstChoices(VoteModel voteModel) =>
    voteModel.Votes.Select(v => v.Choices.First()).ToList();
    public IDictionary<string, int> CountFirstChoices(List<string> firstChoices) =>
    firstChoices.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
    public string[] GetCandidatesWithLeastVotes(IDictionary<string, int> firstChoiceCounts)
    {
        if (firstChoiceCounts.Count == 0) return [];

        var min = firstChoiceCounts.Values.Min();
        return firstChoiceCounts.Where(kv => kv.Value == min).Select(kv => kv.Key).ToArray();
    }
}
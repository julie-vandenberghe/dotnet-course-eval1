using System;
using System.Collections.Generic;

namespace VotePreferentiel;

public class VoteCounter : IVoteCounter
{
    public HashSet<string> GetCandidates(VoteModel voteModel) => throw new NotImplementedException();
    public string GetWinner(VoteModel voteModel) => throw new NotImplementedException();
    public List<string> GetAllFirstChoices(VoteModel voteModel) => throw new NotImplementedException();
    public IDictionary<string, int> CountFirstChoices(List<string> firstChoices) => throw new NotImplementedException();
    public string[] GetCandidatesWithLeastVotes(IDictionary<string, int> firstChoiceCounts) => throw new NotImplementedException();
}
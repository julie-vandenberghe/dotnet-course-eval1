using System;
using System.Collections.Generic;
using System.Linq;

namespace VotePreferentiel;

public class VoteCounter : IVoteCounter
{
    public HashSet<string> GetCandidates(VoteModel voteModel) =>
    new(voteModel.Votes.SelectMany(v => v.Choices));
    public string GetWinner(VoteModel voteModel)
    {
        if (voteModel.Votes.Count == 0 || voteModel.Votes.Any(v => v.Choices.Count == 0))
            throw new InvalidOperationException("Aucun vote, ou bulletin vide.");

        var remaining = GetCandidates(voteModel);
        var current = voteModel;

        while (remaining.Count > 1)
        {
            var counts = CountFirstChoices(GetAllFirstChoices(current));

            // un candidat en lice sans aucun premier choix doit apparaître avec 0 voix
            foreach (var candidate in remaining)
                counts.TryAdd(candidate, 0);

            var eliminated = GetCandidatesWithLeastVotes(counts);

            if (eliminated.Length == remaining.Count)
                throw new InvalidOperationException("Égalité parfaite : pas de gagnant.");

            remaining.ExceptWith(eliminated);

            // on retire les éliminés de chaque bulletin : le choix suivant devient le premier
            current = new VoteModel
            {
                Votes = current.Votes
                    .Select(v => new Ballot { Choices = v.Choices.Where(remaining.Contains).ToList() })
                    .ToList()
            };
        }

        return remaining.Single();
    }
    public List<string> GetAllFirstChoices(VoteModel voteModel) =>
    voteModel.Votes
        .Where(v => v.Choices.Count > 0)
        .Select(v => v.Choices[0])
        .ToList();
    public IDictionary<string, int> CountFirstChoices(List<string> firstChoices) =>
    firstChoices.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
    public string[] GetCandidatesWithLeastVotes(IDictionary<string, int> firstChoiceCounts)
    {
        if (firstChoiceCounts.Count == 0) return [];

        var min = firstChoiceCounts.Values.Min();
        return firstChoiceCounts.Where(kv => kv.Value == min).Select(kv => kv.Key).ToArray();
    }
}
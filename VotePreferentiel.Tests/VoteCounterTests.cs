using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotePreferentiel.Tests;

public class VoteCounterTests
{
    private readonly VoteCounter _counter = new();

    // n bulletins identiques
    private static IEnumerable<Ballot> Ballots(int n, params string[] choices) =>
        Enumerable.Range(0, n).Select(_ => new Ballot { Choices = choices.ToList() });

    private static VoteModel Model(params IEnumerable<Ballot>[] groups) =>
        new() { Votes = groups.SelectMany(g => g).ToList() };

    [Test]
    public async Task GetWinner_NominalCase_ReturnsChloe()
    {
        var model = Model(
            Ballots(1, "Alice", "Bilal", "Chloé"),
            Ballots(1, "Alice", "Chloé", "Bilal"),
            Ballots(1, "Bilal", "Chloé", "Alice"),
            Ballots(1, "Chloé", "Bilal", "Alice"),
            Ballots(1, "Chloé", "Alice", "Bilal"));

        await Assert.That(_counter.GetWinner(model)).IsEqualTo("Chloé");
    }

    [Test]
    public async Task GetWinner_SingleCandidate_ReturnsAlice()
    {
        var model = Model(Ballots(1, "Alice"));

        await Assert.That(_counter.GetWinner(model)).IsEqualTo("Alice");
    }

    [Test]
    public async Task GetWinner_TransfersVotes_ReturnsChloe()
    {
        var model = Model(
            Ballots(4, "Alice", "Bilal", "Chloé", "Diane"),
            Ballots(3, "Bilal", "Chloé", "Alice", "Diane"),
            Ballots(3, "Chloé", "Alice", "Bilal", "Diane"),
            Ballots(1, "Diane", "Chloé", "Bilal", "Alice"));

        await Assert.That(_counter.GetWinner(model)).IsEqualTo("Chloé");
    }

    [Test]
    public async Task GetWinner_TieForLast_EliminatesBothAndReturnsChloe()
    {
        var model = Model(
            Ballots(1, "Alice", "Chloé", "Bilal"),
            Ballots(1, "Bilal", "Chloé", "Alice"),
            Ballots(1, "Chloé", "Alice", "Bilal"),
            Ballots(1, "Chloé", "Bilal", "Alice"));

        await Assert.That(_counter.GetWinner(model)).IsEqualTo("Chloé");
    }

    [Test]
    public async Task GetWinner_EmptyBallot_Throws()
    {
        var model = Model(Ballots(1));   // un bulletin sans aucun choix

        await Assert.That(() => _counter.GetWinner(model)).Throws<InvalidOperationException>();
    }
}
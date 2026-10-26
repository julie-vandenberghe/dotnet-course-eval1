using System.Collections.Generic;

namespace VotePreferentiel;

public class VoteModel
{
    public List<Ballot> Votes { get; set; } = new();
}

public class Ballot
{
    public List<string> Choices { get; set; } = new();
}
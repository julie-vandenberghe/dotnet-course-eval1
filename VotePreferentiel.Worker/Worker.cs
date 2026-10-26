using VotePreferentiel;

namespace VotePreferentiel.Worker;

public class Worker : BackgroundService
{
    private readonly VoteProvider _provider;
    private readonly IVoteCounter _voteCounter;
    private readonly IConfiguration _configuration;
    private readonly IHostApplicationLifetime _lifetime;

    public Worker(VoteProvider provider, IVoteCounter voteCounter, IConfiguration configuration, IHostApplicationLifetime lifetime)
    {
        _provider = provider;
        _voteCounter = voteCounter;
        _configuration = configuration;
        _lifetime = lifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var url = _configuration["VoteSettings:Url"]
                  ?? throw new InvalidOperationException("VoteSettings:Url manquant dans appsettings.json");

        var votes = await _provider.GetVotesAsync(url, stoppingToken);
        var winner = _voteCounter.GetWinner(votes);

        Console.WriteLine($"Le gagnant est : {winner}");
        _lifetime.StopApplication();
    }
}
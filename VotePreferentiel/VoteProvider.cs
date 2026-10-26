using System;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace VotePreferentiel;

public class VoteProvider
{
    private readonly HttpClient _httpClient;

    public VoteProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<VoteModel> GetVotesAsync(string url, CancellationToken ct = default)
    {
        var model = await _httpClient.GetFromJsonAsync<VoteModel>(url, ct);
        return model ?? throw new InvalidOperationException("Fichier de votes vide ou invalide.");
    }
}
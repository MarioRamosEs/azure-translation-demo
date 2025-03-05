
using AzureTranslation.API.Abstractions;
using AzureTranslation.Commons.Models;

namespace AzureTranslation.API.Internals;

internal sealed class TranslationApiServices : ITranslationApiService
{
    /// <inheritdoc />
    public async Task<string> CreateTranslationRequestAsync(string originalText, CancellationToken cancellationToken)
    {
        var requestId = Guid.NewGuid().ToString("N");

        // TODO: Implement the logic to create a translation request

        return requestId;

    }

    ///// <inheritdoc />
    //public Task<TranslationStatus> GetTranslationStatusAsync(string requestId, CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}
}

namespace AzureTranslation.Infrastructure.Options;

public sealed class TranslatorOptions // TODO implement this and LanguageOptions
{
    public string Region { get; init; }

    public string Key { get; init; }

    public string Endpoint { get; init; }
}

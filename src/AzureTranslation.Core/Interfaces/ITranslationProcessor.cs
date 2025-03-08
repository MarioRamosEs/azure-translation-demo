using AzureTranslation.Common.Models;

namespace AzureTranslation.Core.Interfaces;

/// <summary>
/// Servicio para procesar traducciones.
/// </summary>
public interface ITranslationProcessor // TODO in english
{
    /// <summary>
    /// Procesa una solicitud de traducción usando servicios de detección de idioma y traducción.
    /// </summary>
    /// <param name="translationId">ID de la traducción a procesar</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task ProcessTranslationAsync(string translationId, CancellationToken cancellationToken);
}

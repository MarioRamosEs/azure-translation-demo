using AzureTranslation.Common.Models;

namespace AzureTranslation.Core.Interfaces;

/// <summary>
/// Servicio para gestionar solicitudes de traducción desde la API
/// </summary>
public interface ITranslationRequestService // TODO ingles
{
    /// <summary>
    /// Crea una solicitud de traducción y la encola para procesamiento asíncrono
    /// </summary>
    /// <param name="text">Texto original para traducir</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>La entidad de traducción creada</returns>
    Task<Translation> CreateTranslationRequestAsync(string text, CancellationToken cancellationToken);

    /// <summary>
    /// Recupera una traducción existente por su ID
    /// </summary>
    /// <param name="translationId">ID de la traducción</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>La entidad de traducción o null si no existe</returns>
    Task<Translation?> GetTranslationAsync(string translationId, CancellationToken cancellationToken);
}

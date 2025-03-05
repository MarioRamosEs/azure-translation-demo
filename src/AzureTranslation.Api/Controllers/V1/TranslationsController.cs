using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

using AzureTranslation.API.Controllers.V1.Models;

using Microsoft.AspNetCore.Mvc;

namespace AzureTranslation.Api.Controllers.V1;

/// <summary>
/// Provides endpoints to manage translations.
/// </summary>
[ApiController]
[Route(@"api/v{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class TranslationsController : ControllerBase
{
    private readonly ITranslationApiService translationApiService;
    private readonly ILogger<TranslationsController> logger;

    public TranslationsController(ITranslationApiService translationApiService, ILogger<TranslationsController> logger)
    {
        this.translationApiService = translationApiService;
        this.logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(NewTranslationResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTranslationRequest([Required] NewTranslationRequest request)
    {
        try
        {
            logger.LogInformation("Creating new translation request");

            string requestId = await _translationOrchestrator.CreateTranslationRequestAsync(request.OriginalText);

            logger.LogInformation("Created translation request with ID: {RequestId}", requestId);

            return Accepted(new NewTranslationResponse { RequestId = requestId });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating translation request");
            return StatusCode(500, "Ocurrió un error al procesar la solicitud de traducción");
        }
    }

    [HttpGet("{requestId:string}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TranslationStatusResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTranslationStatus([Required] string requestId)
    {
        try
        {
            logger.LogInformation("Fetching translation status for request ID: {RequestId}", requestId);

            var translationStatus = await _translationOrchestrator.GetTranslationStatusAsync(requestId);

            if (translationStatus == null)
            {
                logger.LogWarning("Translation request not found: {RequestId}", requestId);
                return NotFound($"No se encontró una solicitud de traducción con ID {requestId}");
            }

            return Ok(translationStatus);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching translation status for request ID: {RequestId}", requestId);
            return StatusCode(500, "Ocurrió un error al obtener el estado de la traducción");
        }
    }
}

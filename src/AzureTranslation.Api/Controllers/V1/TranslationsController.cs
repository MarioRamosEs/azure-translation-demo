using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mime;

using AzureTranslation.API.Controllers.V1.Models;
using AzureTranslation.Common.Models;
using AzureTranslation.Core.Interfaces;

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
    private readonly ITranslationService translationService;
    private readonly ILogger<TranslationsController> logger;

    public TranslationsController(ITranslationService translationService, ILogger<TranslationsController> logger)
    {
        this.translationService = translationService;
        this.logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(NewTranslationResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTranslationRequest([Required] NewTranslationRequest request, CancellationToken cancellationToken)
    {
        var translationId = await translationService.CreateTranslationRequestAsync(request.OriginalText, cancellationToken);

        return AcceptedAtAction(
            actionName: nameof(GetTranslation),
            controllerName: "Translations",
            routeValues: new { translationId },
            value: new NewTranslationResponse
            {
                TranslationId = translationId,
            });

        // TODO put a try catch block here
    }

    [HttpGet("{translationId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TranslationDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTranslation([FromRoute] string translationId, CancellationToken cancellationToken)
    {
        try
        {
            var translation = await translationService.GetTranslationAsync(translationId, cancellationToken);

            if (translation == null)
            {
                return NotFound($"Translation with ID '{translationId}' not found.");
            }

            return Ok(translation);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving translation with ID {TranslationId}", translationId);
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving the translation.");
        }
    }
}

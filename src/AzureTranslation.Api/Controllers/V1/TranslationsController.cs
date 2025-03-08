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
    private readonly ITranslationRequestService translationRequestService;
    private readonly ILogger<TranslationsController> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TranslationsController"/> class.
    /// </summary>
    /// <param name="translationService">The service responsible for translation operations.</param>
    /// <param name="logger">The logger used for logging controller operations.</param>
    /// <exception cref="ArgumentNullException">Thrown when either <paramref name="translationService"/> or <paramref name="logger"/> is null.</exception>
    public TranslationsController(ITranslationRequestService translationService, ILogger<TranslationsController> logger)
    {
        this.translationRequestService = translationService;
        this.logger = logger;
    }

    /// <summary>
    /// Creates a new translation request for the provided text.
    /// </summary>
    /// <param name="request">The request containing the text to translate.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the translation ID and a reference to check the translation status.
    /// </returns>
    /// <response code="202">Returns the translation ID along with information on how to track the translation.</response>
    /// <response code="400">If the request is invalid or fails validation.</response>
    /// <response code="500">If an unexpected error occurs during processing.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(Translation))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateTranslationRequest([Required] NewTranslationRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var translation = await translationRequestService.CreateTranslationRequestAsync(request.OriginalText, cancellationToken);

            return AcceptedAtAction(
                actionName: nameof(GetTranslation),
                controllerName: "Translations",
                routeValues: new { translationId = translation.Id },
                value: translation);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating translation request");
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing the translation request.");
        }
    }

    /// <summary>
    /// Retrieves a translation by its unique identifier.
    /// </summary>
    /// <param name="translationId">The unique identifier of the translation to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the translation details if found.
    /// </returns>
    /// <response code="200">Returns the translation details.</response>
    /// <response code="404">If the translation with the specified ID is not found.</response>
    /// <response code="500">If an unexpected error occurs during retrieval.</response>
    [HttpGet("{translationId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Translation))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTranslation([FromRoute] string translationId, CancellationToken cancellationToken)
    {
        try
        {
            var translation = await translationRequestService.GetTranslationAsync(translationId, cancellationToken);

            if (translation == null)
            {
                logger.LogWarning("Translation with ID {TranslationId} not found", translationId);
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

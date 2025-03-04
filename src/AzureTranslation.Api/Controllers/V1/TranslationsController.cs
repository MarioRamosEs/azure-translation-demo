using System.Net.Mime;

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
    /// <summary>
    /// Initializes a new instance of the <see cref="TranslationsController"/> class.
    /// </summary>
    public TranslationsController()
    {
    }

    [HttpGet]
    public async Task<IActionResult> Test(CancellationToken cancellationToken)
    {
        return Ok("Hello World");
    }
}

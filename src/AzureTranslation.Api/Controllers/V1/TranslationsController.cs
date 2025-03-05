using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

using Azure.Data.Tables;
using Azure.Messaging.ServiceBus;

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
    private readonly ServiceBusClient serviceBusClient;
    private readonly TableClient tableClient;
    private readonly ILogger<TranslationsController> logger;

    private const string PartitionKeyValue = "Translations";

    public TranslationsController(ServiceBusClient serviceBusClient, TableServiceClient tableServiceClient, ILogger<TranslationsController> logger)
    {
        this.serviceBusClient = serviceBusClient;
        this.tableClient = tableServiceClient.GetTableClient("Translations");
        this.logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(NewTranslationResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTranslationRequest([Required] NewTranslationRequest request, CancellationToken cancellationToken)
    {
        var translationId = Guid.NewGuid().ToString("N");

        // Add to Table Storage
        var entity = new TranslationEntity
        {
            PartitionKey = PartitionKeyValue,
            RowKey = translationId,
            OriginalText = request.OriginalText,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow,
        };

        await tableClient.AddEntityAsync(entity);

        // Add to Service Bus
        await using var sender = serviceBusClient.CreateSender("sbq-translation-requests-mr");

        var message = new ServiceBusMessage()
        {
            MessageId = translationId,
            CorrelationId = translationId,
        };

        await sender.SendMessageAsync(message, cancellationToken);

        return AcceptedAtAction(
            actionName: nameof(GetTranslationStatus),
            controllerName: "Translations",
            routeValues: new { id = translationId },
            value: new NewTranslationResponse
            {
                TranslationId = translationId,
            });
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TranslationStatusResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTranslationStatus([Required] string id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

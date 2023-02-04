﻿using ContentProcessor.Worker.Domain;
using ContentProcessor.Worker.Domain.ContentProviders;
using ContentProcessor.Worker.Domain.ContentProviders.TikTok;
using ContentProcessor.Worker.Services.ContentProviders.TikTok.TikTokContentHandlers;
using ContentProcessor.Worker.Services.ContentProviders.TikTok.TikTokContentHandlers.Abstractions;
using GusMelfordBot.Extensions;
using GusMelfordBot.Extensions.Services.DataLake;
using GusMelfordBot.SimpleKafka.Interfaces;

namespace ContentProcessor.Worker.Services.ContentProviders.TikTok;

public class TikTokService : ITikTokService
{
    private readonly IKafkaProducer<string> _kafkaProducer;
    private readonly IDataLakeService _dataLakeService;
    private readonly ILogger<ITikTokService> _logger;

    public TikTokService(
        IKafkaProducer<string> kafkaProducer, 
        IDataLakeService dataLakeService, 
        ILogger<ITikTokService> logger)
    {
        _kafkaProducer = kafkaProducer;
        _dataLakeService = dataLakeService;
        _logger = logger;
    }

    public async Task ProcessAsync(ProcessTikTokContent? processedContent)
    {
        if (processedContent is null) {
            return;
        }

        AbstractTikTokContentHandler handler = new RefererLinkHandler();
        handler
            .SetNext(new VideoInformationHandler())
            .SetNext(new DownloadLinkHandler())
            .SetNext(new SaveHandler(_dataLakeService, _logger));
        
        processedContent = (await handler.HandleAsync(processedContent)).IfNullThrow();

        if (processedContent is { IsSaved: false, Attempt: <= 5 }) {
            await _kafkaProducer.ProduceAsync(processedContent.ToAttemptContentEvent());
            return;
        }

        if (processedContent.IsAvailable) {
            await _kafkaProducer.ProduceAsync(processedContent.ToContentProcessedEvent());
        }
    }
}
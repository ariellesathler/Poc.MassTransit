using Azure.Storage.Blobs;
using MassTransit;
using MassTransit.MongoDbIntegration.MessageData;

namespace Poc.MassTransit.Common.Config
{
    public static class MessageDataRepositoryFactory
    {
        public static IMessageDataRepository GetRepository(AppConfig config)
            => config.PocKind!.MessageDataRepositoryType switch
            {
                "MongoDB" => new MongoDbMessageDataRepository(config.Mongo!.ConnectionString, config.Mongo.DatabaseName),
                "BlobStorage" => new BlobServiceClient(config.BlobStorage!.ConnectionString).CreateMessageDataRepository(config.BlobStorage!.ContainerName),
                _ => throw new NotSupportedException($"Poc kind not supported! - {config.PocKind!.MessageDataRepositoryType}"),
            };

    }
}
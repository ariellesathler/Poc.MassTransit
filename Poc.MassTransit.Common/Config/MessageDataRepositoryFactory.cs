﻿using Amazon;
using Amazon.S3;
using Azure.Storage.Blobs;
using MassTransit;
using MassTransit.MongoDbIntegration.MessageData;
using Microsoft.Extensions.DependencyInjection;

namespace Poc.MassTransit.Common.Config
{
    public static class MessageDataRepositoryFactory
    {
        public static IMessageDataRepository GetRepository(AppConfig config, IServiceProvider provider)
        =>
            config.PocKind!.MessageDataRepositoryType switch
            {
                "MongoDB" => new MongoDbMessageDataRepository(config.Mongo!.ConnectionString, config.Mongo.DatabaseName),
                "BlobStorage" => new BlobServiceClient(config.BlobStorage!.ConnectionString).CreateMessageDataRepository(config.BlobStorage!.ContainerName),
                "S3" => new S3MessageDataRepository(provider.GetRequiredService<IAmazonS3>(), config.AWS!.BucketName),
                _ => throw new NotSupportedException($"Poc kind not supported! - {config.PocKind!.MessageDataRepositoryType}"),
            };


        public static IMessageDataRepository GetRepository(AppConfig config)
       =>
           config.PocKind!.MessageDataRepositoryType switch
           {
               "MongoDB" => new MongoDbMessageDataRepository(config.Mongo!.ConnectionString, config.Mongo.DatabaseName),
               "BlobStorage" => new BlobServiceClient(config.BlobStorage!.ConnectionString).CreateMessageDataRepository(config.BlobStorage!.ContainerName),
               "S3" => S3Repository(config),//Utilizado em Functions, onde não é possível obter o Provider em tempo de configuração
               _ => throw new NotSupportedException($"Poc kind not supported! - {config.PocKind!.MessageDataRepositoryType}"),
           };

        private static IMessageDataRepository S3Repository(AppConfig config)
        {
            var s3Config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.USEast1,
                ServiceURL = config.AWS!.ServiceURL
            };

            var awsCredentials = new AwsCredentialsKeys(config);

            var amazonClient = new AmazonS3Client(awsCredentials, s3Config);

            return new S3MessageDataRepository(amazonClient, config.AWS!.BucketName);
        }
    }
}
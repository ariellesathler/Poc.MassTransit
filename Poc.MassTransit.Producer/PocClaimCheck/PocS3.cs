using Amazon.S3;
using Azure;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3.Model;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text.Json;
using Poc.MassTransit.Common;
using MassTransit.MessageData.Values;
using Microsoft.Azure.Amqp.Framing;
using System.Reflection.Metadata;
using static System.Reflection.Metadata.BlobBuilder;
using Amazon.S3.Util;
using Amazon.Runtime.Internal.Util;
using MassTransit.Caching.Internals;

namespace Poc.MassTransit.Producer.PocClaimCheck
{
    public class PocS3 : BackgroundService
    {
        private readonly IAmazonS3 amazonS3;

        public PocS3(IAmazonS3 amazonS3)
        {
            this.amazonS3 = amazonS3;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var buckets = await amazonS3.ListBucketsAsync(stoppingToken);

            var list = await amazonS3.ListObjectsAsync("arielle", stoppingToken);

            var file = await amazonS3.GetObjectAsync(new GetObjectRequest
            {
                BucketName = "arielle",
                Key = "d9a0b60a-a3be-4c25-b171-1c69d05aa1fb.txt"

            }, stoppingToken);

            var opt = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            var message = JsonSerializer.Deserialize<ContentMessage>(file.ResponseStream, opt);
        }
    }
}

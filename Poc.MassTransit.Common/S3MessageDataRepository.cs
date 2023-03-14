using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using MassTransit;

namespace Poc.MassTransit.Common
{
    public class S3MessageDataRepository : IMessageDataRepository
    {
        private readonly IAmazonS3 amazonS3Client;
        private readonly string bucketName;

        public S3MessageDataRepository(IAmazonS3 amazonS3Client, string bucketName = "message-data")
        {
            this.amazonS3Client = amazonS3Client;
            this.bucketName = bucketName;
        }              


        public async Task<Stream> Get(Uri address, CancellationToken cancellationToken = default)
        {
            try
            {
                var amazonUri = new AmazonS3Uri(address);
                if (amazonUri is null || amazonUri.Bucket is null || amazonUri.Key is null)
                    throw new MessageDataException($"Address URI could not be readed as AmazonS3Uri: '{address.AbsoluteUri}'");

                var fileObject = await amazonS3Client.GetObjectAsync(new GetObjectRequest
                {
                    BucketName = amazonUri.Bucket,
                    Key = amazonUri.Key

                }, cancellationToken);

                return fileObject.ResponseStream;
            }
            catch (Exception e) when (e.GetType() != typeof(MessageDataException))
            {
                throw new MessageDataException($"MessageData content not found from '{address.AbsoluteUri}'", e);
            }
        }

        public async Task<Uri> Put(Stream stream, TimeSpan? timeToLive = null, CancellationToken cancellationToken = default)
        {
            try
            {
                await amazonS3Client.EnsureBucketExistsAsync(bucketName);

                var fileKey = $"{Guid.NewGuid()}.txt";
                var response = await amazonS3Client.PutObjectAsync(new PutObjectRequest
                {
                    BucketName = bucketName,
                    InputStream = stream,
                    Key = fileKey
                }, cancellationToken);

                LogContext.Debug?.Log($"Stream sended to bucket '{bucketName}' with key '{fileKey}'");

                return new Uri($"s3://{bucketName}/{fileKey}");
            }
            catch (Exception ex)
            {
                throw new MessageDataException($"Error to send stream to bucket '{bucketName}'", ex);
            }
        }
    }
}

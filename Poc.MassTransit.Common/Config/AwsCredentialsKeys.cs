using Amazon.Runtime;

namespace Poc.MassTransit.Common.Config
{
    public class AwsCredentialsKeys : AWSCredentials
    {
        private readonly AppConfig applicationConfig;

        public AwsCredentialsKeys(AppConfig appConfig)
        {
            applicationConfig = appConfig;
        }

        public override ImmutableCredentials GetCredentials()
        {
            return new ImmutableCredentials(applicationConfig.AWS!.AccessKeyId, applicationConfig.AWS.SecretAccessKey, null);
        }
    }
}
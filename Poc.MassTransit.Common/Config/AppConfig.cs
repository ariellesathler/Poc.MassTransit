namespace Poc.MassTransit.Common.Config
{
    public class AppConfig
    {
        public PocKindConfig? PocKind { get; set; }
        public BusConfig? Bus { get; set; }
        public DatabaseConfig? Mongo { get; set; }
        public BlobStorageConfig? BlobStorage { get; set; }

        public AppConfig()
        {}


        public class PocKindConfig
        {
            public string BusType { get; set; }
            public string MessageDataRepositoryType { get; set; }

        }

        public class BusConfig
        {
            public string ConnectionString { get; set; }
        }

        public class DatabaseConfig
        {
            public string ConnectionString { get; set; }
            public string DatabaseName { get; set; }
        }

        public class BlobStorageConfig
        {
            public string ConnectionString { get;  set; }
            public string ContainerName { get;  set; }
        }
    }
}
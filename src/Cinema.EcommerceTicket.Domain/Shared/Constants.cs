using System.Configuration;

namespace Cinema.EcommerceTicket.Domain.Shared;

public static class Constants
{
    public static string ENVIRONMENT => Environment.GetEnvironmentVariable("ENV") ?? "ENV";

    public static class CatalogApi
    {
        public static string NAME => "CatalogApi";
        public static string BASE_URL => Environment.GetEnvironmentVariable("CATALOG_API_BASE_URL") ?? throw new ConfigurationErrorsException("A variável de amibente CATALOG_API_BASE_URL não pode ser nula.");
    }

    public static class RabbitMq
    {
        public static string RABBIMQ_PORT => Environment.GetEnvironmentVariable("RABBIMQ_PORT") ?? throw new ConfigurationErrorsException("A variável de amibente RABBIMQ_PORT não pode ser nula.");  
        public static string RABBIMQ_HOST => Environment.GetEnvironmentVariable("RABBIMQ_HOST") ?? throw new ConfigurationErrorsException("A variável de amibente RABBIMQ_HOST não pode ser nula.");  
        public static string RABBIMQ_USERNAME = Environment.GetEnvironmentVariable("RABBIMQ_USERNAME") ?? throw new ConfigurationErrorsException("A variável de amibente RABBIMQ_USERNAME não pode ser nula.");
        public static string RABBIMQ_PASSWORD = Environment.GetEnvironmentVariable("RABBIMQ_PASSWORD") ?? throw new ConfigurationErrorsException("A variável de amibente RABBIMQ_PASSWORD não pode ser nula.");
        public static string QUEUE_CREATE_ECOMMERCE_TICKET_NAME = Environment.GetEnvironmentVariable("QUEUE_CREATE_ECOMMERCE_TICKET_NAME") ?? throw new ConfigurationErrorsException("A variável de amibente QUEUE_NAME não pode ser nula.");
    }

    public static class MongoDb
    {
        public static string MONGODB_CONNECTION_STRING => Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING") ?? throw new ConfigurationErrorsException("A variável de amibente MONGODB_CONNECTION_STRING não pode ser nula.");
        public static string MONGODB_DATABASE_NAME => Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME") ?? throw new ConfigurationErrorsException("A variável de amibente MONGODB_DATABASE_NAME não pode ser nula.");
        public static string MONGODB_TICKETS_COLLECTION_NAME => Environment.GetEnvironmentVariable("MONGODB_TICKETS_COLLECTION_NAME") ?? throw new ConfigurationErrorsException("A variável de amibente MONGODB_TICKETS_COLLECTION_NAME não pode ser nula.");
    }
}

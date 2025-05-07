using System;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Interface.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Ambev.DeveloperEvaluation.Domain.Services
{
    public class MongoDBService : IMongoDBService
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<BsonDocument> _salesCollection;
        private readonly ILogger<MongoDBService> _logger;

        public MongoDBService(IConfiguration configuration, ILogger<MongoDBService> logger)
        {
            try
            {
                _logger = logger;
                var connectionString = configuration.GetConnectionString("MongoDBConnection");
                
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new ArgumentNullException(nameof(connectionString), "MongoDB connection string is not configured");
                }

                var client = new MongoClient(connectionString);
                _database = client.GetDatabase("DeveloperEvaluation");
                _salesCollection = _database.GetCollection<BsonDocument>("Sales");

                _logger.LogInformation("MongoDB service initialized successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing MongoDB service");
                throw;
            }
        }

        public async Task SaveSalesAsync(object salesData)
        {
            try
            {
                if (salesData == null)
                {
                    throw new ArgumentNullException(nameof(salesData));
                }

                var document = BsonDocument.Parse(salesData.ToString());
                await _salesCollection.InsertOneAsync(document);
                _logger.LogInformation("Sale data saved successfully to MongoDB");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving sale data to MongoDB");
                throw new Exception($"Erro ao salvar no MongoDB: {ex.Message}", ex);
            }
        }

        public async Task<object> GetSalesAsync(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new ArgumentNullException(nameof(id));
                }

                var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
                var document = await _salesCollection.Find(filter).FirstOrDefaultAsync();
                
                if (document == null)
                {
                    _logger.LogWarning("No sale found with id {SaleId}", id);
                    return null;
                }

                _logger.LogInformation("Sale data retrieved successfully from MongoDB");
                return document?.ToJson();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving sale data from MongoDB");
                throw new Exception($"Erro ao buscar no MongoDB: {ex.Message}", ex);
            }
        }

        public async Task DeleteSaleAsync(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new ArgumentNullException(nameof(id));
                }

                var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
                var result = await _salesCollection.DeleteOneAsync(filter);
                
                if (result.DeletedCount == 0)
                {
                    _logger.LogWarning("No sale found to delete with id {SaleId}", id);
                    return;
                }

                _logger.LogInformation("Sale data deleted successfully from MongoDB");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting sale data from MongoDB");
                throw new Exception($"Erro ao deletar no MongoDB: {ex.Message}", ex);
            }
        }
    }
} 
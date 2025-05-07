using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interface;
using Ambev.DeveloperEvaluation.Domain.Interface.IRepositories;
using Ambev.DeveloperEvaluation.Domain.Interface.IServices;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.Domain.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMongoDBService _mongoDBService;
        private readonly ILogger<UnitOfWork> _logger;
        private bool _disposed;
        private bool _mongoDBSuccess;
        private Sale? _currentSale;

        public UnitOfWork(
            ISaleRepository saleRepository,
            IMongoDBService mongoDBService,
            ILogger<UnitOfWork> logger)
        {
            _saleRepository = saleRepository;
            _mongoDBService = mongoDBService;
            _logger = logger;
        }

        public async Task<Sale> CreateSaleAsync(Sale sale)
        {
            try
            {
                _logger.LogInformation("Iniciando criação de venda com Unit of Work");
                
                // Salva no PostgreSQL
                _currentSale = await _saleRepository.AddAsync(sale);
                
                // salvar no MongoDB
                var saleJson = JsonSerializer.Serialize(_currentSale);
                await _mongoDBService.SaveSalesAsync(saleJson);
                _mongoDBSuccess = true;
                
                _logger.LogInformation("Venda {SaleId} salva com sucesso em ambos os bancos", sale.Id);
                
                return _currentSale;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar venda com Unit of Work");
                await RollbackAsync();
                throw;
            }
        }

        public async Task<Sale?> GetSaleAsync(Guid id)
        {
            try
            {
                // Busca apenas no PostgreSQL, pois é a fonte principal
                return await _saleRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar venda {SaleId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Sale>> GetSalesAsync(int page, int size)
        {
            try
            {
                // Busca apenas no PostgreSQL, pois é a fonte principal
                return await _saleRepository.GetAllAsync(page, size);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar vendas - Página: {Page}, Tamanho: {Size}", page, size);
                throw;
            }
        }

        public async Task UpdateSaleAsync(Sale sale)
        {
            try
            {
                _logger.LogInformation("Iniciando atualização de venda {SaleId}", sale.Id);
                
                // Atualiza no PostgreSQL
                await _saleRepository.UpdateAsync(sale);
                
                // Atualiza no MongoDB
                var saleJson = JsonSerializer.Serialize(sale);
                await _mongoDBService.SaveSalesAsync(saleJson);
                
                _logger.LogInformation("Venda {SaleId} atualizada com sucesso em ambos os bancos", sale.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar venda {SaleId}", sale.Id);
                throw;
            }
        }

        public async Task<int> GetTotalSalesCountAsync()
        {
            try
            {
                // Busca apenas no PostgreSQL, pois é a fonte principal
                return await _saleRepository.GetTotalCountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar total de vendas");
                throw;
            }
        }

        public async Task<bool> CommitAsync()
        {
            if (_currentSale == null)
            {
                _logger.LogWarning("Tentativa de commit sem venda em andamento");
                return false;
            }

            try
            {
                if (!_mongoDBSuccess)
                {
                    _logger.LogWarning("Commit não realizado: MongoDB não foi atualizado");
                    return false;
                }

                _logger.LogInformation("Commit realizado com sucesso para venda {SaleId}", _currentSale.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao realizar commit");
                await RollbackAsync();
                return false;
            }
        }

        public async Task RollbackAsync()
        {
            if (_currentSale == null)
            {
                _logger.LogWarning("Tentativa de rollback sem venda em andamento");
                return;
            }

            try
            {
                if (_mongoDBSuccess)
                {
                    // Se o MongoDB foi atualizado mas houve erro no PostgreSQL,
                    // precisamos limpar o MongoDB
                    await _mongoDBService.DeleteSaleAsync(_currentSale.Id.ToString());
                    _logger.LogInformation("Rollback realizado: dados removidos do MongoDB");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao realizar rollback");
                // Não relançamos a exceção para não mascarar o erro original
            }
            finally
            {
                _currentSale = null;
                _mongoDBSuccess = false;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _currentSale = null;
            }
            _disposed = true;
        }
    }
} 
using Dapper;
using Npgsql;
using System.Data;
using Application.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    internal sealed class DbService : IDbService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public DbService(IConfiguration configuration, ILogger<DbService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<T> GetAsync<T>(string command, object parms)
        {
            try
            {
                using IDbConnection connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                return (await connection.QueryAsync<T>(command, parms)
                    .ConfigureAwait(false))
                    .FirstOrDefault()!;
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Error occurred while executing query");
                throw;
            }
        }

        public async Task<List<T>> GetAll<T>(string command, object parms)
        {
            try
            {
                using IDbConnection connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));

                return (await connection.QueryAsync<T>(command, parms)).ToList();
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Error occurred while executing query");
                throw;
            }
        }

        public async Task<int> EditData(string command, object parms)
        {
            try
            {
                using IDbConnection connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));

                return await connection.ExecuteAsync(command, parms);
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Error occurred while executing query");
                throw;
            }
        }
    }
}

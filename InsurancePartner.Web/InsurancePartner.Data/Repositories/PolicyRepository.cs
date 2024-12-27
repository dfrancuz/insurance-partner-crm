using Dapper;
using InsurancePartner.Data.Configurations;
using InsurancePartner.Data.Interfaces;
using InsurancePartner.Data.Models;
using Microsoft.Data.SqlClient;

namespace InsurancePartner.Data.Repositories;

public class PolicyRepository : IPolicyRepository
{
    private readonly string _connectionString;

    public PolicyRepository(DatabaseConfig config)
    {
        _connectionString = config.ConnectionString;
    }

    public async Task<int> CreatePolicyAsync(Policy policy)
    {
        await using var connection = new SqlConnection(_connectionString);
        const string sql =
            @"INSERT INTO Policies (PartnerId, PolicyNumber, Amount)
              VALUES (@PartnerId, @PolicyNumber, @Amount)
              SELECT CAST(SCOPE_IDENTITY() as int)";

        return await connection.ExecuteScalarAsync<int>(sql, policy);
    }
}

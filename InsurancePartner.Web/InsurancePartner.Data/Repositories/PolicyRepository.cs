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

    public async Task<IEnumerable<Policy>> GetPartnerPoliciesAsync(int partnerId)
    {
        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<Policy>(
            "SELECT * FROM Policies WHERE PartnerId = @PartnerId",
            new
            {
                PartnerId = partnerId
            });
    }

    public async Task<bool> PolicyNumberExistsAsync(string policyNumber)
    {
        await using var connection = new SqlConnection(_connectionString);
        var count = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(1) FROM Policies WHERE PolicyNumber = @PolicyNumber",
            new
            {
                PolicyNumber = policyNumber
            });

        return count > 0;
    }
}

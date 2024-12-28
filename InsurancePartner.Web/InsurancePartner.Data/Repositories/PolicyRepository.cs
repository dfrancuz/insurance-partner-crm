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
            @"INSERT INTO Policies (PolicyNumber, Amount)
              VALUES (@PolicyNumber, @Amount)
              SELECT CAST(SCOPE_IDENTITY() as int)";

        return await connection.ExecuteScalarAsync<int>(sql, policy);
    }

    public async Task<int> AssignPolicyToPartnerAsync(int policyId, int partnerId)
    {
        await using var connection = new SqlConnection(_connectionString);
        const string sql =
            @"INSERT INTO PolicyPartners (PolicyId, PartnerId)
              VALUES (@PolicyId, @PartnerId);
              SELECT CAST(SCOPE_IDENTITY() as int)";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                PolicyId = policyId,
                PartnerId = partnerId
            });
    }

    public async Task<IEnumerable<Policy>> GetPartnerPoliciesAsync(int partnerId)
    {
        await using var connection = new SqlConnection(_connectionString);
        const string sql =
            @"SELECT p.*, pp.*
              FROM Policies p
              INNER JOIN PolicyPartners pp ON p.PolicyId = pp.PolicyId
              WHERE pp.PartnerId = @PartnerId";

        return await connection.QueryAsync<Policy>(
            sql,
            new
            {
                PartnerId = partnerId
            });
    }

    public async Task<bool> RemovePolicyFromPartnerAsync(int policyId, int partnerId)
    {
        await using var connection = new SqlConnection(_connectionString);
        const string sql =
            @"DELETE FROM PolicyPartners
              WHERE PolicyId = @PolicyId AND PartnerId = @PartnerId";

        var rowsAffected = await connection.ExecuteAsync(
            sql,
            new
            {
                PolicyId = policyId,
                PartnerId = partnerId
            });

        return rowsAffected > 0;
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

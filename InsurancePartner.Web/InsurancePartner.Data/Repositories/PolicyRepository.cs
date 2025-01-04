namespace InsurancePartner.Data.Repositories;

using Configurations;
using Dapper;
using Interfaces;
using Microsoft.Data.SqlClient;
using Models;

public class PolicyRepository : IPolicyRepository
{
    private readonly string _connectionString;

    public PolicyRepository(DatabaseConfig config)
    {
        _connectionString = config.ConnectionString;
    }

    public async Task<IEnumerable<Policy>> GetAllPoliciesAsync()
    {
        await using var connection = new SqlConnection(_connectionString);
        const string sql = @"SELECT * FROM Policies ORDER BY CreatedAtUtc DESC";
        return await connection.QueryAsync<Policy>(sql);
    }

    public async Task<Policy?> GetPolicyByIdAsync(int policyId)
    {
        await using var connection = new SqlConnection(_connectionString);
        const string sql = @"SELECT * FROM Policies WHERE PolicyId = @PolicyId";
        return await connection.QuerySingleOrDefaultAsync<Policy>(
            sql,
            new
            {
                PolicyId = policyId
            });
    }

    public async Task<IEnumerable<Policy>> GetPoliciesByIdsAsync(List<int> policyIds)
    {
        await using var connection = new SqlConnection(_connectionString);
        const string sql =
            @"SELECT * 
              FROM Policies 
              WHERE PolicyId IN @PolicyIds";

        return await connection.QueryAsync<Policy>(
            sql,
            new
            {
                PolicyIds = policyIds
            });
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

    public async Task<bool> UpdatePolicyAsync(Policy policy)
    {
        await using var connection = new SqlConnection(_connectionString);
        const string sql =
            @"UPDATE Policies
              SET PolicyNumber = @PolicyNumber,
                  Amount = @Amount,
                  CreatedAtUtc = @CreatedAtUtc
              WHERE PolicyId = @PolicyId";

        var affectedRows = await connection.ExecuteAsync(sql, policy);

        return affectedRows > 0;
    }

    public async Task<(bool IsDeleted, string message)> DeletePolicyAsync(int policyId)
    {
        await using var connection = new SqlConnection(_connectionString);
        const string fetchPartnersSql =
            @"SELECT p.PartnerId, p.FirstName, p.LastName
              FROM Partners p
              INNER JOIN PolicyPartners pp ON p.PartnerId = pp.PartnerId
              WHERE pp.PolicyId = @PolicyId";

        var affectedPartners = await connection.QueryAsync<Partner>(
            fetchPartnersSql,
            new
            {
                PolicyId = policyId
            });

        var partnerNames = affectedPartners.Select(p => p.FullName).ToList();

        var partnersMessage = partnerNames.Any()
            ? $"Policy is associated with the following partners who will loose access: {string.Join(", ", partnerNames)}."
            : "No partners are associated with this policy.";

        const string deleteAssociationsSql =
            @"DELETE FROM PolicyPartners
              WHERE PolicyId = @PolicyId";

        await connection.ExecuteAsync(
            deleteAssociationsSql,
            new
            {
                PolicyId = policyId
            });

        const string deletePolicySql =
            @"DELETE FROM Policies
              WHERE PolicyId = @PolicyId";

        var affectedRows = await connection.ExecuteAsync(
            deletePolicySql,
            new
            {
                PolicyId = policyId
            });

        return affectedRows > 0
            ? (true, $@"Policy deleted successfully. {partnersMessage}")
            : (false, "Policy deletion failed.");
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

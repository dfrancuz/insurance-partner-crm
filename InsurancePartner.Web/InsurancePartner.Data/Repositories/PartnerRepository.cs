namespace InsurancePartner.Data.Repositories;

using Configurations;
using Dapper;
using Interfaces;
using Microsoft.Data.SqlClient;
using Models;

public class PartnerRepository : IPartnerRepository
{
    private readonly string _connectionString;

    public PartnerRepository(DatabaseConfig config)
    {
        _connectionString = config.ConnectionString;
    }

    public async Task<IEnumerable<Partner>> GetAllPartnersAsync()
    {
        await using var connection = new SqlConnection(_connectionString);
        const string sql =
            @"SELECT p.*,
                     COUNT(pol.PolicyId) as PolicyCount,
                     SUM(pol.Amount) as TotalPolicyAmount 
              FROM Partners p 
              LEFT JOIN PolicyPartners pp ON p.PartnerId = pp.PartnerId
              LEFT JOIN Policies pol ON pp.PolicyId = pol.PolicyId
              GROUP BY p.PartnerId, p.FirstName, p.LastName, p.Address,
                       p.PartnerNumber, p.CroatianPIN, p.PartnerTypeId, 
                       p.CreatedAtUtc, p.CreateByUser, p.IsForeign, 
                       p.ExternalCode, p.Gender
              ORDER BY p.CreatedAtUtc DESC";

        return await connection.QueryAsync<Partner>(sql);
    }

    public async Task<Partner?> GetPartnerByIdAsync(int partnerId)
    {
        await using var connection = new SqlConnection(_connectionString);
        const string sql =
            @"SELECT p.*, pol.*
              FROM Partners p
              LEFT JOIN PolicyPartners pp ON p.PartnerId = pp.PartnerId
              LEFT JOIN Policies pol ON pp.PolicyId = pol.PolicyId
              WHERE p.PartnerId = @PartnerId";

        var partnerDictionary = new Dictionary<int, Partner>();

        await connection.QueryAsync<Partner, Policy, Partner>(
            sql,
            (partner, policy) =>
            {
                if (!partnerDictionary.TryGetValue(partner.PartnerId, out var partnerEntry))
                {
                    partnerEntry = partner;
                    partnerDictionary.Add(partner.PartnerId, partnerEntry);
                }

                if (policy != null)
                {
                    partnerEntry.Policies.Add(policy);
                }

                return partnerEntry;
            },

            new
            {
                PartnerId = partnerId,
            },
            splitOn: "PolicyId"
        );

        return partnerDictionary.Values.FirstOrDefault();
    }

    public async Task<int> CreatePartnerAsync(Partner partner)
    {
        await using var connection = new SqlConnection(_connectionString);
        const string sql =
            @"INSERT INTO Partners(FirstName, LastName, Address, PartnerNumber,
                     CroatianPIN, PartnerTypeId, CreateByUser, 
                     IsForeign, ExternalCode, Gender)
              VALUES (@FirstName, @LastName, @Address, @PartnerNumber,
                      @CroatianPIN, @PartnerTypeId, @CreateByUser,
                      @IsForeign, @ExternalCode, @Gender);
              SELECT CAST(SCOPE_IDENTITY() as int)";

        return await connection.ExecuteScalarAsync<int>(sql, partner);
    }

    public async Task<bool> UpdatePartnerAsync(Partner partner)
    {
        await using var connection = new SqlConnection(_connectionString);
        const string sql =
            @"UPDATE Partners
              SET FirstName = @FirstName,
                  LastName = @LastName,
                  Address = @Address,
                  PartnerNumber = @PartnerNumber,
                  CroatianPIN = @CroatianPIN,
                  PartnerTypeId = @PartnerTypeId,
                  CreateByUser = @CreateByUser,
                  IsForeign = @IsForeign,
                  ExternalCode = @ExternalCode,
                  Gender = @Gender
              WHERE PartnerId = @PartnerId";

        var rowsAffected = await connection.ExecuteAsync(sql, partner);
        return rowsAffected > 0;
    }

    public async Task<bool> DeletePartnerAsync(int partnerId)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            const string sql = @"DELETE FROM Partners WHERE PartnerId = @PartnerId";

            var rowsAffected = await connection.ExecuteAsync(
                sql,
                new
                {
                    PartnerId = partnerId
                },
                transaction: transaction);

            if (rowsAffected == 0)
            {
                return false;
            }

            await transaction.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            await transaction.RollbackAsync();
            return false;
        }
    }

    public async Task<IEnumerable<PartnerType>> GetPartnerTypesAsync()
    {
        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<PartnerType>("SELECT * FROM PartnerTypes");
    }

    public async Task<bool> ExternalCodeExistsAsync(string externalCode)
    {
        await using var connection = new SqlConnection(_connectionString);
        var count = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(1) FROM Partners WHERE ExternalCode = @ExternalCode",
            new
            {
                ExternalCode = externalCode
            });

        return count > 0;
    }

    public async Task<bool> PartnerNumberExistsAsync(string partnerNumber)
    {
        await using var connection = new SqlConnection(_connectionString);
        var count = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(1) FROM Partners WHERE PartnerNumber = @PartnerNumber",
            new
            {
                PartnerNumber = partnerNumber
            });

        return count > 0;
    }
}

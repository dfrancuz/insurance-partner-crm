using Dapper;
using InsurancePartner.Data.Configurations;
using InsurancePartner.Data.Interfaces;
using InsurancePartner.Data.Models;
using Microsoft.Data.SqlClient;

namespace InsurancePartner.Data.Repositories;

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
              LEFT JOIN Policies pol ON p.PartnerId = pol.PartnerId 
              GROUP BY p.PartnerId, p.FirstName, p.LastName, p.Address,
                       p.PartnerNumber, p.CroatianPIN, p.PartnerTypeId, 
                       p.CreatedAtUtc, p.CreateByUser, p.IsForeign, 
                       p.ExternalCode, p.Gender
              ORDER BY p.CreatedAtUtc DESC";

        return await connection.QueryAsync<Partner>(sql);
    }

    public Task<Partner?> GetPartnerByIdAsync(int partnerId)
    {
        throw new NotImplementedException();
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
}

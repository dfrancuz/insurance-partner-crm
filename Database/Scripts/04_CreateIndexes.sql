USE InsurancePartner
GO

CREATE INDEX IX_Partners_CreatedAtUtc ON Partners(CreatedAtUtc DESC);
CREATE INDEX IX_Policies_PartnerId ON Policies(PartnerId);

USE InsurancePartner
GO

CREATE INDEX IX_Partners_CreatedAtUtc ON Partners(CreatedAtUtc DESC);
CREATE INDEX IX_PolicyPartners_PolicyId ON PolicyPartners(PolicyId);
CREATE INDEX IX_PolicyPartners_PartnerId ON PolicyPartners(PartnerId);

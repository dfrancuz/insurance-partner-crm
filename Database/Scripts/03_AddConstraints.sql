USE InsurancePartner
GO

ALTER TABLE Partners
ADD CONSTRAINT FK_Partners_PartnerTypes
FOREIGN KEY (PartnerTypeId) REFERENCES PartnerTypes(PartnerTypeId);

ALTER TABLE Policies
ADD CONSTRAINT FK_Policies_Partners
FOREIGN KEY (PartnerId) REFERENCES Partners(PartnerId);

ALTER TABLE Partners
ADD CONSTRAINT UQ_Partners_ExternalCode UNIQUE (ExternalCode);

ALTER TABLE Partners
ADD CONSTRAINT UQ_Partners_PartnerNumber UNIQUE (PartnerNumber);

ALTER TABLE Policies
ADD CONSTRAINT UQ_Policies_PolicyNumber UNIQUE (PolicyNumber);

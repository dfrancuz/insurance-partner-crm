USE InsurancePartner
GO

-- Seed Partners
INSERT INTO Partners (
    FirstName, LastName, Address, PartnerNumber, CroatianPIN, 
    PartnerTypeId, CreateByUser, IsForeign, ExternalCode, Gender
) VALUES

-- Personal partners
(
    'Ana', 'Horvat', 'Ilica 242, Zagreb',
    '12345678901234567890', '12345678901',
    1, 'ana.admin@insurance.com', 0, 'HRPER000000001', 'F'
),
(
    'Marko', 'Kovačić', 'Vukovarska 123, Split',
    '98765432109876543210', '98765432109',
    1, 'marko.admin@insurance.com', 0, 'HRPER000000002', 'M'
),
(
    'Nina', 'Novak', 'Radnička 45, Rijeka',
    '11122233344455566677', '11122233344',
    1, 'nina.admin@insurance.com', 0, 'HRPER000000003', 'F'
),

-- Legal partners
(
    'Tech', 'Solutions d.o.o.', 'Savska 67, Zagreb',
    '22233344455566677788', NULL,
    2, 'admin@insurance.com', 0, 'HRLEG000000001', 'N'
),
(
    'Global', 'Trading GmbH', 'Hauptstrasse 1, Vienna',
    '33344455566677788899', NULL,
    2, 'admin@insurance.com', 1, 'ATLEG000000001', 'N'
),
(
    'Mario', 'Schmidt', 'Bahnhofstrasse 123, Munich',
    '44455566677788899900', NULL,
    1, 'admin@insurance.com', 1, 'DEPER000000001', 'M'
),
(
    'Elena', 'Kovač', 'Slovenska 55, Ljubljana',
    '55566677788899900011', NULL,
    1, 'admin@insurance.com', 1, 'SIPER000000001', 'F'
);

-- Seed Policies
INSERT INTO Policies (PolicyNumber, Amount) VALUES 
('POL0000000001', 1500.00),
('POL0000000002', 2750.50),
('POL0000000003', 5500.00),
('POL0000000004', 3250.75),
('POL0000000005', 6750.25),
('POL0000000006', 4250.00),
('POL0000000007', 1750.50);

-- Link Policies to Partners
INSERT INTO PolicyPartners (PolicyId, PartnerId)
SELECT p.PolicyId, par.PartnerId
FROM 
    (VALUES 
        (1, 1), -- Ana has 2 policies
        (2, 1),
        (3, 2), -- Marko has 1 policy
        (4, 3), -- Nina has 1 policy
        (5, 4), -- Tech Solutions has 1 expensive policy
        (6, 5), -- Global Trading has 1 policy
        (7, 6)  -- Mario has 1 policy
    ) AS links(PolicyId, PartnerPosition)
    CROSS APPLY (
        SELECT PartnerId 
        FROM Partners 
        ORDER BY PartnerId 
        OFFSET links.PartnerPosition-1 ROWS 
        FETCH NEXT 1 ROWS ONLY
    ) AS par
    CROSS APPLY (
        SELECT PolicyId 
        FROM Policies 
        ORDER BY PolicyId 
        OFFSET links.PolicyId-1 ROWS 
        FETCH NEXT 1 ROWS ONLY
    ) AS p;

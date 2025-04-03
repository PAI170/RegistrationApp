USE EmployeesDB;
GO

-- Insertar tipos de documentos
INSERT INTO DocumentType (Name, Description)
VALUES 
    ('DNI', 'Documento Nacional de Identidad'),
    ('Pasaporte', 'Pasaporte'),
    ('DIMEX', 'Documento de Identificacion de Migracion y Extranjeria'),
    ('PT', 'Permiso de Trabajo'),
	('Licencia', 'Licencia de Conducir Nacional');
GO

-- Insertar países
INSERT INTO Country (Name, CountryCode)
VALUES 
    ('Costa Rica', '+506'),
    ('Nicaragua', '+505');
GO

-- Insertar provincias
INSERT INTO Province (Name, CountryId)
VALUES 
    ('Limon', 1),
    ('Cartago', 1),
    ('San Jose', 1),
    ('Heredia', 1),
    ('Alajuela', 1),
	('Puntarenas', 1),
	('Guanacaste', 1);
GO

-- Insertar ciudades
INSERT INTO City (Name, ProvinceId)
VALUES 
    ('Pococi', 1),
    ('Desamparados', 3),
    ('San Ramon', 5),
    ('Paraiso', 2),
    ('Barva', 4),
	('Santa Cruz', 7),
	('Montes de Oro', 6);
GO

-- Insertar distritos
INSERT INTO District (Name, CityId)
VALUES 
    ('Cariari', 1),
    ('Hatillo', 2),
    ('Piedades Norte', 3),
    ('Santiago', 4),
    ('San Pedro', 5),
    ('Tempate', 6),
    ('Miramar', 7);
GO

-- Insertar empleados
INSERT INTO Employee (
    DocumentTypeId, 
    DocumentId, 
    Name, 
    LastName, 
    BirthDay, 
    Email, 
    JoinedDate, 
    CostPerHour, 
    IBAN, 
    CountryId, 
    ProvinceId, 
    CityId, 
    DistrictId, 
    StreetAddress, 
    PhoneNumber, 
    PhonePrefix
)
VALUES 
    (1, '12345678A', 'María', 'González', '1985-05-12', 'maria.gonzalez@example.com', '2022-01-15', 25.50, 'ES9121000418450200051332', 1, 1, 1, 1, 'Calle Gran Vía 28', '612345678', '+506'),
    (1, '87654321B', 'Juan', 'Martínez', '1990-08-23', 'juan.martinez@example.com', '2021-11-05', 28.75, 'ES7100730100590163309263', 1, 2, 2, 3, 'Av. Diagonal 123', '623456789', '+506'),
    (2, 'XDG123456', 'Ana', 'López', '1988-03-30', 'ana.lopez@example.com', '2022-03-20', 30.00, 'ES9121000418450200051444', 1, 3, 3, 5, 'Calle Colón 45', '634567890', '+506'),
    (3, 'Y3456789B', 'Carlos', 'Rodríguez', '1995-11-15', 'carlos.rodriguez@example.com', '2022-02-01', 22.50, 'ES7100730100590163309555', 1, 4, 4, 6, 'Av. de la Constitución 12', '645678901', '+506'),
    (1, '55667788C', 'Laura', 'Fernández', '1992-07-08', 'laura.fernandez@example.com', '2021-09-15', 26.25, 'ES9121000418450200051666', 1, 5, 5, 7, 'Plaza Circular 5', '656789012', '+506');
GO

SELECT 
    e.Id,
    e.Name, 
    e.LastName, 
    dt.Name AS DocumentType,
    e.DocumentId,
    e.Email, 
    e.CostPerHour,
    c.Name AS Country,
    p.Name AS Province,
    ct.Name AS City,
    d.Name AS District
FROM Employee e
JOIN DocumentType dt ON e.DocumentTypeId = dt.Id
JOIN Country c ON e.CountryId = c.Id
JOIN Province p ON e.ProvinceId = p.Id
JOIN City ct ON e.CityId = ct.Id
JOIN District d ON e.DistrictId = d.Id;
GO
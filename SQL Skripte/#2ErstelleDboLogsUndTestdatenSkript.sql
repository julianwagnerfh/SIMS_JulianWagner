CREATE TABLE dbo.logs (
    id INT PRIMARY KEY IDENTITY(1,1), -- Automatische ID-Generierung
    zeitstempel DATETIME NOT NULL,     -- Zeitstempel f�r Logs
    beschreibung NVARCHAR(MAX) NOT NULL -- Beschreibung des Logs
);

-- Einf�gen von Testdaten in die Tabelle
INSERT INTO dbo.logs (id, zeitstempel, beschreibung)
VALUES
    (1, GETDATE(), 'Logeintrag 1: System gestartet.')
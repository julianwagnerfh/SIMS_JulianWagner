USE IncidentFrontendApp;

Create table dbo.Incidents (
    id INT PRIMARY KEY Identity(1,1),   -- Primärschlüssel für jeden Vorfall
    bearbeiter VARCHAR(255),             -- Der Benutzer, der den Vorfall bearbeitet
    melder VARCHAR(255),                 -- Der Benutzer, der den Vorfall gemeldet hat
    schweregrad VARCHAR(255), -- Schweregrad des Vorfalls
    bearbeitungsstatus VARCHAR(255), -- Aktueller Status des Vorfalls
    cve VARCHAR(50),                     -- CVE (Common Vulnerabilities and Exposures) Referenz
    systembetroffenheit VARCHAR(255),                 -- Betroffenes System
    beschreibung VARCHAR(255),                   -- Beschreibung des Vorfalls
    zeitstempel Datetime2(7), -- Zeitpunkt der Meldung oder Bearbeitung
    eskalationslevel INT       -- Eskalationslevel, z.B. 1, 2, 3 für unterschiedliche Benutzer
)

INSERT INTO dbo.Incidents (bearbeiter, melder, schweregrad, bearbeitungsstatus, cve, systembetroffenheit, beschreibung, zeitstempel, eskalationslevel)
VALUES ('Max Mustermann', 'Anna Müller', 'hoch', 'offen', 'CVE-2023-1234', 'Webserver-01', 'Unautorisierter Zugriff entdeckt', '2024-10-15 10:00:00', 1);

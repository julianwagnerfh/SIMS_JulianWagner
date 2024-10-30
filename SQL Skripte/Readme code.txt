# Projektname: SIMS API

![Build Status](https://badgen.net/github/status/username/repo-name)
![License](https://badgen.net/badge/license/MIT/green)
![Version](https://badgen.net/badge/version/1.0.0/blue)

## Übersicht
Die SIMS API ist eine Web-API, die eine einfache Benutzerverwaltung und Incident-Management-System bietet. Sie ermöglicht das Registrieren, Einloggen und Verwalten von Benutzern sowie das Erstellen, Abrufen, Aktualisieren und Löschen von Incidents.

## Inhaltsverzeichnis
1. [Systemvoraussetzungen](#systemvoraussetzungen)
2. [Features](#features)
3. [Installation](#installation)
4. [Verwendung](#verwendung)
5. [API-Endpunkte](#api-endpunkte)
8. [SAST Ergebnisse](#sast-ergebnisse)
9. [Mitwirkende](#mitwirkende)
10. [Roadmap](#roadmap)
11. [Lizenz](#lizenz)
12. [GitHub-Repo](#github-repo)

## Systemvoraussetzungen
- **Betriebssystem**: Windows
- **.NET-Runtime**: .NET 8 oder höher

## Features
- Benutzerregistrierung und -anmeldung mit Passwort-Hashing.
- Verwaltung von Benutzern in einer Redis-Datenbank.
- Incident-Management mit SQL-Datenbank.
- CRUD-Operationen für Incidents.
- Logging für wichtige Ereignisse in der Datenbank.

## Installation
1. Klone das Repository:
   ```bash
   git clone https://github.com/username/sims-api.git
   cd sims-api
2. Installiere die benötigten NuGet-Pakete
In Redis Controller:
- using Microsoft.AspNetCore.Http;
- using Microsoft.AspNetCore.Mvc;
- using StackExchange.Redis;
- using BCrypt.Net;

In SIMSAPIController
- using Microsoft.AspNetCore.Http;
- using Microsoft.AspNetCore.Mvc;
- using SIMSAPI.Models;
- using System.Data;
- using System.Data.SqlClient;

3. Kompiliere die Compose-File und stelle sicher dass alle 4 container laufen
```bash
version: '3.4'

services:
  simsapi:
    image: ${DOCKER_REGISTRY-}simsapi
    build:
      context: .
      dockerfile: SIMSAPI/Dockerfile

  simsfrontend:
    image: ${DOCKER_REGISTRY-}simsfrontend
    build:
      context: .
      dockerfile: SIMSFrontend/Dockerfile

  sql-1:
    image: mcr.microsoft.com/mssql/server:2019-latest
    container_name: sql-1
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=Adm1234!
    ports:
      - "1433:1433"

  redis:
    image: redis:latest
    container_name: redis
    ports:
      - "6379:6379"

4. Starte das Compose-Projekt
```

### Verwendung
Die API kann mit Tools wie Swagger, Postman oder cURL verwendet werden, um HTTP-Anfragen an die verschiedenen Endpunkte zu senden.

### API-Endpunkte
- Benutzerverwaltung
```Bash
POST /api/register
Registriert einen neuen Benutzer.
POST /api/login
Meldet einen Benutzer an.
GET /api/users
Gibt eine Liste aller Benutzer zurück.
DELETE /api/users/{username}
Löscht einen Benutzer.
````

- Incident-Management
```Bash
GET /api/GetIncidents
Gibt alle Incidents zurück.
POST /api/AddIncident
Fügt einen neuen Incident hinzu.
DELETE /api/DeleteIncident
Löscht einen Incident.
PUT /api/UpdateIncident/{id}
Aktualisiert einen bestehenden Incident.
```

### SAST Ergebnisse
Die statische Codeanalyse mit Semgrep hat folgende Sicherheitsprobleme identifiziert:

Passwort-Hashing: Sichere Verwendung von BCrypt für Passwort-Hashing.
SQL-Injection: Alle SQL-Befehle verwenden Parameterbindung, um SQL-Injection-Angriffe zu verhindern.

### Mitwirkende
Max Mustermann (max@example.com)
Lisa Beispiel (lisa@example.com)

### Roadmap
 Unterstützung für OAuth2-Authentifizierung
 Implementierung eines Frontend-Clients
 Integration von Benachrichtigungen für Incidents

### Lizenz
Dieses Projekt ist unter der MIT-Lizenz lizenziert. 

### GitHub-Repo
Hier geht's zum GitHub-Repository:
- github.com/julianwagnerfh?tab=repositories

  
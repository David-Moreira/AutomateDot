# 🔄 AutomateDot - Self-Hosted Automation Dashboard

AutomateDot is a lightweight, self-hosted automation tool built with Blazor and .NET. It lets you create automation recipes ("If This Then That") using triggers and actions — no cloud dependencies, no lock-in.

## ✨ Features

- 🧩 Create custom automation workflows
- 📬 Trigger on GitHub events, webhooks, timers
- 🚀 Actions like sending webhooks, emails, logs
- 🐳 Docker-ready for deployment anywhere

## 🛠️ Example Use Cases

- On new GitHub issue → Send a Discord message
- On failed ping → Send an email
- On daily schedule → Run a script

## 📦 Getting Started

### Option 1: Run with Docker

```bash
docker run -d \
  -p 8888:8080 \
  -v ./databases:/app/databases \
  -v /DataProtection-Keys:/root/.aspnet/DataProtection-Keys \
  -v /scripts/:/app/scripts/ \
  -e ConnectionStrings:Sqlite=databases/sqlite.db \
  -e ConnectionStrings:HangfireSqlite=databases/hangfirelite.db \
  -e Serilog:WriteTo:0:Name=Console \
  daverick/automatedot
```

### Option 2: Run with Docker Compose

```bash
name: automatedot
services:
  app:
    image: dockerdaverick/automatedot
    ports:
      - "8888:8080"
    volumes:
      - /volumes/automatedot/DataProtection-Keys:/root/.aspnet/DataProtection-Keys
      - /volumes/automatedot/databases/:/app/databases/
      - /volumes/automatedot/scripts/:/app/scripts/
    environment:
      - ConnectionStrings:Sqlite=databases/sqlite.db
      - ConnectionStrings:HangfireSqlite=databases/hangfirelite.db
      - Serilog:WriteTo:0:Name=Console
    restart: always
```

### Option 3: Run Locally (Manual) - For Developers

```
git clone https://github.com/david-moreira/AutomateDot.git
cd AutomateDot
dotnet run
```

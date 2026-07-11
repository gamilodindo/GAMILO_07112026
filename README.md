# Coding Challenge - File Processor API

A RESTful API built with ASP.NET Core with SQLite that processes uploaded CSV and JSON files.

## Features
- Upload CSV/JSON files
- File Validation
- API Key Authentication
- Swagger Documentation

## Technologies
- .Net 8.0
- ASP.NET Core Web API
- EntityFramework Core
- SQLite
- Swagger
- Serilog
- Clean Architecture

## Prerequisites
- Visual Studio 2022 (17.8+) or Visual Studio 2026
- .NET 8 SDK
- Git

## Clone the Repository

```bash
git clone https://github.com/gamilodindo/GAMILO_07112026.git
```

or download the repository as a ZIP from GitHub.

## Open the Solution

1. Open Visual Studio.
2. Select **Open a project or solution**.
3. Open `FileProcessor.slnx`.

## Build the Solution

- Go to **Build → Build Solution**, or press **Ctrl + Shift + B**.

## Run the API

1. Set **FileProcessor.API** as the Startup Project.
2. Press **F5** or **Ctrl + F5**.
3. Swagger will open automatically.

## Database

If the database does not exist:

1. Open **Tools → NuGet Package Manager → Package Manager Console**.
2. Set the **Default Project** to **FileProcessor.Infrastructure**.
3. In Package Manager Console, run:

```
dotnet ef database update --project .\FileProcessor.Infrastructure --startup-project .\FileProcessor.API
```

## Swagger

After running the application, Swagger is available at:

```
https://localhost:7133/swagger
```

### API Key Authentication

Only the upload endpoint is protected using an API key.

1. Click the **Authorize** button in the upper-right corner of the Swagger page.
2. Enter the API key.
3. Click **Authorize**.
4. Close the dialog.
5. You can now call the protected endpoints.

> **Note:** The API key is found at FileProcessor.API/appsettings.json. Copy the value of Value under ApiKey section.

### Upload a File

1. Expand the **POST /api/Files/upload** endpoint.
2. Click **Try it out**.
3. Click **Choose File** and select a supported file (`.csv` or `.json` . Sample file is included on this repository named sample.json and sample_csv.csv).
4. Click **Execute**.
5. Review the response body.

### Supported File Types

- `.csv`
- `.json`

### Sample Successful Response

```json
{
  "filename": "sample_csv.csv",
  "recordCount": 16,
  "result": "The total number of customers is 16, and the total amount paid is Php85,120.00. Individual payments range from Php23.00 to Php63,214.00",
  "processingTime": "9.84 ms",
  "processedDateTime": "July 11, 2026 12:02:38 UTC"
}
```

### Get Report

1. Expand the **POST /api/Files/report** endpoint.
2. Click **Try it out**.
4. Click **Execute**.
5. Review the response body.

### Sample Successful Response

```json
[
  {
    "filename": "sample.json",
    "fileType": "JSON",
    "contentType": "application/json",
    "fileSize": "3.42 kb",
    "recordCount": "1 item(s)",
    "processingTime": "11.88 ms",
    "processedDateTime": "July 11, 2026 09:41:33 UTC"
  },
  {
    "filename": "sample_csv.csv",
    "fileType": "CSV",
    "contentType": "text/csv",
    "fileSize": "0.2 kb",
    "recordCount": "16 item(s)",
    "processingTime": "2975.86 ms",
    "processedDateTime": "July 11, 2026 09:41:49 UTC"
  }
]
```

## Error Responses

### Missing API Key

```json
{
  "message": "API Key is missing."
}
```

### Invalid API Key

```json
{
  "message": "Invalid API Key."
}
```

### Unsupported File Type

```json
{
  "message": "Unsupported file type."
}
```

---

## Logging

Application logs are generated using **Serilog** and written to the `Logs` directory.

---

## Running with Docker

### Build the Docker Image

From the solution root directory:

```bash
docker build -t fileprocessor-api -f FileProcessor.API/Dockerfile .
```

### Run the Docker Container

```bash
docker run -d -p 8080:8080 --name fileprocessor fileprocessor-api
```

### Access Swagger

```
http://localhost:8080/swagger
```

---

## Docker Database Notes

The application uses a SQLite database located in the `Database` folder.

When running in Docker:

- The application creates the `Database` directory automatically if it does not exist.
- Entity Framework Core migrations initialize the database on startup (if configured).
- If you want the SQLite database to persist after removing the container, mount the `Database` folder as a Docker volume.

Example:

```bash
docker run -d \
  -p 8080:8080 \
  -v ${PWD}/Database:/app/Database \
  --name fileprocessor \
  fileprocessor-api
```

---

## Project Structure

```text
FileProcessor.sln
│
├── FileProcessor.API
├── FileProcessor.Application
├── FileProcessor.Domain
└── FileProcessor.Infrastructure
```

---
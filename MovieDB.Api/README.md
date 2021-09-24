# MovieDB API

---

API for the MovieDB App. Manage movies, concerts and theaters you have seen or visited.

## Installation

Copy the *appsettings.secret.example.json* file, fill in the right values and then set them as user-secrets:
```
cp appsettings.secret.example.json appsettings.secret.json
type .\appsettings.secret.json | dotnet user-secrets set
```

Run the migrations:
```
dotnet ef database update
```

## Usage

Start the app:
```
dotnet run
```

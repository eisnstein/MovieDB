# MovieDB API

---

API for the MovieDB App. Manage movies, concerts and theaters you have seen or visited.

## Installation

Set values for your Email and a secret key in `appsettings.Development.json`. In `production` use ENV variables:

```sh
AppSettings_Secret=should-be-at-least-256-bits-long
AppSettings_SmtpUser=username
AppSettings_SmtpPass=password
...
```

Run the migrations:

```sh
dotnet ef database update
```

## Usage

Start the app:

```sh
dotnet run
```

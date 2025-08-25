FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY . .

RUN dotnet restore
RUN dotnet publish -c Release -o out -r linux-arm64 --no-self-contained /p:PublishSingleFile=true

FROM mcr.microsoft.com/dotnet/aspnet:9.0-bookworm-slim-arm64v8
WORKDIR /app

RUN mkdir Data

COPY --from=build /app/out .

EXPOSE 8081

CMD ["./MovieDB.Api"]

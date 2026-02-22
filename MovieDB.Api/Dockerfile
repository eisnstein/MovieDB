FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY . .

RUN dotnet restore
RUN dotnet publish -c Release -o out -r linux-arm64 --no-self-contained /p:PublishSingleFile=true

FROM mcr.microsoft.com/dotnet/aspnet:10.0.3-noble-chiseled-arm64v8
WORKDIR /app

RUN mkdir Data

COPY --from=build /app/out .

EXPOSE 8081

CMD ["./MovieDB.Api"]

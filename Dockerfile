# Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore src/GavGavich.Web/GavGavich.Web.csproj
RUN dotnet publish src/GavGavich.Web/GavGavich.Web.csproj -c Release -o /app/publish --no-restore

# Run
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["sh", "-c", "dotnet GavGavich.Web.dll --urls http://0.0.0.0:${PORT:-8080}"]

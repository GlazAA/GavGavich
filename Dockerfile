# Build (alpine — быстрее на бесплатном Render)
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

COPY src/GavGavich.Domain/GavGavich.Domain.csproj src/GavGavich.Domain/
COPY src/GavGavich.Application/GavGavich.Application.csproj src/GavGavich.Application/
COPY src/GavGavich.Infrastructure/GavGavich.Infrastructure.csproj src/GavGavich.Infrastructure/
COPY src/GavGavich.Web/GavGavich.Web.csproj src/GavGavich.Web/
RUN dotnet restore src/GavGavich.Web/GavGavich.Web.csproj

COPY src/ src/
RUN dotnet publish src/GavGavich.Web/GavGavich.Web.csproj -c Release -o /app/publish /p:UseAppHost=false --no-restore

# Run
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080
# Render передаёт PORT; локально по умолчанию 8080
ENTRYPOINT ["sh", "-c", "dotnet GavGavich.Web.dll --urls http://0.0.0.0:${PORT:-8080}"]

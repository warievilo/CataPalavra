FROM mcr.microsoft.com/dotnet/sdk:6.0.202-bullseye-slim-arm64v8 AS build-env
WORKDIR /app

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0.4-bullseye-slim-arm64v8
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "CataPalavra.Web.dll"]
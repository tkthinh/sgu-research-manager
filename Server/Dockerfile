# ---------------- Build Stage ----------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy sln and csproj files separately for better Docker layer caching
COPY Api.sln ./
COPY Application/Application.csproj Application/
COPY Domain/Domain.csproj Domain/
COPY Infrastructure/Infrastructure.csproj Infrastructure/
COPY WebApi/WebApi.csproj WebApi/

# Clear NuGet configuration and restore dependencies
RUN dotnet nuget locals all --clear
RUN dotnet restore "Api.sln" --disable-parallel

# Copy the rest of the source code
COPY . .

# Build and publish WebApi project
RUN dotnet publish "WebApi/WebApi.csproj" -c Release -o /app/publish

# ---------------- Runtime Stage ----------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copy the published output from build stage
COPY --from=build /app/publish .

# Set environment variables if needed
ENV ASPNETCORE_URLS=http://+:80

# Expose the port the app will run on
EXPOSE 80

ENTRYPOINT ["dotnet", "WebApi.dll"]
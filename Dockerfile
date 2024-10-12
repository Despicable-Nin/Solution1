# Base image for SQL Server
FROM mcr.microsoft.com/mssql/server:2022-latest AS sqlserver

# SQL Server environment variables
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=someThingComplicated1234

# Expose SQL Server port
EXPOSE 1433

# Base image for .NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build and publish the .NET project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only the BlazorApp2 project file to avoid unnecessary rebuilds
COPY ["BlazorApp2/BlazorApp2.csproj", "BlazorApp2/"]
RUN dotnet restore "BlazorApp2/BlazorApp2.csproj"

# Copy the entire project
COPY BlazorApp2/ BlazorApp2/
WORKDIR /src/BlazorApp2
RUN dotnet build "BlazorApp2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazorApp2.csproj" -c Release -o /app/publish

# Final image with published application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Configuration for Serilog with Seq
ENV Seq__ServerUrl=http://seq:5341
# ENV Seq__ApiKey=your_api_key_here 
# Optional, set if you want to secure API access

ENTRYPOINT ["dotnet", "BlazorApp2.dll"]

# ---------------- Build Stage ----------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything and restore dependencies
COPY . .
RUN dotnet restore 

# build
RUN dotnet publish -c Release -o /out

# ---------------- Runtime Stage ----------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Create data directory for SQLite
RUN mkdir -p /app/data

# Copy published files
COPY --from=build /out ./

# Copy your existing SQLite DB file into the container
COPY ExpenseTracker.db /app/data/ExpenseTracker.db

# Set ASP.NET Core environment to Docker
EXPOSE 8080

ENTRYPOINT ["dotnet", "ExpenseTracker.dll"]

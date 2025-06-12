# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ../CampusLearn.API/CampusLearn.API.sln ./
COPY ../CampusLearn.API/CampusLearn.API.csproj ./
COPY ../CampusLearn.Bootstrap/CampusLearn.Bootstrap.csproj ../CampusLearn.Bootstrap/
COPY ../CampusLearn.DataModel/CampusLearn.DataModel.csproj ../CampusLearn.DataModel/
COPY ../CampusLearn.DataLayer/CampusLearn.DataLayer.csproj ../CampusLearn.DataLayer/
COPY ../CampusLearn.Services/CampusLearn.Services.csproj ../CampusLearn.Services/
COPY ../CampusLearn.APITest/CampusLearn.APITest.csproj ../CampusLearn.APITest/

# Restore
#RUN dotnet restore CampusLearn.API.sln
RUN dotnet restore CampusLearn.API.sln --disable-parallel --interactive --no-cache

# Copy all source
COPY . ./
COPY ../CampusLearn.Bootstrap ../CampusLearn.Bootstrap
COPY ../CampusLearn.DataModel ../CampusLearn.DataModel
COPY ../CampusLearn.DataLayer ../CampusLearn.DataLayer
COPY ../CampusLearn.Services ../CampusLearn.Services


#  Set working directory to API project folder
WORKDIR /src/CampusLearn.API


# Publish
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

EXPOSE 8080
ENTRYPOINT ["dotnet", "CampusLearn.API.dll"]

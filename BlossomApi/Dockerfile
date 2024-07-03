# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the official ASP.NET Core build image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BlossomApi/BlossomApi.csproj", "BlossomApi/"]
RUN dotnet restore "BlossomApi/BlossomApi.csproj"
COPY . .
WORKDIR "/src/BlossomApi"
RUN dotnet build "BlossomApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlossomApi.csproj" -c Release -o /app/publish

# Use the base image to run the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Install curl and procps (for ps)
RUN apt-get update && apt-get install -y curl procps

ENTRYPOINT ["dotnet", "BlossomApi.dll"]

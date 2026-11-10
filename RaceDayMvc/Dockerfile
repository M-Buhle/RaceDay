# 1. Use the official ASP.NET Core runtime as a base image for the final app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
# 2. Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# 3. Copy the csproj and restore dependencies
COPY ["RaceDayMvc.csproj", "./"]
RUN dotnet restore "RaceDayMvc.csproj"
# 4. Copy the rest of the files and build
COPY . .
RUN dotnet build "RaceDayMvc.csproj" -c Release -o /app/build
# 5. Publish the app
FROM build AS publish
RUN dotnet publish "RaceDayMvc.csproj" -c Release -o /app/publish /p:UseAppHost=false
# 6. Copy the published files to the final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RaceDayMvc.dll"]
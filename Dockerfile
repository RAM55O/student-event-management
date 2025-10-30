# Stage 1: Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Stage 2: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["StudentEventManagement/StudentEventManagement.csproj", "StudentEventManagement/"]
RUN dotnet restore "StudentEventManagement/StudentEventManagement.csproj"
COPY . .
WORKDIR "/src/StudentEventManagement"
RUN dotnet build "StudentEventManagement.csproj" -c Release -o /app/build

# Stage 3: Publish
FROM build AS publish
RUN dotnet publish "StudentEventManagement.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 4: Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudentEventManagement.dll"]

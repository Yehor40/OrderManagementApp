# ----- Build Stage -----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["OrderManagementApp.csproj", "./"]
RUN dotnet restore "OrderManagementApp.csproj"

COPY . .
WORKDIR "/src"
RUN dotnet build "OrderManagementApp.csproj" -c Release -o /app/build

# ----- Publish Stage -----
FROM build AS publish
RUN dotnet publish "OrderManagementApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ----- Final Stage -----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "OrderManagementApp.dll"]


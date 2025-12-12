FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["OrderManagementApp/OrderManagementApp.csproj", "OrderManagementApp/"]
RUN dotnet restore "OrderManagementApp/OrderManagementApp.csproj"

COPY . .
WORKDIR "/src/OrderManagementApp"
RUN dotnet build "OrderManagementApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderManagementApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "OrderManagementApp.dll"]


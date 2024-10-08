FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8082
ENV ASPNETCORE_URLS=http://+:8082

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

COPY NuGet.Config ./

WORKDIR /src
COPY ["src/TMS.Security.Contracts/TMS.Security.Contracts.csproj", "TMS.Security.Contracts/"]
COPY ["src/TMS.Security.Core/TMS.Security.Core.csproj", "TMS.Security.Core/"]
COPY ["src/TMS.Security.Service/TMS.Security.Service.csproj", "TMS.Security.Service/"]
COPY ["src/TMS.Security.DataAccess/TMS.Security.DataAccess.csproj", "TMS.Security.DataAccess/"]
COPY ["src/TMS.Security.UseCases/TMS.Security.UseCases.csproj", "TMS.Security.UseCases/"]
COPY ["src/TMS.Security.Integration/TMS.Security.Integration.csproj", "TMS.Security.Integration/"]

WORKDIR /src/TMS.Security.Service
RUN dotnet restore "TMS.Security.Service.csproj"

COPY . .
RUN dotnet build "TMS.Security.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TMS.Security.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY ["src/TMS.Security.Service/appsettings.json", "/app/appsettings.json"]

ENTRYPOINT ["dotnet", "TMS.Security.Service.dll"]
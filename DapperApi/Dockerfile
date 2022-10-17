#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DapperApi/DapperApi.csproj", "DapperApi/"]
COPY ["DTOs/DTOs.csproj", "DTOs/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Infastructure/Infastructure.csproj", "Infastructure/"]
RUN dotnet restore "DapperApi/DapperApi.csproj"
COPY . .
WORKDIR "/src/DapperApi"
RUN dotnet build "DapperApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DapperApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DapperApi.dll"]
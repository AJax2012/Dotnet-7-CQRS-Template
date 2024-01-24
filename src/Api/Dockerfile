FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SourceName.Api/SourceName.Api.csproj", "SourceName.Api/"]
RUN dotnet restore "SourceName.Api/SourceName.Api.csproj"
COPY . .
WORKDIR "/src/SourceName.Api"
RUN dotnet build "SourceName.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SourceName.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SourceName.Api.dll"] 

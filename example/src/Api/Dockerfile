FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ExampleProject.Api/ExampleProject.Api.csproj", "ExampleProject.Api/"]
RUN dotnet restore "ExampleProject.Api/ExampleProject.Api.csproj"
COPY . .
WORKDIR "/src/ExampleProject.Api"
RUN dotnet build "ExampleProject.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExampleProject.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExampleProject.Api.dll"] 

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["oodb-mongo-server.csproj", "."]
RUN dotnet restore "./oodb-mongo-server.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "oodb-mongo-server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "oodb-mongo-server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "oodb-mongo-server.dll"]
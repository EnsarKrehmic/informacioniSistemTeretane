FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["InformacioniSistemTeretane.csproj", "."]
RUN dotnet restore "./InformacioniSistemTeretane.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "InformacioniSistemTeretane.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InformacioniSistemTeretane.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InformacioniSistemTeretane.dll"]
ENV ASPNETCORE_URLS=http://*:80
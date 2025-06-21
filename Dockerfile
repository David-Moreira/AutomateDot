FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

COPY . /source

WORKDIR /source


RUN dotnet restore src/AutomateDot/AutomateDot.csproj
RUN dotnet publish src/AutomateDot/AutomateDot.csproj -o /app --configuration Release

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=build /app .

ENTRYPOINT ["dotnet", "AutomateDot.dll"]

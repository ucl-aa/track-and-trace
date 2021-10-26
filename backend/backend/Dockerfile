FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /src

COPY *.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app



FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine

COPY --from=build /app .

ENTRYPOINT ["dotnet", "backend.dll"]
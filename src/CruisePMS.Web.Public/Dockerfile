FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base

WORKDIR /app
COPY . .

ENTRYPOINT ["dotnet", "CruisePMS.Web.Public.dll"]
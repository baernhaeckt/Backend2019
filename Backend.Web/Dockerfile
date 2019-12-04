FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Backend.Web/Backend.Web.csproj", "Backend.Web/"]
COPY ["Backend.Infrastructure/Backend.Infrastructure.csproj", "Backend.Infrastructure/"]
COPY ["Backend.Core/Backend.Core.csproj", "Backend.Core/"]
RUN dotnet restore "Backend.Web/Backend.Web.csproj"
COPY . .
WORKDIR "/src/Backend.Web"
RUN dotnet build "Backend.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Backend.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Backend.Web.dll"]
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ClubEventManagementAPI/ClubEventManagementAPI.csproj", "ClubEventManagementAPI/"]
COPY ["Club-Event-Management.Core/ApplicationCore.csproj", "Club-Event-Management.Core/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "ClubEventManagementAPI/ClubEventManagementAPI.csproj"
COPY . .
WORKDIR "/src/ClubEventManagementAPI"
RUN dotnet build "ClubEventManagementAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClubEventManagementAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClubEventManagementAPI.dll"]
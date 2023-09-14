FROM mcr.microsoft.com/dotnet/runtime:6.0-focal AS base
WORKDIR /app
EXPOSE 80/udp

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["GameServer/GameServer.csproj", "GameServer/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "GameServer/GameServer.csproj"

COPY . .

WORKDIR "/src/"
RUN dotnet build "GameServer/GameServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameServer/GameServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "GameServer.dll"]

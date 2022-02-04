FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DiscordToTelegramSender/DiscordToTelegramSender.csproj", "DiscordToTelegramSender/"]
RUN dotnet restore "DiscordToTelegramSender/DiscordToTelegramSender.csproj"
COPY . .
WORKDIR "/src/DiscordToTelegramSender"
RUN dotnet build "DiscordToTelegramSender.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DiscordToTelegramSender.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiscordToTelegramSender.dll"]

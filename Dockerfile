FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY MoodTrackerAPI /src
RUN dotnet build -o /bin -r linux-musl-x64


FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine

WORKDIR /app

COPY --from=build /bin ./

ENTRYPOINT ["dotnet", "MoodTrackerAPI.dll"]
FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src", "."]
WORKDIR "/src/AsyncProjectionStressTest.AutoClientTester"
RUN dotnet restore
RUN dotnet build "AsyncProjectionStressTest.AutoClientTester.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AsyncProjectionStressTest.AutoClientTester.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AsyncProjectionStressTest.AutoClientTester.dll"]

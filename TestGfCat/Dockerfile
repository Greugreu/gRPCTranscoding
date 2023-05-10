FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TestGfCat/TestGfCat.csproj", "TestGfCat/"]
RUN dotnet restore "TestGfCat/TestGfCat.csproj"
COPY . .
WORKDIR "/src/TestGfCat"
RUN dotnet build "TestGfCat.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestGfCat.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestGfCat.dll"]

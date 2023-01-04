FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OutBoxPattern/OutBoxPattern.csproj", "OutBoxPattern/"]
RUN dotnet restore "OutBoxPattern/OutBoxPattern.csproj"
COPY . .
WORKDIR "/src/OutBoxPattern"
RUN dotnet build "OutBoxPattern.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OutBoxPattern.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OutBoxPattern.dll"]

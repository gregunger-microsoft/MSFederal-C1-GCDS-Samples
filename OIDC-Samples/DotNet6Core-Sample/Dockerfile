FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
COPY ./myCertificates/DoD_PKE_CA_chain.pem /etc/ssl/certs/DoD_PKE_CA_chain.pem
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["gcds-greg.csproj", "."]
RUN dotnet restore "./gcds-greg.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "gcds-greg.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "gcds-greg.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "gcds-greg.dll"]
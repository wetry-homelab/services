#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/Kubernox.Service/Kubernox.Service.csproj", "src/Kubernox.Service/"]
RUN dotnet restore "src/Kubernox.Service/Kubernox.Service.csproj"
COPY . .
WORKDIR "/src/src/Kubernox.Service"
RUN dotnet build "Kubernox.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kubernox.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kubernox.Service.dll"]

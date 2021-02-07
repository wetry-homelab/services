#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/Datacenter.Worker/Datacenter.Worker.csproj", "src/Datacenter.Worker/"]
RUN dotnet restore "src/Datacenter.Worker/Datacenter.Worker.csproj"
COPY . .
WORKDIR "/src/src/Datacenter.Worker"
RUN dotnet build "Datacenter.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Datacenter.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Datacenter.Worker.dll"]

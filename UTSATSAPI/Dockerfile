#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UTSATSAPI/UTSATSAPI.csproj", "UTSATSAPI/"]
COPY ["UTSATSAPI.Helpers/UTSATSAPI.Helpers.csproj", "UTSATSAPI.Helpers/"]
COPY ["UTSATSAPI.Models/UTSATSAPI.Models.csproj", "UTSATSAPI.Models/"]
COPY ["UTSATSAPI.Repositories/UTSATSAPI.Repositories.csproj", "UTSATSAPI.Repositories/"]
RUN dotnet restore "./UTSATSAPI/UTSATSAPI.csproj"
COPY . .
WORKDIR "/src/UTSATSAPI"
RUN dotnet build "./UTSATSAPI.csproj" -c %BUILD_CONFIGURATION% -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UTSATSAPI.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UTSATSAPI.dll"]
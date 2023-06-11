# See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Depending on the operating system of the host machines(s) that will build or run the containers,
# the image specified in the FROM statement may need to be changed.
# For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY *.sln .
COPY Online_Marketplace/Online_Marketplace.API/Online_Marketplace.API.csproj Online_Marketplace/Online_Marketplace.API/
COPY Online_Marketplace/Online_Marketplace.BLL/Online_Marketplace.BLL.csproj Online_Marketplace/Online_Marketplace.BLL/
COPY Online_Marketplace/Online_Marketplace.Contracts/Online_Marketplace.Contracts.csproj Online_Marketplace/Online_Marketplace.Contracts/
COPY Online_Marketplace/Online_Marketplace.DAL/Online_Marketplace.DAL.csproj Online_Marketplace/Online_Marketplace.DAL/
COPY Online_Marketplace/Online_Marketplace.Logger/Online_Marketplace.Logger.csproj Online_Marketplace/Online_Marketplace.Logger/
COPY Online_Marketplace/Online_Marketplace.Shared/Online_Marketplace.Shared.csproj Online_Marketplace/Online_Marketplace.Shared/
COPY Online_Marketplace/Online_Marketplace.Presentation/Online_Marketplace.Presentation.csproj Online_Marketplace/Online_Marketplace.Presentation/
RUN dotnet restore "Online_Marketplace/Online_Marketplace.API/Online_Marketplace.API.csproj"
COPY . .
WORKDIR "/src/Online_Marketplace/Online_Marketplace.API"
RUN dotnet build "Online_Marketplace.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Online_Marketplace.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Online_Marketplace.API.dll"]


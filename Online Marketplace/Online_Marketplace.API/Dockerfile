#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Online Marketplace/Online_Marketplace.API/Online_Marketplace.API.csproj", "Online Marketplace/Online_Marketplace.API/"]
COPY ["Online Marketplace/Online_Marketplace.BLL/Online_Marketplace.BLL.csproj", "Online Marketplace/Online_Marketplace.BLL/"]
COPY ["Online Marketplace/Online_Marketplace.Contracts/Online_Marketplace.Contracts.csproj", "Online Marketplace/Online_Marketplace.Contracts/"]
COPY ["Online Marketplace/Online_Marketplace.DAL/Online_Marketplace.DAL.csproj", "Online Marketplace/Online_Marketplace.DAL/"]
COPY ["Online Marketplace/Online_Marketplace.Logger/Online_Marketplace.Logger.csproj", "Online Marketplace/Online_Marketplace.Logger/"]
COPY ["Online Marketplace/Online_Marketplace.Shared/Online_Marketplace.Shared.csproj", "Online Marketplace/Online_Marketplace.Shared/"]
COPY ["Online Marketplace/Online_Marketplace.Presentation/Online_Marketplace.Presentation.csproj", "Online Marketplace/Online_Marketplace.Presentation/"]
RUN dotnet restore "Online Marketplace/Online_Marketplace.API/Online_Marketplace.API.csproj"
COPY . .
WORKDIR "/src/Online Marketplace/Online_Marketplace.API"
RUN dotnet build "Online_Marketplace.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Online_Marketplace.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["Online Marketplace/Online_Marketplace.API/Resources", "Resources/"] # Copy the Resources directory to the image
ENTRYPOINT ["dotnet", "Online_Marketplace.API.dll"]
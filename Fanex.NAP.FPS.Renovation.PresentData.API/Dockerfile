#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-nanoserver-1803 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-nanoserver-1803 AS build
WORKDIR /src
COPY ["Fanex.NAP.FPS.Renovation.PresentData.API/Fanex.NAP.FPS.Renovation.PresentData.API.csproj", "Fanex.NAP.FPS.Renovation.PresentData.API/"]
RUN dotnet restore "Fanex.NAP.FPS.Renovation.PresentData.API/Fanex.NAP.FPS.Renovation.PresentData.API.csproj"
COPY . .
WORKDIR "/src/Fanex.NAP.FPS.Renovation.PresentData.API"
RUN dotnet build "Fanex.NAP.FPS.Renovation.PresentData.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Fanex.NAP.FPS.Renovation.PresentData.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Fanex.NAP.FPS.Renovation.PresentData.API.dll"]
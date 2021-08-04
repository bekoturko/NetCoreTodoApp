#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["NetCoreTodoApp.Business/NetCoreTodoApp.Business.csproj", "Business/"]
COPY ["NetCoreTodoApp.Business.Abstract/NetCoreTodoApp.Business.Abstract.csproj", "Business.Abstract/"]
COPY ["NetCoreTodoApp.Data/NetCoreTodoApp.Data.csproj", "NetCoreTodoApp.Data/"]
COPY ["NetCoreTodoApp.Data.Abstract/NetCoreTodoApp.Data.Abstract.csproj", "Data.Abstract/"]
COPY ["NetCoreTodoApp.Framework/NetCoreTodoApp.Framework.csproj", "Framework/"]
COPY ["NetCoreTodoApp.Framework.Abstract/NetCoreTodoApp.Framework.Abstract.csproj", "Framework.Abstract/"]
COPY ["NetCoreTodoApp.Model/NetCoreTodoApp.Model.csproj", "Model/"]
COPY ["NetCoreTodoApp.NUnitTests/NetCoreTodoApp.NUnitTests.csproj", "NUnitTests/"]
COPY ["NetCoreTodoApp/NetCoreTodoApp.csproj", "NetCoreTodoApp/"]
RUN dotnet restore "NetCoreTodoApp/NetCoreTodoApp.csproj"
COPY . .
WORKDIR "/src/NetCoreTodoApp"
RUN dotnet build "NetCoreTodoApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NetCoreTodoApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetCoreTodoApp.dll"]
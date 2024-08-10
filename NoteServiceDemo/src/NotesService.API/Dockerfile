FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=https://+:8081;http://+:8080
EXPOSE 8080
EXPOSE 8081

ENV ConnectionStrings__AuthenticationDB="Server=host.docker.internal,1433;Database=Auth;User Id=sa;Password=tempStrong!PASS4;MultipleActiveResultSets=true;TrustServerCertificate=True"
ENV ConnectionStrings__DefaultConnection="Server=host.docker.internal,1433;Database=Notes;User Id=sa;Password=tempStrong!PASS4;MultipleActiveResultSets=true;TrustServerCertificate=True"

# copy .csproj and restore
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["src/NotesService.API/NotesService.API.csproj", "NotesService.API/"]
COPY ["src/NotesService.API.Abstractions/NotesService.API.Abstractions.csproj", "NotesService.API.Abstractions/"]
COPY ["src/NotesService.API.DataAccess/NotesService.API.DataAccess.csproj", "NotesService.API.DataAccess/"]
RUN dotnet restore "NotesService.API/NotesService.API.csproj" --disable-parallel

# copy the rest and build
COPY src/ .
WORKDIR "/src/NotesService.API"
RUN dotnet build "NotesService.API.csproj" -c Release -o /app/build

# publish 
FROM build AS publish
RUN dotnet publish "NotesService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# copy published items into new folder
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "NotesService.API.dll"]

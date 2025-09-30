# Dockerfile

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia o arquivo .csproj para a raiz /src e renomeia se necessário.
# Esta linha está correta e não precisa mudar.
COPY ["API/API.csproj", "API.csproj"]

# MUDANÇA 1: O restore agora procura o arquivo direto na pasta /src
RUN dotnet restore "API.csproj"
 
COPY . .
WORKDIR "/src"
# MUDANÇA 2: O build agora usa o caminho relativo correto
RUN dotnet build "API/API.csproj" -c Release -o /app/build

FROM build AS publish
# MUDANÇA 3: O publish agora usa o caminho relativo correto
RUN dotnet publish "API/API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 8080
 
ENTRYPOINT ["dotnet", "API.dll"]

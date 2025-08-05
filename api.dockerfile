FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src


COPY ["API/API.csproj", "API/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["Webhook.Job/Webhook.Job.csproj", "Webhook.Job/"] # O job tamb�m � uma depend�ncia


RUN dotnet restore "API/API.csproj"


COPY . .


WORKDIR "/src/API"
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .


ENV ASPNETCORE_URLS=http://+:8080
 
ENTRYPOINT ["dotnet", "API.dll"]
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["API/API.csproj", "API/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["Webhook.Job/Webhook.Job.csproj", "Webhook.Job/"]

RUN dotnet restore "Webhook.Job/Webhook.Job.csproj"

COPY . .

WORKDIR "/src/Webhook.Job"
RUN dotnet publish "Webhook.Job.csproj" -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .


ENTRYPOINT ["dotnet", "Webhook.Job.dll"]
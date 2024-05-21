FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["commerce-tracker-v2.csproj", "commerce-tracker-v2/"]
RUN dotnet restore "commerce-tracker-v2/commerce-tracker-v2.csproj"
WORKDIR "/src/commerce-tracker-v2"
COPY . .
RUN dotnet build "commerce-tracker-v2.csproj" -c Release -o /app/build



FROM build as publish
RUN dotnet publish "commerce-tracker-v2.csproj" -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .


ENTRYPOINT [ "dotnet", "commerce-tracker-v2.dll" ]

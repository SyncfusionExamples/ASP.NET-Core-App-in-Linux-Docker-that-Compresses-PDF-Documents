FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["PDFCompression/PDFCompression.csproj", "PDFCompression/"]
RUN dotnet restore "PDFCompression/PDFCompression.csproj"
COPY . .
WORKDIR "/src/PDFCompression"
RUN dotnet build "PDFCompression.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "PDFCompression.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PDFCompression.dll"]
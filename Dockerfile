# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY QLKS.API.csproj .
RUN dotnet restore QLKS.API.csproj

COPY . .
RUN dotnet publish QLKS.API.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# ✅ Sửa EXPOSE để phù hợp cổng Render sẽ sử dụng
EXPOSE 10000
ENTRYPOINT ["dotnet", "QLKS.API.dll"]

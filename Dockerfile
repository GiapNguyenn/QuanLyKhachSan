# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY QuanLyKhachSan/QuanLyKhachSan.csproj QuanLyKhachSan/
RUN dotnet restore QuanLyKhachSan/QuanLyKhachSan.csproj

COPY . .
WORKDIR /src/QuanLyKhachSan
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "QuanLyKhachSan.dll"]
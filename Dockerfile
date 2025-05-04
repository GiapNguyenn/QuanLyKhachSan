# ----- Build Stage -----
  FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
  WORKDIR /src
  
  # Copy csproj and restore
  COPY QuanLyKhachSan/QuanLyKhachSan.csproj QuanLyKhachSan/
  RUN dotnet restore "QuanLyKhachSan/QuanLyKhachSan.csproj"
  
  # Copy the rest and publish
  COPY . .
  WORKDIR /src/QuanLyKhachSan
  RUN dotnet publish -c Release -o /app/publish
  
  # ----- Runtime Stage -----
  FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
  WORKDIR /app
  COPY --from=build /app/publish .
  ENTRYPOINT ["dotnet", "QuanLyKhachSan.dll"]
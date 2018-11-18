FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 60596
EXPOSE 44385

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["DotNetCoreStreaming/DotNetCoreStreaming.csproj", "DotNetCoreStreaming/"]
RUN dotnet restore "DotNetCoreStreaming/DotNetCoreStreaming.csproj"
COPY . .
WORKDIR "/src/DotNetCoreStreaming"
RUN dotnet build "DotNetCoreStreaming.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DotNetCoreStreaming.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DotNetCoreStreaming.dll"]
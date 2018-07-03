FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 50366
EXPOSE 44393

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ChatServerCore/ChatServerCore.csproj ChatServerCore/
RUN dotnet restore ChatServerCore/ChatServerCore.csproj
COPY . .
WORKDIR /src/ChatServerCore
RUN dotnet build ChatServerCore.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ChatServerCore.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ChatServerCore.dll"]

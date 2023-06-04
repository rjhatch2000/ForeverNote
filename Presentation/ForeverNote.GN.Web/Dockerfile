FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["ForeverNote.Web/ForeverNote.Web.csproj", "ForeverNote.Web/"]
COPY ["ForeverNote.Web.Framework/ForeverNote.Web.Framework.csproj", "ForeverNote.Web.Framework/"]
COPY ["ForeverNote.Core/ForeverNote.Core.csproj", "ForeverNote.Core/"]
COPY ["ForeverNote.Services/ForeverNote.Services.csproj", "ForeverNote.Services/"]
COPY ["ForeverNote.Api/ForeverNote.Api.csproj", "ForeverNote.Api/"]
RUN dotnet restore "ForeverNote.Web/ForeverNote.Web.csproj"
COPY . .
WORKDIR "/src/ForeverNote.Web"
RUN dotnet build "ForeverNote.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ForeverNote.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "ForeverNote.Web.dll"]
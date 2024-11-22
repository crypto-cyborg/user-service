FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY ./UserService.sln ./UserService.sln
COPY ./UserService.API/UserService.API.csproj ./UserService.API/
COPY ./UserService.Core/UserService.Core.csproj ./UserService.Core/
COPY ./UserService.Application/UserService.Application.csproj ./UserService.Application/
COPY ./UserService.Persistence/UserService.Persistence.csproj ./UserService.Persistence/

RUN dotnet restore

COPY ./UserService.API/ ./UserService.API/
COPY ./UserService.Core/ ./UserService.Core/
COPY ./UserService.Application/ ./UserService.Application/
COPY ./UserService.Persistence/ ./UserService.Persistence/

RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build-env /app/out .

ENTRYPOINT [ "dotnet", "UserService.API.dll" ]


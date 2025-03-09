FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

EXPOSE 8080
EXPOSE 50051

COPY ./UserService.sln ./UserService.sln
COPY ./UserService.API/UserService.API.csproj ./UserService.API/
COPY ./UserService.Core/UserService.Core.csproj ./UserService.Core/
COPY ./UserService.Application/UserService.Application.csproj ./UserService.Application/
COPY ./UserService.Persistence/UserService.Persistence.csproj ./UserService.Persistence/
COPY ./UserService.Infrastructure/UserService.Infrastructure.csproj ./UserService.Infrastructure/

RUN dotnet restore

COPY ./UserService.API/ ./UserService.API/
COPY ./UserService.Core/ ./UserService.Core/
COPY ./UserService.Application/ ./UserService.Application/
COPY ./UserService.Persistence/ ./UserService.Persistence/
COPY ./UserService.Infrastructure/ ./UserService.Infrastructure/

RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build-env /app/out .

ENTRYPOINT [ "dotnet", "UserService.API.dll" ]


#Build Stage
FROM microsoft/dotnet:2.2-sdk AS build-env

WORKDIR /workdir

COPY ./User.Common.Contracts ./User.Common.Contracts/
COPY ./User.API ./User.API/

RUN dotnet restore ./User.API/User.API.csproj
RUN dotnet publish ./User.API/User.API.csproj -o /publish

FROM microsoft/dotnet:2.2-aspnetcore-runtime
COPY --from=build-env /publish /publish
WORKDIR /publish
EXPOSE 5000
ENTRYPOINT ["dotnet", "User.API.dll"]
#Build Stage
FROM microsoft/dotnet:2.2-sdk AS build-env

WORKDIR /workdir

COPY ./User.Common.Contracts ./User.Common.Contracts/
COPY ./User.Activation.Consumer ./User.Activation.Consumer/

RUN dotnet restore ./User.Activation.Consumer/User.Activation.Consumer.csproj
RUN dotnet publish ./User.Activation.Consumer/User.Activation.Consumer.csproj -o /publish

FROM microsoft/dotnet:2.2-aspnetcore-runtime
COPY --from=build-env /publish /publish
WORKDIR /publish
EXPOSE 5000
ENTRYPOINT ["dotnet", "User.Activation.Consumer.dll"]
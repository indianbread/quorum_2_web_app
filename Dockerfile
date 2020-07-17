FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./

#ls-la between steps to see what's happening on the container
RUN ls -al

RUN dotnet restore

RUN ls -al

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out
RUN ls -al

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "kata_frameworkless_basic_web_application.dll"]
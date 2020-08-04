FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

# copy sln and csproj files into the image
COPY *.sln .
COPY src/kata_frameworkless_web_app/*.csproj ./src/kata_frameworkless_web_app/
COPY src/kata_frameworkless_basic_web_application.tests/*.csproj ./src/kata_frameworkless_basic_web_application.tests/
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet build

#run the unit tests
FROM build AS test
WORKDIR /app/src/kata_frameworkless_basic_web_application.tests
RUN dotnet test

FROM build AS publish
WORKDIR /app/src/kata_frameworkless_web_app
RUN dotnet publish -c Release -o publish


# Load Image for deploy image
FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS base
WORKDIR /app
EXPOSE 8080


# Build runtime image
FROM base AS runtime
WORKDIR /app
COPY --from=publish /app/src/kata_frameworkless_web_app/publish ./ 
ENTRYPOINT ["dotnet", "kata_frameworkless_web_app.dll"]
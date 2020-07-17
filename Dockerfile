FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

# copy sln and csproj files into the image
COPY *.sln ./
COPY kata_frameworkless_web_app/*.csproj ./kata_frameworkless_web_app/
COPY kata_frameworkless_basic_web_application.tests/*.csproj ./kata_frameworkless_basic_web_application.tests/
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet build

FROM build AS testrunner
WORKDIR /app/kata_frameworkless_basic_web_application.tests
CMD ["dotnet", "test"]

#run the unit tests
FROM build AS test
WORKDIR /app/kata_frameworkless_basic_web_application.tests
RUN dotnet test

#RUN dotnet test
RUN dotnet publish -c Release -o out
#RUN ls -al

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "kata_frameworkless_basic_web_application.dll"]
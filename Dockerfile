FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
RUN curl "https://awscli.amazonaws.com/awscli-exe-linux-x86_64.zip" -o "awscliv2.zip" && apt-get update && apt-get install unzip && unzip awscliv2.zip && ./aws/install
COPY /ops/scripts/dynamo_db_local_install.sh ./
RUN sh dynamo_db_local_install.sh
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
RUN java -Djava.library.path=/DynamoDBLocal_lib -jar DynamoDBLocal.jar -sharedDb & && dotnet test

FROM build AS publish
WORKDIR /app/src/kata_frameworkless_web_app
RUN dotnet publish -c Release -o publish


# Load Image for deploy image
FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS base
WORKDIR /app
RUN curl "https://awscli.amazonaws.com/awscli-exe-linux-x86_64.zip" -o "awscliv2.zip" && apt-get update && apt-get install unzip && unzip awscliv2.zip && ./aws/install

# Build runtime image
FROM base AS runtime
WORKDIR /app
COPY --from=publish /app/src/kata_frameworkless_web_app/publish ./
COPY ops/scripts/startup.sh /startup.sh 
ENTRYPOINT ["/startup.sh"]
EXPOSE 8080
#ENTRYPOINT ["dotnet", "kata_frameworkless_web_app.dll"]
# ["/bin/bash"]
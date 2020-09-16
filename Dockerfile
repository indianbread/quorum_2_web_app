FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

COPY *.sln .
COPY src/kata_frameworkless_web_app/*.csproj ./src/kata_frameworkless_web_app/
COPY src/kata.users.domain/*.csproj ./src/kata.users.domain/
COPY src/kata.users.repositories/*.csproj ./src/kata.users.repositories/
COPY src/kata.users.shared/*.csproj ./src/kata.users.shared/
RUN dotnet restore

COPY . ./
RUN dotnet build

FROM build AS publish
WORKDIR /app/src/kata_frameworkless_web_app
RUN dotnet publish -c Release -o publish

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

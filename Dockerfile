FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS base
WORKDIR /app
RUN curl "https://awscli.amazonaws.com/awscli-exe-linux-x86_64.zip" -o "awscliv2.zip" && apt-get update && apt-get install unzip && unzip awscliv2.zip && ./aws/install

# Build runtime image
FROM base AS runtime
WORKDIR /app
COPY /publish/ ./
COPY ops/scripts/startup.sh ./
ENTRYPOINT ["/app/startup.sh"]
EXPOSE 8080

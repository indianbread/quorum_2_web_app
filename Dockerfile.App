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

#!/usr/bin/env bash

echo "--- :docker: Docker build"
docker build -f Dockerfile.Test -t nhan-frameworkless-app:$1 .
echo "--- :docker: Docker run"
docker run nhan-frameworkless-app:$1
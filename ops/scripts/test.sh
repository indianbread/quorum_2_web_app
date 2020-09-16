#!/usr/bin/env bash

echo "--- :docker: Build image for testing"
docker build -f Dockerfile.Test -t nhan-frameworkless-app:$1 .
echo "--- :docker: Running tests"
docker run nhan-frameworkless-app:$1
#!/usr/bin/env bash
set -euo pipefail
echo "--- :docker: Docker build"
docker build -f Dockerfile -t 741922737521.dkr.ecr.ap-southeast-2.amazonaws.com/nhan-frameworkless-app:latest .
echo "--- :docker: Push image to ECR"
docker push 741922737521.dkr.ecr.ap-southeast-2.amazonaws.com/nhan-frameworkless-app:latest
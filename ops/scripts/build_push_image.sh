#!/usr/bin/env bash
set -euo pipefail
echo "--- :docker: Docker build"
docker build -f Dockerfile --target publish -t 741922737521.dkr.ecr.ap-southeast-2.amazonaws.com/nhan-frameworkless-app:$1 .
echo "--- :docker: Push image to ECR"
docker push 741922737521.dkr.ecr.ap-southeast-2.amazonaws.com/nhan-frameworkless-app:$1
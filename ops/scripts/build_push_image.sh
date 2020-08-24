#!/usr/bin/env bash
set -euo pipefail
echo "-- :docker: log in to ECR--"
aws ecr get-login-password --region ap-southeast-2 | docker login --username AWS --password-stdin 741922737521.dkr.ecr.ap-southeast-2.amazonaws.com
echo "--- :docker: Docker build"
docker build -f Dockerfile -t 741922737521.dkr.ecr.ap-southeast-2.amazonaws.com/nhan-frameworkless-app:$1 .
echo "--- :docker: Push image to ECR"
docker push 741922737521.dkr.ecr.ap-southeast-2.amazonaws.com/nhan-frameworkless-app:$1
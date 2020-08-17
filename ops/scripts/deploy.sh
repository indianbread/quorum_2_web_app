#!/usr/bin/env bash
set -euo pipefail
echo "login to ecr"
docker login -u AWS -p $(aws ecr get-login-password --region ap-southeast-2) 741922737521.dkr.ecr.ap-southeast-2.amazonaws.com
echo "--- :rocket: Jupiter Deploy"
kubectl apply -n fma -f ./ops/deploy/jupiter_deploy.yml
kubectl rollout status --watch=true deployment/nhan-frameworkless-web-app-deployment -n fma
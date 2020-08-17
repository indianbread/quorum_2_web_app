#!/usr/bin/env bash
set -euo pipefail
echo "login to ecr"
aws ecr get-login-password --region ap-southeast-2 | docker login --username AWS --password-stdin 741922737521.dkr.ecr.ap-southeast-2.amazonaws.com
echo "--- :rocket: Jupiter Deploy"
kubectl apply -n fma -f ./ops/deploy/jupiter_deploy.yml
kubectl rollout status --watch=true deployment/nhan-frameworkless-web-app-deployment -n fma
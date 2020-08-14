#!/usr/bin/env bash
set -euo pipefail
echo "--- :rocket: Jupiter Deploy"
kubectl apply -n fma -f ./ops/deploy/jupiter_deploy.yml
kubectl rollout status --watch=true deployment/nhan-frameworkless-web-app-deployment -n fma
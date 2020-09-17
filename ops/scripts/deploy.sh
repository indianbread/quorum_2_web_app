#!/usr/bin/env bash
set -euo pipefail
echo "--- :rocket: Jupiter Deploy"
ktmpl ./ops/deploy/jupiter_deploy.yml -p imageTag $1  | kubectl apply -f -
kubectl rollout status --watch=true deployment/nhan-frameworkless-web-app-deployment -n fma
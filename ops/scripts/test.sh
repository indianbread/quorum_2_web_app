#!/usr/bin/env bash
set -euo pipefail
echo "--- :docker: Docker build"
docker build -f Dockerfile --target test -t nhan-web-app .
echo "--- :docker: Docker run test"
docker run --rm nhan-web-app
#!/usr/bin/env bash
set -euo pipefail
echo "--- :docker: Docker build"
docker build -f Dockerfile --target testrunner -t nhan-web-app:latest .
echo "--- :docker: Docker run test"
docker run --rm nhan-web-app:latest
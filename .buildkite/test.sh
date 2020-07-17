#!/usr/bin/env bash
set -euo pipefail
echo "--- :docker: Docker compose build"
docker build --target testrunner -t nhan-web-app:latest ../.
echo "--- :docker: Docker compose run test"
docker run --rm nhan-web-app:latest
#!/usr/bin/env bash
set -euo pipefail
echo "--- :docker: Docker compose build"
docker-compose build build
echo "--- test"
docker-compose run build
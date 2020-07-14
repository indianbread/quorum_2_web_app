#!/usr/bin/env bash
set -euo pipefail
echo "--- :docker: Docker compose build"
docker-compose build build-bfs
echo "--- test"
docker-compose run build-bfs
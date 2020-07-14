#!/usr/bin/env bash

set -euo pipefail
echo "--- :docker: Docker compose build"
docker-compose build test
echo "--- test"
docker-compose run test
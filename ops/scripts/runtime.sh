#!/usr/bin/env bash
set -euo pipefail
echo "--- :docker: Docker build"
docker build -f Dockerfile -t nhan-web-app:latest .
echo "--- :docker: Docker run runtime"
docker run -it --rm -p 8080:8080 nhan-web-app:latest
#docker run --publish 80:80 --detach --name nhan-app nhan-web-app:latest  

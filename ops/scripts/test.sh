#!/usr/bin/env bash
#set -euo pipefail
echo "Setting up Dynamodb Local"
sh ./dynamodb_local/dynamodb_local_startup.sh &
echo "Running tests"
dotnet test
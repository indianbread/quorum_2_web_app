#!/usr/bin/env bash

if [[ -z "${SECRET_NAME}" ]]; then
  echo "Setting secret user"
  export SECRET_USER=Nhan
fi
export DB_ENV=local
export AWS_ACCESS_KEY_ID=X 
export AWS_SECRET_ACCESS_KEY=X
sh ./dynamodb_local/dynamodb_local_startup.sh
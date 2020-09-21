#!/bin/bash

eval $(aws s3 cp s3://nhan-secret-name/secret_name.txt - | sed 's/^/export /')
if [[ -z "${SECRET_NAME}" ]]; then
  echo "Setting secret user"
  export SECRET_USER="Nhan"
fi
dotnet kata_frameworkless_web_app.dll

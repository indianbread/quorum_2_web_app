#!/bin/bash
# Check that the environment variable has been set correctly

eval $(aws s3 cp s3://nhan-secret-name/secret_name.txt - | sed 's/^/export /')
aws s3 cp s3://nhan-secret-name/secret_name.txt -
#env
dotnet kata_frameworkless_web_app.dll

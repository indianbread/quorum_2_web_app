#!/bin/bash


eval $(aws s3 cp s3://nhan-secret-name/secret_name.txt - | sed 's/^/export /')
dotnet kata_frameworkless_web_app.dll

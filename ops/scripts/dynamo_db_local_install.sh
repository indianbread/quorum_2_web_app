#!/usr/bin/env bash

echo "Installing Java"
apt update
apt install default-jdk
echo "Setting up Dynamodb Local"
wget http://dynamodb-local.s3-website-us-west-2.amazonaws.com/dynamodb_local_latest.tar.gz
gunzip dynamodb_local_latest.tar.gz
mkdir ./dynamolocal
tar -jxvf dynamodb_local_latest.tar -c ./dynamolocal
cd ./dynamolocal
java -Djava.library.path=./DynamoDBLocal_lib/ -jar DynamoDBLocal.jar
dynamo
#set -euo pipefail
#!/usr/bin/env bash

echo "Installing Java"
apt-get update
apt-get install default-jdk -y
which java
echo "Installing Dynamodb Local"
wget http://dynamodb-local.s3-website-us-west-2.amazonaws.com/dynamodb_local_latest.tar.gz
gunzip dynamodb_local_latest.tar.gz
mkdir ./dynamolocal
tar -xf dynamodb_local_latest.tar -C ./dynamolocal


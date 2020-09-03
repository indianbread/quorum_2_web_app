#set this environment variable: WEBAPP_ENVIRONMENT=DEVELOPMENT
echo "Starting DynamoDB development database"
#docker-compose -f ./dynamodb_local/docker-compose-dynamodb-local.yml up -d
docker build -f ./dynamodb_local/Dockerfile.DynamodbLocal -t dynamodblocal:latest .
docker run -d -p 8000:8000 dynamodblocal:latest
#up command = create and start containers
sleep 3 #allow database to start up
echo "Checking if User table exists"
aws dynamodb describe-table --table-name NhanUser --endpoint-url http://localhost:8000
if [ "$?" != "0" ]; then
  echo "Creating User table"
  aws dynamodb create-table --table-name NhanUser --cli-input-json file://dynamodb_local/tables/user.json --endpoint-url http://localhost:8000
fi
echo "Starting up Dynamodb local database"
java -Djava.library.path=/dynamolocal/DynamoDBLocal_lib -jar /dynamolocal/DynamoDBLocal.jar -sharedDb &
sleep 3 #allow database to start up
echo "Checking if User table exists"
aws dynamodb describe-table --table-name NhanUser --endpoint-url http://localhost:8000
if [ "$?" != "0" ]; then
  echo "Creating User table"
  aws dynamodb create-table --table-name NhanUser --cli-input-json file://dynamodb_local/tables/user.json --endpoint-url http://localhost:8000
fi
echo "Adding test user to database"
aws dynamodb put-item \
    --table-name NhanUser \
    --item '{
      "Id": {"S": "1"},
      "FirstName": {"S": "Bob"}    }' \
    --endpoint-url http://localhost:8000
echo "Running tests"
dotnet test
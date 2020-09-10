echo "Starting up Dynamodb local database"
java -Djava.library.path=/dynamolocal/DynamoDBLocal_lib -jar /dynamolocal/DynamoDBLocal.jar -sharedDb &
sleep 3 #allow database to start up
echo "Checking if User table exists"
AWS_ACCESS_KEY_ID=X AWS_SECRET_ACCESS_KEY=X aws dynamodb describe-table --table-name NhanUser --endpoint-url http://localhost:8000 --region ap-southeast-2
if [ "$?" != "0" ]; then
  echo "Creating User table"
  AWS_ACCESS_KEY_ID=X AWS_SECRET_ACCESS_KEY=X aws dynamodb create-table --table-name NhanUser --cli-input-json file://dynamodb_local/tables/user.json --endpoint-url http://localhost:8000 --region ap-southeast-2
fi
echo "Adding test user to database"
AWS_ACCESS_KEY_ID=X AWS_SECRET_ACCESS_KEY=X aws dynamodb put-item \
    --table-name NhanUser \
    --item '{
      "Id": {"S": "1"},
      "FirstName": {"S": "Bob"}    }' \
    --endpoint-url http://localhost:8000 --region ap-southeast-2
echo "Running tests"
dotnet test
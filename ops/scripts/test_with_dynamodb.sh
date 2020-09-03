java -Djava.library.path=/dynamolocal/DynamoDBLocal_lib -jar /dynamolocal/DynamoDBLocal.jar -sharedDb &
sleep 3 #allow database to start up
echo "Checking if User table exists"
aws dynamodb describe-table --table-name NhanUser --endpoint-url http://localhost:8000
if [ "$?" != "0" ]; then
  echo "Creating User table"
  aws dynamodb create-table --table-name NhanUser --cli-input-json file://dynamodb_local/tables/user.json --endpoint-url http://localhost:8000 --output text
fi 
dotnet test
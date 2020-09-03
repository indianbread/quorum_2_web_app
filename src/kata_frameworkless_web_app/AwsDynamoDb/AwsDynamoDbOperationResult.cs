namespace kata_frameworkless_web_app.AwsDynamoDb
{
    public class AwsDynamoDbOperationResult
    {
        public OperationResult result { get; set; }
        public string ErrorMessage { get; set; }

        public static AwsDynamoDbOperationResult Success()
        {
            return new AwsDynamoDbOperationResult()
            {
                result = OperationResult.Succeeded,
                
            };
        }

        public static AwsDynamoDbOperationResult Failed(string errorMessage)
        {
            return new AwsDynamoDbOperationResult()
            {
                result = OperationResult.Failed,
                ErrorMessage = "Error: " + errorMessage
            };
        }
        
    }
}
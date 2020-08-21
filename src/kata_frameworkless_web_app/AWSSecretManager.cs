using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Amazon.Util;
using Newtonsoft.Json.Linq;


namespace kata_frameworkless_web_app
{
    public class AwsSecretManager
    {
        public static string GetSecret()
        {
            var secretName = Environment.GetEnvironmentVariable("SECRET_NAME");
            var secret = "";

            var memoryStream = new MemoryStream();

            IAmazonSecretsManager
                client = new AmazonSecretsManagerClient(RegionEndpoint
                    .APSoutheast2); //access key id & secret access key

            var request = new GetSecretValueRequest {SecretId = secretName, VersionStage = "AWSCURRENT"};

            GetSecretValueResponse response = null;

            try
            {
                response = client.GetSecretValueAsync(request).GetAwaiter().GetResult();
                if (response.SecretString != null)
                {
                    secret = response.SecretString;
                }
                else
                {
                    memoryStream = response.SecretBinary;
                    var reader = new StreamReader(memoryStream);
                    secret = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
                }

                var secretObject = JObject.Parse(secret);
                return ((secretObject["SecretUser"]) ?? string.Empty).Value<string>();
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
            
        }
    }
}
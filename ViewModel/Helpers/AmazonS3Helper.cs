using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.Runtime.Internal;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Util;
using static System.Net.WebRequestMethods;

namespace EvernoteClone.ViewModel.Helpers
{
    public static class AmazonS3Helper
    {
        public static async Task<string> uploadToS3(string objectName, string content)
        {
            // Create credentials object to access bucket.
            AWSCredentials credentials = new BasicAWSCredentials(Keys.accessKey, Keys.secretKey);

            // create an instance of IAmazonS3 class using USEast1 endpoint.
            using (IAmazonS3 client = new AmazonS3Client(credentials, RegionEndpoint.USEast1))
            {
                var putRequest = new PutObjectRequest
                {
                    BucketName = Keys.bucketName,
                    Key = $"{objectName}.rtf",
                    FilePath = Path.Combine([Environment.CurrentDirectory, "notes", $"{objectName}.rtf"])
                };
                PutObjectResponse response = await client.PutObjectAsync(putRequest);
            }

            // Return the file location of the newly created object.
            return $"{Keys.bucketUrl}{objectName}.rtf";
        }

        public static async Task<string> retrieveFromS3(string objectName)
        {
            // Create a filename for the local file.
            string fileName = Path.Combine([Environment.CurrentDirectory, "notes", $"{objectName}.rtf"]);

            // Create credentials object to access bucket and an instance of IAmazonS3 class using USEast1 endpoint.
            AWSCredentials credentials = new BasicAWSCredentials(Keys.accessKey, Keys.secretKey);
            using (IAmazonS3 client = new AmazonS3Client(credentials, RegionEndpoint.USEast1))
            {
                var getRequest = new GetObjectRequest
                {
                    BucketName = Keys.bucketName,
                    Key = $"{objectName}.rtf"
                };

                try
                {
                    // Get the object in the database and write it to the local computer for parsing.
                    GetObjectResponse response = await client.GetObjectAsync(getRequest);
                    await response.WriteResponseStreamToFileAsync(fileName, false, CancellationToken.None);
                }
                catch (AmazonS3Exception s3Exception)
                {
                    Console.WriteLine($"Error saving {objectName}: {s3Exception.Message}");
                }
            }

            return fileName;
        }

        public static async Task<bool> deleteFromS3(string objectName)
        {
            // Create a filename for the local file.
            string fileName = Path.Combine([Environment.CurrentDirectory, "notes", $"{objectName}.rtf"]);

            // Create credentials object to access bucket and an instance of IAmazonS3 class using USEast1 endpoint.
            AWSCredentials credentials = new BasicAWSCredentials(Keys.accessKey, Keys.secretKey);
            using (IAmazonS3 client = new AmazonS3Client(credentials, RegionEndpoint.USEast1))
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = Keys.bucketName,
                    Key = $"{objectName}.rtf"
                };

                try
                {
                    // Get the object in the database and write it to the local computer for parsing.
                    DeleteObjectResponse response = await client.DeleteObjectAsync(deleteObjectRequest);
                    if (response.HttpStatusCode == HttpStatusCode.OK)
                        return true;
                    else
                        return false;
                }
                catch (AmazonS3Exception s3Exception)
                {
                    Console.WriteLine($"Error saving {objectName}: {s3Exception.Message}");
                    return false;
                }
            }
        }
    }
}

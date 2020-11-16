using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using RpgGame.Models;

namespace RpgGame.Helpers
{
    public interface IAWSS3BucketHelper
    {
        Task<bool> UploadFile(Stream inputStream, string fileName);
        Task<ListVersionsResponse> FilesList();
        Task<Stream> GetFile(string key);
        Task<bool> DeleteFile(string key);
    }
    
    public class AWSS3BucketHelper : IAWSS3BucketHelper
    {
        private readonly IAmazonS3 _amazonS3;
        private readonly ServiceConfiguration _settings;

        public AWSS3BucketHelper(IAmazonS3 amazonS3, IOptions<ServiceConfiguration> settings)
        {
            _amazonS3 = amazonS3;
            _settings = settings.Value;
        }
        
        public async Task<bool> UploadFile(Stream inputStream, string fileName)
        {
            try
            {
                PutObjectRequest request = new PutObjectRequest()
                {
                    InputStream = inputStream,
                    BucketName = _settings.AWSS3.BucketName,
                    Key = fileName
                };
                PutObjectResponse response = await _amazonS3.PutObjectAsync(request);
                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ListVersionsResponse> FilesList()
        {
            return await _amazonS3.ListVersionsAsync(_settings.AWSS3.BucketName);
        }

        public async Task<Stream> GetFile(string key)
        {
            GetObjectResponse response = await _amazonS3.GetObjectAsync(_settings.AWSS3.BucketName, key);
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return response.ResponseStream;
            }
            return null;
        }

        public async Task<bool> DeleteFile(string key)
        {
            try
            {
                DeleteObjectResponse response = await _amazonS3.DeleteObjectAsync(_settings.AWSS3.BucketName, key);
                if (response.HttpStatusCode == HttpStatusCode.NoContent)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3.Model;
using Microsoft.VisualBasic;
using RpgGame.Helpers;
using RpgGame.Models.enums;

namespace RpgGame.Services
{
    public class AWSS3FileService : IAWSS3FileService
    {
        private readonly IAWSS3BucketHelper _awss3BucketHelper;

        public AWSS3FileService(IAWSS3BucketHelper awss3BucketHelper)
        {
            _awss3BucketHelper = awss3BucketHelper;
        }
        
        public async Task<bool> UploadFile(UploadFileName uploadFileName)
        {
            try
            {
                var path = Path.Combine("Files", uploadFileName.ToString() + ".png");
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    string fileExtension = Path.GetExtension(path);
                    string fileName = $"{DateTime.Now.Ticks}{fileExtension}";
                    return await _awss3BucketHelper.UploadFile(fileStream, fileName);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<string>> FilesList()
        {
            try
            {
                ListVersionsResponse listVersionsResponse = await _awss3BucketHelper.FilesList();
                return listVersionsResponse.Versions.Select(c => c.Key).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Stream> GetFile(string key)
        {
            try
            {
                Stream fileStream = await _awss3BucketHelper.GetFile(key);
                if (fileStream == null)
                {
                    throw new Exception("File not found");
                }

                return fileStream;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> UpdateFile(UploadFileName uploadFileName, string key)
        {
            try
            {
                var path = Path.Combine("Files", uploadFileName.ToString() + ".png");
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    return await _awss3BucketHelper.UploadFile(fileStream, key);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DeleteFile(string key)
        {
            try
            {
                return await _awss3BucketHelper.DeleteFile(key);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
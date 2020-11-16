using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using RpgGame.Models.enums;

namespace RpgGame.Services
{
    public interface IAWSS3FileService
    {
        Task<bool> UploadFile(UploadFileName uploadFileName);
        Task<List<string>> FilesList();
        Task<Stream> GetFile(string key);
        Task<bool> UpdateFile(UploadFileName uploadFileName, string key);
        Task<bool> DeleteFile(string key);
    }
}
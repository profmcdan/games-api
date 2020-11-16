using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RpgGame.Helpers;
using RpgGame.Models.enums;
using RpgGame.Services;

namespace RpgGame.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/files")]
    public class FilesController : ControllerBase
    {
        private readonly IAWSS3FileService _awss3FileService;
        public FilesController(IAWSS3FileService awss3FileService)
        {
            _awss3FileService = awss3FileService;
        }

        [HttpPost]
        public async Task<ActionResult> UploadFile(UploadFileName uploadFileName)
        {
            var result = await _awss3FileService.UploadFile(uploadFileName);
            return Ok(new {isSuccess = result});
        }

        [HttpGet]
        public async Task<ActionResult> GetFileList()
        {
            var result = await _awss3FileService.FilesList();
            return Ok(result);
        }

        [HttpGet("{fileName}")]
        public async Task<ActionResult> GetFile(string fileName)
        {
            try
            {
                var result = await _awss3FileService.GetFile(fileName);
                return Ok(result);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
        

        [HttpPut("{fileName}")]
        public async Task<ActionResult> UpdateFile(UploadFileName uploadFileName, string fileName)
        {
            var result = await _awss3FileService.UpdateFile(uploadFileName, fileName);
            return Ok(new {isSuccess = result});
        }

        [HttpDelete("{fileName}")]
        public async Task<ActionResult> DeleteFile(string fileName)
        {
            var result = await _awss3FileService.DeleteFile(fileName);
            return Ok(new {isSuccess = result});
        }
        
    }
}
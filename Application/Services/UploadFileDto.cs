using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class UploadFileDto
{
    public IFormFile File { get; set; }
}
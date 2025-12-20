using Film.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Film.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProfileController(IUserProfileService _profileService) : ControllerBase
{
    [HttpPut("upload-image")]
    [Authorize]
    public async Task<IActionResult> UploadProfileImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Invalid file.");

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profile-images");
        Directory.CreateDirectory(uploadsFolder);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var relativePath = $"/profile-images/{fileName}";
        // Save to DB
        await _profileService.UpdateProfileImageAsync(userId, relativePath);

        return Ok(new { imageUrl = relativePath });
    }

}

using AutoMapper;
using Film.Application.Dtos;
using Film.Application.Interfaces;
using Film.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Film.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewsController(IReviewService reviewService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllReviewsForMovie(int movieId)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateReview([FromBody] CreateReviewDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(userId == null)
        {
            return Unauthorized();
        }

    
        return CreatedAtAction(nameof(GetAllReviewsForMovie), new { review = review.Id }, addedReview);
    }
}

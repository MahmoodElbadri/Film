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

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateReview([FromBody] CreateReviewDto dto)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdString != null)
        {
            int userId;
            if (int.TryParse(userIdString, out userId))
            {
                var reviewDto = await reviewService.AddReviewAsync(dto, userId);
                return Ok(reviewDto);
            }
            else
            {
                return BadRequest("Invalid user ID");
            }
        }
        else
        {
            return Unauthorized();
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllReviewsForMovie(int movieId)
    {
        var reviewDto = await reviewService.GetReviewsByMovieIdAsync(movieId);
        return Ok(reviewDto);
    }

    [HttpGet("movie/{movieId:int}/average")]
    public async Task<IActionResult> GetAverageRatingForMovie(int movieId)
    {
        var averageRating = await reviewService.GetAverageRatingForMovieAsync(movieId);
        return Ok(averageRating);
    }

}

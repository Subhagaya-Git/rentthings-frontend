using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using RentThings.Api.Services.Azure; // 👈 මේක අනිවාර්යයෙන්ම තියෙන්න ඕනේ!

namespace RentThings.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListingController : ControllerBase
    {
        private readonly IImageValidationService _imageValidationService;

        public ListingController(IImageValidationService imageValidationService)
        {
            _imageValidationService = imageValidationService;
        }

        private static readonly List<dynamic> MockListings = new()
        {
            new { Id = 1, Title = "Sony Alpha Camera", Latitude = 7.2906, Longitude = 80.6337 },
            new { Id = 2, Title = "Camping Tent", Latitude = 7.2950, Longitude = 80.6400 },
            new { Id = 3, Title = "Drone DJI Mini", Latitude = 6.9271, Longitude = 79.8612 }
        };

        [HttpGet("nearby")]
        public IActionResult GetNearby(double userLat, double userLng, double maxDistanceKm = 10)
        {
            var nearbyItems = new List<dynamic>();
            foreach (var listing in MockListings)
            {
                double dist = CalculateHaversine(userLat, userLng, listing.Latitude, listing.Longitude);
                if (dist <= maxDistanceKm) nearbyItems.Add(listing);
            }
            return Ok(nearbyItems);
        }

        private double CalculateHaversine(double lat1, double lon1, double lat2, double lon2)
        {
            double R = 6371;
            double dLat = (lat2 - lat1) * Math.PI / 180;
            double dLon = (lon2 - lon1) * Math.PI / 180;
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateListing([FromForm] IFormFile file, [FromForm] string title)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Image is required.");

            using (var stream = file.OpenReadStream())
            {
                var validationResult = await _imageValidationService.ValidateListingImageAsync(stream);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new { message = "Invalid Image! " + validationResult.DetectedItems });
                }
            }

            return Ok(new { message = "Listing created successfully!" });
        }
    }
}
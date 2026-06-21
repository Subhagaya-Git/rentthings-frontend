using Microsoft.EntityFrameworkCore;
using RentThings.Api.Data;
using RentThings.Api.Models;

namespace RentThings.Api.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(RentThingsDbContext context)
    {
        if (await context.Users.AnyAsync()) return;

        var admin = new User
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Email = "admin@rentthings.com",
            FirstName = "Alex",
            LastName = "Admin",
            Role = UserRole.Admin,
            TrustScore = 100,
            TrustLevel = TrustLevel.Platinum,
            IsVerified = true,
            Location = "Colombo"
        };

        var owner = new User
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Email = "owner@rentthings.com",
            FirstName = "Olivia",
            LastName = "Owner",
            Role = UserRole.Owner,
            TrustScore = 82,
            TrustLevel = TrustLevel.Platinum,
            IsVerified = true,
            Location = "Kandy"
        };

        var renter = new User
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Email = "renter@rentthings.com",
            FirstName = "Ryan",
            LastName = "Renter",
            Role = UserRole.Renter,
            TrustScore = 68,
            TrustLevel = TrustLevel.Gold,
            IsVerified = true,
            Location = "Colombo",
            Phone = "+94771234567"
        };

        var owner2 = new User
        {
            Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            Email = "owner2@rentthings.com",
            FirstName = "Nimal",
            LastName = "Perera",
            Role = UserRole.Owner,
            TrustScore = 45,
            TrustLevel = TrustLevel.Silver,
            IsVerified = true,
            Location = "Galle"
        };

        context.Users.AddRange(admin, owner, renter, owner2);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var availEnd = today.AddDays(90);

        var listings = new List<Listing>
        {
            new()
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                OwnerId = owner.Id,
                Title = "Sony A7 IV Mirrorless Camera Kit",
                Description = "Professional full-frame camera with 28-70mm lens, extra battery, and carrying case. Perfect for events and photography projects.",
                Category = "Cameras",
                PricePerDay = 8500m,
                Deposit = 50000m,
                Location = "Kandy, Central Province",
                City = "Kandy",
                State = "Central",
                Latitude = 7.2906,
                Longitude = 80.6337,
                Status = ListingStatus.Active,
                AverageRating = 4.9,
                ReviewCount = 47,
                IsFeatured = true,
                Images =
                [
                    new ListingImage { BlobUrl = "https://images.unsplash.com/photo-1516035069371-29a1b244cc32?w=600&q=80", IsPrimary = true, SortOrder = 0 },
                    new ListingImage { BlobUrl = "https://images.unsplash.com/photo-1606983340126-99ab4feaa64a?w=600&q=80", SortOrder = 1 }
                ]
            },
            new()
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                OwnerId = owner.Id,
                Title = "DeWalt 20V Power Tool Combo Set",
                Description = "Complete drill/driver kit with impact driver, two batteries, charger, and tool bag. Ideal for home projects.",
                Category = "Power Tools",
                PricePerDay = 3500m,
                Deposit = 15000m,
                Location = "Kandy, Central Province",
                City = "Kandy",
                State = "Central",
                Latitude = 7.2906,
                Longitude = 80.6337,
                Status = ListingStatus.Active,
                AverageRating = 4.7,
                ReviewCount = 23,
                IsFeatured = true,
                Images =
                [
                    new ListingImage { BlobUrl = "https://images.unsplash.com/photo-1504148455328-c376907d081c?w=600&q=80", IsPrimary = true, SortOrder = 0 }
                ]
            },
            new()
            {
                Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                OwnerId = owner.Id,
                Title = "4-Person Camping Tent & Gear Bundle",
                Description = "Waterproof tent, sleeping bags, camp stove, and lantern. Everything you need for a weekend adventure.",
                Category = "Camping Gear",
                PricePerDay = 4500m,
                Deposit = 20000m,
                Location = "Nuwara Eliya, Central Province",
                City = "Nuwara Eliya",
                State = "Central",
                Latitude = 6.9497,
                Longitude = 80.7891,
                Status = ListingStatus.Active,
                AverageRating = 4.8,
                ReviewCount = 31,
                Images =
                [
                    new ListingImage { BlobUrl = "https://images.unsplash.com/photo-1504280390367-361c6d9f38f4?w=600&q=80", IsPrimary = true, SortOrder = 0 }
                ]
            },
            new()
            {
                Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                OwnerId = owner2.Id,
                Title = "Epson 4K Projector + 120\" Screen",
                Description = "Bright 4K projector with portable screen and HDMI cables. Great for movie nights and presentations.",
                Category = "Event Equipment",
                PricePerDay = 6500m,
                Deposit = 35000m,
                Location = "Colombo, Western Province",
                City = "Colombo",
                State = "Western",
                Latitude = 6.9271,
                Longitude = 79.8612,
                Status = ListingStatus.Active,
                AverageRating = 4.6,
                ReviewCount = 18,
                IsFeatured = true,
                Images =
                [
                    new ListingImage { BlobUrl = "https://images.unsplash.com/photo-1478720568477-152d9b164e26?w=600&q=80", IsPrimary = true, SortOrder = 0 }
                ]
            },
            new()
            {
                Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                OwnerId = owner2.Id,
                Title = "JBL PartyBox 310 Bluetooth Speaker",
                Description = "Powerful portable speaker with light show. Perfect for parties, weddings, and outdoor events.",
                Category = "Speakers",
                PricePerDay = 4000m,
                Deposit = 18000m,
                Location = "Galle, Southern Province",
                City = "Galle",
                State = "Southern",
                Latitude = 6.0535,
                Longitude = 80.2210,
                Status = ListingStatus.Active,
                AverageRating = 4.5,
                ReviewCount = 12,
                Images =
                [
                    new ListingImage { BlobUrl = "https://images.unsplash.com/photo-1545454675-3531b543be5d?w=600&q=80", IsPrimary = true, SortOrder = 0 }
                ]
            },
            new()
            {
                Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                OwnerId = owner.Id,
                Title = "Dyson V15 Detect Vacuum",
                Description = "Premium cordless vacuum with laser dust detection. Deep clean carpets and hard floors effortlessly.",
                Category = "Home Appliances",
                PricePerDay = 2500m,
                Deposit = 12000m,
                Location = "Colombo, Western Province",
                City = "Colombo",
                State = "Western",
                Latitude = 6.9271,
                Longitude = 79.8612,
                Status = ListingStatus.Inactive,
                AverageRating = 4.9,
                ReviewCount = 56,
                Images =
                [
                    new ListingImage { BlobUrl = "https://images.unsplash.com/photo-1558618666-fcd25c85cd64?w=600&q=80", IsPrimary = true, SortOrder = 0 }
                ]
            },
            new()
            {
                Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                OwnerId = owner2.Id,
                Title = "DJI Mini 3 Pro Drone",
                Description = "Compact drone with 4K camera. Pending admin review.",
                Category = "Electronics",
                PricePerDay = 5500m,
                Deposit = 40000m,
                Location = "Colombo, Western Province",
                City = "Colombo",
                State = "Western",
                Status = ListingStatus.PendingReview,
                Images =
                [
                    new ListingImage { BlobUrl = "https://images.unsplash.com/photo-1473968512647-3e447244af8f?w=600&q=80", IsPrimary = true, SortOrder = 0 }
                ]
            },
            new()
            {
                Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                OwnerId = owner.Id,
                Title = "Suspicious Listing - Flagged Item",
                Description = "This listing was flagged for review.",
                Category = "Electronics",
                PricePerDay = 100m,
                Deposit = 50m,
                Location = "Colombo",
                Status = ListingStatus.Flagged,
                Images =
                [
                    new ListingImage { BlobUrl = "https://images.unsplash.com/photo-1498049794561-7780f7231661?w=600&q=80", IsPrimary = true, SortOrder = 0 }
                ]
            }
        };

        foreach (var listing in listings.Where(l => l.Status == ListingStatus.Active))
        {
            for (var d = today; d <= availEnd; d = d.AddDays(1))
                listing.Availability.Add(new ListingAvailability { Date = d, IsAvailable = true });
        }

        context.Listings.AddRange(listings);

        var cameraListing = listings[0];
        var toolListing = listings[1];

        var rentals = new List<Rental>
        {
            new()
            {
                Id = Guid.Parse("e1111111-1111-1111-1111-111111111111"), // r වෙනුවට e ආදේශ කරන ලදී
                ListingId = cameraListing.Id,
                RenterId = renter.Id,
                StartDate = today.AddDays(3),
                EndDate = today.AddDays(6),
                Status = RentalStatus.Requested,
                TotalPrice = 8500m * 4,
                DepositAmount = 50000m,
                Message = "Need for a wedding shoot in Kandy."
            },
            new()
            {
                Id = Guid.Parse("e2222222-2222-2222-2222-222222222222"),
                ListingId = toolListing.Id,
                RenterId = renter.Id,
                StartDate = today.AddDays(-10),
                EndDate = today.AddDays(-7),
                Status = RentalStatus.HandedOver,
                TotalPrice = 3500m * 4,
                DepositAmount = 15000m,
                ApprovedAt = DateTime.UtcNow.AddDays(-12)
            },
            new()
            {
                Id = Guid.Parse("e3333333-3333-3333-3333-333333333333"),
                ListingId = listings[3].Id,
                RenterId = renter.Id,
                StartDate = today.AddDays(-30),
                EndDate = today.AddDays(-27),
                Status = RentalStatus.Reviewed,
                TotalPrice = 6500m * 4,
                DepositAmount = 35000m,
                CompletedAt = DateTime.UtcNow.AddDays(-25)
            },
            new()
            {
                Id = Guid.Parse("e4444444-4444-4444-4444-444444444444"),
                ListingId = listings[4].Id,
                RenterId = renter.Id,
                StartDate = today.AddDays(-60),
                EndDate = today.AddDays(-58),
                Status = RentalStatus.Rejected,
                TotalPrice = 4000m * 3,
                DepositAmount = 18000m,
                OwnerNotes = "Item unavailable during requested dates."
            },
            new()
            {
                Id = Guid.Parse("e5555555-5555-5555-5555-555555555555"),
                ListingId = cameraListing.Id,
                RenterId = renter.Id,
                StartDate = today.AddDays(-90),
                EndDate = today.AddDays(-87),
                Status = RentalStatus.Reviewed,
                TotalPrice = 8500m * 4,
                DepositAmount = 50000m,
                CompletedAt = DateTime.UtcNow.AddMonths(-3)
            }
        };

        context.Rentals.AddRange(rentals);

        context.Reviews.Add(new Review
        {
            RentalId = rentals[2].Id,
            ReviewerId = renter.Id,
            RevieweeId = owner2.Id,
            Rating = 5,
            Comment = "Projector was perfect for our event!",
            IsOwnerReview = false
        });

        context.UserReports.AddRange(
            new UserReport
            {
                ReporterId = renter.Id,
                ReportedListingId = listings[7].Id,
                Reason = "Suspicious pricing",
                Description = "Price seems too low for this item."
            },
            new UserReport
            {
                ReporterId = admin.Id,
                ReportedUserId = owner2.Id,
                Reason = "Late return",
                Description = "Previous renter reported late return.",
                IsResolved = true
            }
        );

        context.Notifications.AddRange(
            new Notification
            {
                UserId = owner.Id,
                Type = NotificationType.BookingRequest,
                Title = "New rental request",
                Message = "Ryan Renter requested your Sony A7 IV camera kit.",
                ActionUrl = "/owner/dashboard"
            },
            new Notification
            {
                UserId = renter.Id,
                Type = NotificationType.System,
                Title = "Welcome to RentThings!",
                Message = "Browse thousands of items to rent near you.",
                ActionUrl = "/search"
            }
        );

        await context.SaveChangesAsync();
    }
}
using BulkyBook.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess.Data;

public static class DataSeeder
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        SeedCategories(modelBuilder);
        SeedProducts(modelBuilder);
    }

    private static void SeedCategories(ModelBuilder modelBuilder)
    {
        var categories = new List<Category>()
        {
            new Category {Id = 1 , Name = "Action" , DisplayOrder = 1},
            new Category {Id = 2 , Name = "SciFi" , DisplayOrder = 2},
            new Category {Id = 3 , Name = "History" , DisplayOrder = 3}
        };

        modelBuilder.Entity<Category>().HasData(categories);
    }

    private static void SeedProducts(ModelBuilder modelBuilder)
    {
        var products = new List<Product>
    {
        new Product
        {
            Id = 1,
            Title = "Fortune of Time",
            Author = "Billy Spark",
            Description = "A gripping tale of time travel and the quest for eternal life. Join the protagonist on an epic journey across different eras, where every decision counts.",
            ISBN = "SWD9999001",
            ListPrice = 99,
            Price = 90,
            Price50 = 85,
            Price100 = 80,
            CategoryId = 1,
            ImageUrl = ""
        },
        new Product
        {
            Id = 2,
            Title = "Dark Skies",
            Author = "Nancy Hoover",
            Description = "A chilling story of survival in a world on the brink of collapse. Will humanity overcome its darkest hour or succumb to the forces of nature?",
            ISBN = "CAW777777701",
            ListPrice = 40,
            Price = 30,
            Price50 = 25,
            Price100 = 20,
            CategoryId = 2,
            ImageUrl = ""
        },
        new Product
        {
            Id = 3,
            Title = "Vanish in the Sunset",
            Author = "Julian Button",
            Description = "In a secluded town, mysteries unfold as a sudden disappearance leads to secrets being uncovered. Can the truth be found before it's too late?",
            ISBN = "RITO5555501",
            ListPrice = 55,
            Price = 50,
            Price50 = 40,
            Price100 = 35,
            CategoryId = 3,
            ImageUrl = ""
        },
        new Product
        {
            Id = 4,
            Title = "Cotton Candy",
            Author = "Abby Muscles",
            Description = "A whimsical and heartwarming story of friendship and dreams, where the sweetness of life is experienced through the simple joys of childhood.",
            ISBN = "WS3333333301",
            ListPrice = 70,
            Price = 65,
            Price50 = 60,
            Price100 = 55,
            CategoryId = 1,
            ImageUrl = ""
        },
        new Product
        {
            Id = 5,
            Title = "Rock in the Ocean",
            Author = "Ron Parker",
            Description = "An adventure-filled narrative that explores the mysteries of the deep sea. Dive into a world of unknown creatures and hidden treasures beneath the waves.",
            ISBN = "SOTJ1111111101",
            ListPrice = 30,
            Price = 27,
            Price50 = 25,
            Price100 = 20,
            CategoryId = 2,
            ImageUrl = ""
        },
        new Product
        {
            Id = 6,
            Title = "Leaves and Wonders",
            Author = "Laura Phantom",
            Description = "A poetic reflection on nature's beauty and the changing seasons. This book brings to life the wonders of the natural world through vivid imagery and thoughtful prose.",
            ISBN = "FOT000000001",
            ListPrice = 25,
            Price = 23,
            Price50 = 22,
            Price100 = 20,
            CategoryId = 3,
            ImageUrl = ""
        },
        new Product
        {
            Id = 7,
            Title = "Whispers in the Wind",
            Author = "Evelyn Morris",
            Description = "A historical fiction novel set in the Victorian era, where forbidden love and betrayal intertwine, creating a powerful narrative of passion and regret.",
            ISBN = "WIT00000101",
            ListPrice = 45,
            Price = 40,
            Price50 = 35,
            Price100 = 30,
            CategoryId = 1,
            ImageUrl = ""
        },
        new Product
        {
            Id = 8,
            Title = "Echoes of the Past",
            Author = "Michael T. Grant",
            Description = "A thrilling mystery novel where a detective uncovers long-buried secrets. Every twist and turn brings the protagonist closer to solving the enigma of a century-old crime.",
            ISBN = "EOP998877601",
            ListPrice = 50,
            Price = 45,
            Price50 = 40,
            Price100 = 35,
            CategoryId = 2,
            ImageUrl = ""
        },
        new Product
        {
            Id = 9,
            Title = "Stars Beyond the Horizon",
            Author = "Clara Middleton",
            Description = "A science fiction epic that takes readers to the farthest reaches of space. Encounter alien civilizations and technological marvels in this fast-paced space adventure.",
            ISBN = "SBH123456789",
            ListPrice = 60,
            Price = 55,
            Price50 = 50,
            Price100 = 45,
            CategoryId = 2,
            ImageUrl = ""

        },
        new Product
        {
            Id = 10,
            Title = "Silent Snowfall",
            Author = "Henry Walker",
            Description = "Set in a small, snow-covered village, this quiet, introspective novel delves into themes of loneliness, community, and the search for inner peace during the coldest of winters.",
            ISBN = "SSN001122334",
            ListPrice = 38,
            Price = 32,
            Price50 = 28,
            Price100 = 25,
            CategoryId = 3,
            ImageUrl = ""
        }
    };

        modelBuilder.Entity<Product>().HasData(products);
    }
}


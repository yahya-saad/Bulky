namespace BulkyWeb.Data;

public static class DataSeeder
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        var categories = new List<Category>()
        {
            new Category {Id = 1 , Name = "Action" , DisplayOrder = 1},
            new Category {Id = 2 , Name = "SciFi" , DisplayOrder = 2},
            new Category {Id = 3 , Name = "History" , DisplayOrder = 3}
        };

        modelBuilder.Entity<Category>().HasData(categories);
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BulkyBook.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Add_Product_Table_And_Seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListPrice = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Price50 = table.Column<double>(type: "float", nullable: false),
                    Price100 = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "Description", "ISBN", "ListPrice", "Price", "Price100", "Price50", "Title" },
                values: new object[,]
                {
                    { 1, "Billy Spark", "A gripping tale of time travel and the quest for eternal life. Join the protagonist on an epic journey across different eras, where every decision counts.", "SWD9999001", 99.0, 90.0, 80.0, 85.0, "Fortune of Time" },
                    { 2, "Nancy Hoover", "A chilling story of survival in a world on the brink of collapse. Will humanity overcome its darkest hour or succumb to the forces of nature?", "CAW777777701", 40.0, 30.0, 20.0, 25.0, "Dark Skies" },
                    { 3, "Julian Button", "In a secluded town, mysteries unfold as a sudden disappearance leads to secrets being uncovered. Can the truth be found before it's too late?", "RITO5555501", 55.0, 50.0, 35.0, 40.0, "Vanish in the Sunset" },
                    { 4, "Abby Muscles", "A whimsical and heartwarming story of friendship and dreams, where the sweetness of life is experienced through the simple joys of childhood.", "WS3333333301", 70.0, 65.0, 55.0, 60.0, "Cotton Candy" },
                    { 5, "Ron Parker", "An adventure-filled narrative that explores the mysteries of the deep sea. Dive into a world of unknown creatures and hidden treasures beneath the waves.", "SOTJ1111111101", 30.0, 27.0, 20.0, 25.0, "Rock in the Ocean" },
                    { 6, "Laura Phantom", "A poetic reflection on nature's beauty and the changing seasons. This book brings to life the wonders of the natural world through vivid imagery and thoughtful prose.", "FOT000000001", 25.0, 23.0, 20.0, 22.0, "Leaves and Wonders" },
                    { 7, "Evelyn Morris", "A historical fiction novel set in the Victorian era, where forbidden love and betrayal intertwine, creating a powerful narrative of passion and regret.", "WIT00000101", 45.0, 40.0, 30.0, 35.0, "Whispers in the Wind" },
                    { 8, "Michael T. Grant", "A thrilling mystery novel where a detective uncovers long-buried secrets. Every twist and turn brings the protagonist closer to solving the enigma of a century-old crime.", "EOP998877601", 50.0, 45.0, 35.0, 40.0, "Echoes of the Past" },
                    { 9, "Clara Middleton", "A science fiction epic that takes readers to the farthest reaches of space. Encounter alien civilizations and technological marvels in this fast-paced space adventure.", "SBH123456789", 60.0, 55.0, 45.0, 50.0, "Stars Beyond the Horizon" },
                    { 10, "Henry Walker", "Set in a small, snow-covered village, this quiet, introspective novel delves into themes of loneliness, community, and the search for inner peace during the coldest of winters.", "SSN001122334", 38.0, 32.0, 25.0, 28.0, "Silent Snowfall" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}

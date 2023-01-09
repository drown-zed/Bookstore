using Bookstore.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookstore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("Database"));
           
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddRouting(options => options.LowercaseUrls = true);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            SeedData(app);

            app.Run();
        }

        private static void SeedData(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DataContext>();

                var authors = new List<Author>
                {
                    new Author { FirstName = "First 1", LastName = "Last 1"},
                    new Author { FirstName = "First 2", LastName = "Last 2"},
                };

                context.Author.AddRange(authors);

                var books = new List<Book>
                {
                    new Book { Title = "Book 1", Author = authors[0] },
                    new Book { Title = "Book 2", Author = authors[1] },
                    new Book { Title = "Book 3", Author = authors[1] }
                };

                context.Book.AddRange(books);
                context.SaveChanges();
            }
        }
    }
}
using bookDemo.Data;
using bookDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace bookDemo.Controllers
{
    //The [Route("api/books")] attribute specifies the route for this API controller, indicating that it will handle requests under the "/api/books" endpoint.
    [Route("api/books")]

    //The [ApiController] attribute denotes that this class is an API controller, enabling various features such as automatic model validation.
    [ApiController]
    public class BooksController : ControllerBase
    {
        //The HttpGet attribute indicates that this method responds to HTTP GET requests.
        [HttpGet]

        //The purpose of the method is to retrieve all books from the application context.
        public IActionResult GetAllBooks()
        {
            var books = ApplicationContext.Books;

            return Ok(books);
        }

        //The [HttpGet("{id:int}")] attribute specifies that this method responds to HTTP GET requests with a route parameter "id" of type integer.
        [HttpGet("{id:int}")]

        //The purpose of the method is to retrieve a specific book by its unique identifier from the application context.
        //It checks if the requested book exists, and if not, it returns a 404 status code.
        //If the book is found, it returns a successful response with the details of the requested book.
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id) //The [FromRoute(Name = "id")] attribute binds the "id" parameter from the route to the method parameter.
        {
            var book = ApplicationContext.Books.Where(b => b.Id == id).FirstOrDefault();

            if (book == null)
            {
                return NotFound(); // Respond with a 404 status code if the book is not found.
            }
            // Return a successful response with the details of the requested book.
            return Ok(book);
        }

        //The [HttpPost] attribute specifies that this method responds to HTTP POST requests.
        [HttpPost]

        //The method represents the creation of a new book based on the provided request body.
        public IActionResult CreateOneBook([FromBody] Book book) //The[FromBody] attribute indicates that the Book parameter comes from the request body.
        {
            try
            {
                if (book == null)
                {
                    return BadRequest(); // Return a 400 Bad Request status code if the book is null.
                }

                ApplicationContext.Books.Add(book);
                // Return a successful response with the created book and a 201 Created status code.
                return StatusCode(201, book);
            }
            catch (Exception ex)
            {
                return BadRequest(); // Return a 400 Bad Request status code in case of an exception.
            }

        }
        //The [HttpPut("{id:int}")] attribute specifies that this method responds to HTTP PUT requests with a route parameter "id" of type integer.
        [HttpPut("{id:int}")]

        //The [FromRoute(Name = "id")] attribute binds the "id" parameter from the route to the method parameter.
        //The method represents updating an existing book based on the provided id and book in the request body.
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id,
            [FromBody] Book book) // The reason we have both the id and book structure here is; We will delete with id and add the book structure.
        {
            // Check if the book with the specified id exists.
            var entity = ApplicationContext.Books.Find(b => b.Id == id);

            if (entity is null)
                return NotFound(); // Return a 404 Not Found status code if the book is not found.

            // Check if the provided id in the route matches the id in the book entity.
            if (id != book.Id)
            {
                return BadRequest("Parameters do not match");
            }
            // Remove the existing book entity and add the updated book to the application context.
            ApplicationContext.Books.Remove(entity);
            book.Id = entity.Id;
            ApplicationContext.Books.Add(book);

            return Ok(book);

        }

        //The [HttpDelete] attribute specifies that this method responds to HTTP DELETE requests.
        [HttpDelete]

        //The purpose of the method is to clear all books from the application context, essentially resetting the book collection.
        public IActionResult DeleteAllBook()

        {
            ApplicationContext.Books.Clear();
            return NoContent(); 

        }

        //The [HttpDelete("{id:int}")] attribute specifies that this method responds to HTTP DELETE requests with a route parameter "id" of type integer.
        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)

        {
            var entity = ApplicationContext.Books.Find(b => b.Id == id);

            if (entity is null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = $"Book with id:{id} could not found."
                });

            ApplicationContext.Books.Remove(entity);
            return NoContent();
        }

        //In order to use the Patch Http method, some packages must be installed; microsoft.aspnetcore.mvc.newtonsoftjson and Install-Package Microsoft.AspNetCore.JsonPatch packages must be install and newtonsoftjson must add to program.cs pipeline as builder.Services.AddControllers().AddNewtonsoftJson();

        //The [HttpPatch("{id:int}")] attribute Partially updates a specific book identified by its unique integer ID.
        [HttpPatch("{id:int}")]

        public IActionResult PartiallyupdateOneBook([FromRoute(Name = "id")] int id,
            [FromBody] JsonPatchDocument<Book> bookPatch) //"bookPatch" A JSON patch document containing the changes to be applied to the book.
        {
            var entity = ApplicationContext.Books.Find(b => b.Id.Equals(id));
            if (entity is null)

                return NotFound();

            bookPatch.ApplyTo(entity); //We modify the body value we brought from the request on the entity from which we receive ApplicationContext via id.

            return NoContent();

        }
    }
}

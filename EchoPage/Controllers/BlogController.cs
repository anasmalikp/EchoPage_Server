using EchoPage.Interface;
using EchoPage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EchoPage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogServices services;
        public BlogController(IBlogServices services)
        {
            this.services = services;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> NewBlog(Blogs blog)
        {
            var response = await services.createBlog(blog);
            if (response)
            {
                return Ok("Blog created successfully");
            }
            return BadRequest("something went wrong");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllBlogs()
        {
            var response = await services.GetBlogs();
            return Ok(response);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateBlogs(Blogs blog)
        {
            var response = await services.UpdateBlog(blog);
            if(response)
            {
                return Ok("Updated Successfully");
            }
            return BadRequest("Something went wrong");
        }

        [HttpPost("search")]
        [Authorize]
        public async Task<IActionResult> Search(string prompt)
        {
            var response = await services.SearchBlogs(prompt);
            return Ok(response);
        }

        [HttpGet("individualblogs")]
        [Authorize]
        public async Task<IActionResult> IndividualBlogs(int id)
        {
            var response = await services.GetSingleBlog(id);
            return Ok(response);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var response = await services.RemoveBlog(id);
            if (response)
            {
                return Ok("deleted");
            }
            return NotFound();
        }
    }
}

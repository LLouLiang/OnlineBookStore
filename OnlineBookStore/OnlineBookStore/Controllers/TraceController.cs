using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineBookStore.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineBookStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TraceController : ControllerBase
    {
        private readonly OnlineBookStoreDbContext _dbContext;

        public TraceController(OnlineBookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetTraces()
        {
            var traces = _dbContext.Traces.ToList();
            return Ok(traces);
        }

        [HttpGet("{id}")]
        public IActionResult GetTrace(int id)
        {
            var trace = _dbContext.Traces.FirstOrDefault(t => t.Id == id);
            if (trace == null)
            {
                return NotFound();
            }
            return Ok(trace);
        }
    }
}

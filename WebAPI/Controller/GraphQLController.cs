/*using HotChocolate;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;
using WebAPI.GraphQL;
using WebAPI.Models;

[Route("api/packages")]
[ApiController]
public class GraphQLController : ControllerBase
{
    private readonly IRequestExecutor _executor;
    private readonly IPackageService

    public GraphQLController(IRequestExecutor executor)
    {
        
        _executor = executor;
    }

    [HttpPost("getallpackages")]
    public async Task<IActionResult> GetAllPackages([FromBody] GraphQLQuery query)
    {
        try
        {
            var executionResult = await _executor.ExecuteAsync(GetPackagesQuery);
            return Ok(executionResult);
            
        }
        catch
        {
            return BadRequest("bad request");
        }
    }

}
*/
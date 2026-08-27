using Microsoft.AspNetCore.Mvc;

namespace Maxgram.Backend.Common;

public static class ControllerResultExtensions
{
    public static ActionResult<T> ToActionResult<T>(this ServiceResult<T> result)
    {
        return result.Error switch
        {
            ErrorCode.None => new OkObjectResult(result.Data),
            ErrorCode.NotFound => new NotFoundObjectResult(new { message = result.Message }),
            ErrorCode.Forbidden => new ObjectResult(new { message = result.Message }) { StatusCode = 403 },
            ErrorCode.Conflict => new ConflictObjectResult(new { message = result.Message }),
            _ => new BadRequestObjectResult(new { message = result.Message })
        };
    }
}

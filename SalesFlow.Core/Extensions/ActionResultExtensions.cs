using Microsoft.AspNetCore.Mvc;
using SalesFlow.Core.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Core.Extensions
{
    public static class ActionResultExtensions
    {
        public static IActionResult ToActionResult(this ControllerBase controller, Result result)
        {
            return result.IsSuccess
                ? controller.Ok(result)
                : controller.BadRequest(result);
        }

        public static IActionResult ToActionResult<T>(this ControllerBase controller, Result<T> result)
        {
            return result.IsSuccess
                ? controller.Ok(result)
                : controller.BadRequest(result);
        }
    }
}

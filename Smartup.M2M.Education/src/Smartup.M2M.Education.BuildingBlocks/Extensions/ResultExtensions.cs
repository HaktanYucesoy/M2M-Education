using Microsoft.AspNetCore.Http;
using Smartup.M2M.Education.BuildingBlocks.Results;


namespace Smartup.M2M.Education.BuildingBlocks.Extensions
{
    public static class ResultExtensions
    {
        public static IResult ToHttpResult<T>(this Result<T> result,
        Func<T, IResult>? onSuccess = null)
        {
            if(result.IsSuccess)

                return onSuccess!=null? onSuccess(result.Value) :
                    Microsoft.AspNetCore.Http.Results.Ok(result.Value);

            return CreateProblemDetails(result.Errors);
        }

        public static IResult ToHttpResult(this Result result)
        {
            if (result.IsSuccess)
                return Microsoft.AspNetCore.Http.Results.NoContent();

            return CreateProblemDetails(result.Errors);
        }



        private static IResult CreateProblemDetails(IReadOnlyList<Error> errors)
        {
            ArgumentNullException.ThrowIfNull(errors);
            var primary = errors[0];

            var statusCode = primary.Type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                ErrorType.BusinessRule => StatusCodes.Status422UnprocessableEntity,
                ErrorType.Failure => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            };

            string title = GetTitle(primary.Type);
            string detail=primary.Description;

            if (primary.Type == ErrorType.Validation && errors.Any(e => !string.IsNullOrWhiteSpace(e.Target)))
            {
                var validationErrors = errors
                    .Where(e => !string.IsNullOrWhiteSpace(e.Target))
                    .GroupBy(e => e.Target!)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.Description).ToArray());

                
                return Microsoft.AspNetCore.Http.Results.ValidationProblem(
                    validationErrors,
                    title: title,
                    detail: detail,
                    statusCode: statusCode,
                    extensions: new Dictionary<string, object?> { ["errorCode"] = primary.Code });
            }

            return Microsoft.AspNetCore.Http.Results.Problem(
                title: title,
                detail: detail,
                statusCode: statusCode,
                extensions: new Dictionary<string, object?> { ["errorCode"] = primary.Code });
        }

        private static string GetTitle(ErrorType errorType) => errorType switch
        {
            ErrorType.Validation => "Validation Error",
            ErrorType.NotFound => "Resource Not Found",
            ErrorType.Conflict => "Data Conflict",
            ErrorType.Unauthorized => "Unauthorized Access",
            ErrorType.Forbidden => "Forbidden",
            ErrorType.BusinessRule => "Business Rule Violation",
            ErrorType.Failure => "Internal Server Error",
            _ => "Server Error"
        };
    }
}

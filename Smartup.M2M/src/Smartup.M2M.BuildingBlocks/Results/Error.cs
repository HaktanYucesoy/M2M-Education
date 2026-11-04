
namespace Smartup.M2M.BuildingBlocks.Results
{
    public readonly record struct Error
    {
        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);

        public string Code { get; }
        public string Description { get; }
        public ErrorType Type { get; }
        public string? Target { get; }

        public Error(string code, string description, ErrorType type, string? target = null)
        {
            Code = code ?? string.Empty;
            Description = description ?? string.Empty;
            Type = type;
            Target = target;
        }

        public static Error Failure(string code, string description, string? target = null) =>
            new(code, description, ErrorType.Failure, target);

        public static Error Validation(string code, string description, string? target = null) =>
            new(code, description, ErrorType.Validation, target);

        public static Error NotFound(string code, string description, string? target = null) =>
            new(code, description, ErrorType.NotFound, target);

        public static Error Conflict(string code, string description, string? target = null) =>
            new(code, description, ErrorType.Conflict, target);

        public static Error Unauthorized(string code, string description, string? target = null) =>
            new(code, description, ErrorType.Unauthorized, target);

        public static Error Forbidden(string code, string description, string? target = null) =>
            new(code, description, ErrorType.Forbidden, target);

        public static Error BusinessRule(string code, string description, string? target = null) =>
            new(code, description, ErrorType.BusinessRule, target);
    }

}

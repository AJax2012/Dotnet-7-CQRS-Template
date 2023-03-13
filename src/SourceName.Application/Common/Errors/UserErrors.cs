using System.Diagnostics.CodeAnalysis;

using ErrorOr;

namespace SourceName.Application.Common.Errors;

/// <summary>
/// Errors from business logic based on User model.
/// </summary>
public static partial class Errors
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Public required for access in other namespaces")]
    public static class User
    {
        public static readonly Error NotFound = Error.NotFound("User.NotFound", "Could not find User");
    }
}
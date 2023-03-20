using System.Diagnostics.CodeAnalysis;

using ErrorOr;

namespace SourceName.Application.Common.Errors;

/// <summary>
/// Errors from business logic based on Entity model.
/// </summary>
public static partial class Errors
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Public required for access in other namespaces")]
    public static class Entity
    {
        public static readonly Error NotFound = Error.NotFound("Entity.NotFound", "Could not find Entity");
        public static readonly Error CreateError = Error.Failure("Entity.Created", "Entity failed to be created");
        public static readonly Error UpdateError = Error.Failure("Entity.Updated", "Entity failed to be updated");

        public static readonly Error
            Exists = Error.Conflict("Entity.Exists", "Entity with description already exists.");
    }
}
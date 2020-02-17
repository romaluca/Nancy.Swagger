using System;

namespace Swagger.ObjectModel.Alyce.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Enum, AllowMultiple = false, Inherited = true)]
    internal class SwaggerDataAttribute : Attribute { }
}
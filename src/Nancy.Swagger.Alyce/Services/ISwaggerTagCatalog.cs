using System;
using System.Collections.Generic;
using Swagger.ObjectModel;

namespace Nancy.Swagger.Alyce.Services
{
    [SwaggerApi]
    public interface ISwaggerTagCatalog : IEnumerable<Tag>
    {
        void AddTag(Tag tag);
    }
}
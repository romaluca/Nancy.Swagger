using Swagger.ObjectModel;

namespace Nancy.Swagger.Alyce.Services
{
    [SwaggerApi]
    public interface ISwaggerMetadataProvider
    {
        SwaggerRoot GetSwaggerJson(NancyContext context);
    }
}
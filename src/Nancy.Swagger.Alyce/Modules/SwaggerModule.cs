using Nancy.Swagger.Alyce.Services;

namespace Nancy.Swagger.Alyce.Modules
{
    [SwaggerApi]
    public class SwaggerModule : NancyModule
    {
        public SwaggerModule(ISwaggerMetadataProvider converter)
            : base(SwaggerConfig.ResourceListingPath)
        {
            Get("/api-docs", _ => converter.GetSwaggerJson(Context).ToJson());
            Get("/swagger.json", _ => converter.GetSwaggerJson(Context).ToJson());
        }
    }
}
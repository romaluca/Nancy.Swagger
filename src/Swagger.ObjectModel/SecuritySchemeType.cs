namespace Swagger.ObjectModel.Alyce
{
    using Swagger.ObjectModel.Alyce.Attributes;

    /// <summary>
    /// The security scheme type.
    /// </summary>
    [SwaggerData]
    public enum SecuritySchemes
    {
        /// <summary>
        /// The basic.
        /// </summary>
        [SwaggerEnumValue("basic")]
        Basic,

        /// <summary>
        /// The api key.
        /// </summary>
        [SwaggerEnumValue("apiKey")]
        ApiKey,

        /// <summary>
        /// The oauth 2.
        /// </summary>
        [SwaggerEnumValue("oauth2")]
        Oauth2
    }
}
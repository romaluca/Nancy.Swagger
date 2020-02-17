namespace Swagger.ObjectModel.Alyce
{
    using Swagger.ObjectModel.Alyce.Attributes;

    /// <summary>
    /// The header.
    /// </summary>
    public class Header : DataType
    {
        /// <summary>
        /// A short description of the header.
        /// </summary>
        [SwaggerProperty("description")]
        public string Description { get; set; }
    }
}
﻿namespace Swagger.ObjectModel.Alyce
{
    using System.Collections.Generic;

    using Swagger.ObjectModel.Alyce.Attributes;

    /// <summary>
    /// Describes a single response from an API Operation.
    /// </summary>
    public class Response : SwaggerModel
    {
        /// <summary>
        /// A short description of the response. GFM syntax can be used for rich text representation.
        /// </summary>
        [SwaggerProperty("description", true)]
        public string Description { get; set; }

        /// <summary>
        /// A definition of the response structure. 
        /// </summary>
        [SwaggerProperty("schema")]
        public Schema Schema { get; set; }

        /// <summary>
        /// A list of headers that are sent with the response.
        /// The name of the property corresponds to the name of the header. The value describes the type of the header.
        /// </summary>
        [SwaggerProperty("headers")]
        public IDictionary<string, Header> Headers { get; set; }

        /// <summary>
        /// An example of the response message. 
        /// The name of the property MUST be one of the Operation produces values (either implicit or inherited). 
        /// The value SHOULD be an example of what such a response would look like.
        /// </summary>
        [SwaggerProperty("examples")]
        public IDictionary<string, object> Examples { get; set; }
    }
}
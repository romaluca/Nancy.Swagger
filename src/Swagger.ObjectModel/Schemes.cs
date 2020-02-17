﻿namespace Swagger.ObjectModel.Alyce
{
    using Swagger.ObjectModel.Alyce.Attributes;
    
    /// <summary>
    /// The schemes.
    /// </summary>
    [SwaggerData]
    public enum Schemes
    {
        /// <summary>
        /// The http.
        /// </summary>
        [SwaggerEnumValue("http")]
        Http,

        /// <summary>
        /// The https.
        /// </summary>
        [SwaggerEnumValue("https")]
        Https,

        /// <summary>
        /// The ws.
        /// </summary>
        [SwaggerEnumValue("ws")]
        Ws,

        /// <summary>
        /// The wss.
        /// </summary>
        [SwaggerEnumValue("wss")]
        Wss,
    }
}
﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiKeySecuritySchemeBuilder.cs" company="CHS Health Services">
//   Copyright (c) 2015 CHS Health Services. All rights reserved.
// </copyright>
// <summary>
//   The api key security scheme builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Swagger.ObjectModel.Alyce.Builders
{
    /// <summary>
    /// The api key security scheme builder.
    /// </summary>
    public class ApiKeySecuritySchemeBuilder : SecuritySchemeBuilder
    {
        /// <summary>
        /// The build.
        /// </summary>
        /// <returns>
        /// The <see cref="SecurityScheme"/>.
        /// </returns>
        /// <exception cref="RequiredFieldException">
        /// </exception>
        public override SecurityScheme Build()
        {
            if (string.IsNullOrWhiteSpace(this.name))
            {
                throw new RequiredFieldException("Name");
            }

            if (this.securityIn == null)
            {
                throw new RequiredFieldException("In");
            }

            return new SecurityScheme { Type = SecuritySchemes.ApiKey, Name = this.name, In = this.securityIn, Description = this.description };
        }

        /// <summary>
        /// A short description for security scheme.
        /// </summary>
        /// <param name="description">
        /// The description.
        /// </param>
        /// <returns>
        /// The <see cref="SecuritySchemeBuilder"/>.
        /// </returns>
        public ApiKeySecuritySchemeBuilder Description(string description)
        {
            this.description = description;
            return this;
        }

        /// <summary>
        /// The name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="SecuritySchemeBuilder"/>.
        /// </returns>
        public ApiKeySecuritySchemeBuilder Name(string name)
        {
            this.name = name;
            return this;
        }

        /// <summary>
        /// Declare that the API key is located in the query
        /// </summary>
        /// <returns>
        /// The <see cref="SecuritySchemeBuilder"/>.
        /// </returns>
        public ApiKeySecuritySchemeBuilder IsInQuery()
        {
            return this.In(ApiKeyLocations.Query);
        }

        /// <summary>
        /// Declare that the API key is located in a header
        /// </summary>
        /// <returns>
        /// The <see cref="SecuritySchemeBuilder"/>.
        /// </returns>
        public ApiKeySecuritySchemeBuilder IsInHeader()
        {
            return this.In(ApiKeyLocations.Header);
        }

        /// <summary>
        /// The in.
        /// </summary>
        /// <param name="securityIn">
        /// The security in.
        /// </param>
        /// <returns>
        /// The <see cref="SecuritySchemeBuilder"/>.
        /// </returns>
        private ApiKeySecuritySchemeBuilder In(ApiKeyLocations securityIn)
        {
            this.securityIn = securityIn;
            return this;
        }
    }
}
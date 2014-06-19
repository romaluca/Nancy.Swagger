﻿using System;
using System.Linq;

using System.Collections.Generic;

using Nancy.Swagger.ApiDeclaration;

namespace Nancy.Swagger
{
    /// <summary>
    /// Holds all the Swagger metadata pertaining to a specific route.
    /// </summary>
    public class SwaggerRouteData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerRouteData"/> class.
        /// </summary>
        public SwaggerRouteData()
        {
            OperationParameters = new List<SwaggerParameterData>();
            OperationResponseMessages = new List<ResponseMessage>();
            OperationProduces = new List<string>();
            OperationConsumes = new List<string>();
        }

        public string ResourcePath { get; set; }

        public string ApiPath { get; set; }

        public string OperationNickname { get; set; }

        public string OperationSummary { get; set; }

        public HttpMethod OperationMethod { get; set; }

        public string OperationNotes { get; set; }

        public IList<SwaggerParameterData> OperationParameters { get; set; }

        public IList<ResponseMessage> OperationResponseMessages { get; set; }

        public IList<string> OperationProduces { get; set; }

        public IList<string> OperationConsumes { get; set; }

        public Type OperationModel { get; set; }

        public Operation ToOperation()
        {
            var operation = new Operation
            {
                Nickname = OperationNickname,
                Summary = OperationSummary,
                Method = OperationMethod,
                Notes = OperationNotes,
                Parameters = OperationParameters.Select(p => p.ToParameter()),
                ResponseMessages = OperationResponseMessages,
                Produces = OperationProduces,
                Consumes = OperationConsumes,
            };

            if (OperationModel != null)
            {
                if (Primitive.IsPrimitive(OperationModel))
                {
                    var primitive = Primitive.FromType(OperationModel);

                    operation.Type = primitive.Type;
                    operation.Format = primitive.Format;
                }
                else
                {
                    operation.Type = OperationModel.DefaultModelId();
                }
            }
            else
            {
                operation.Type = "void";
            }

            return operation;
        }
    }
}
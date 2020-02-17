using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Nancy.Swagger.Alyce.Annotations.Attributes;
using Swagger.ObjectModel.Alyce;

namespace Nancy.Swagger.Alyce.Annotations.SwaggerObjects
{
    public class AnnotatedBodyParameter : BodyParameter
    {
        public AnnotatedBodyParameter(string name, Type paramType, RouteParamAttribute attrib, ISwaggerModelCatalog modelCatalog)
        {
            Name = attrib.Name ?? name;
            this.AddBodySchema(paramType, modelCatalog);
        }
    }
}

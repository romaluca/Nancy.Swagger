﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Nancy.Routing;
using Nancy.Swagger.Alyce.Services;
using Swagger.ObjectModel;
using Swagger.ObjectModel.Builders;

namespace Nancy.Swagger.Alyce
{

    [SwaggerApi]
    public static class SwaggerExtensions
    {
        /// <summary>
        /// Returns an instance of <see cref="PathItem"/> representing this route.
        /// </summary>
        /// <param name="description">The <see cref="RouteDescription"/>.</param>
        /// <param name="action">An <see cref="Action{PathItemBuilder}"/> for building the <see cref="PathItem"/>.</param>
        /// <returns>An instance of <see cref="PathItem"/> constructed using <paramref name="description"/> and by invoking <paramref name="action"/>.</returns>
        public static PathItem AsSwagger(this RouteDescription description, Action<PathItemBuilder> action)
        {
            var builder = new PathItemBuilder(description.Method.ToHttpMethod());
            action.Invoke(builder);
            return builder.Build();
        }

        public static T ToDataType<T>(this Type type, bool isTopLevel = false)
            where T : DataType, new()
        {
            var dataType = new T();

            if (SwaggerTypeMapping.IsMappedType(type))
            {
                type = SwaggerTypeMapping.GetMappedType(type);
            }

            if (type == null)
            {
                dataType.Type = "void";

                return dataType;
            }

            if (type.IsNullable())
            {
                type = Nullable.GetUnderlyingType(type);
            }
            if (Primitive.IsPrimitive(type))
            {
                var primitive = Primitive.FromType(type);

                dataType.Format = primitive.Format;
                dataType.Type = primitive.Type;

                return dataType;
            }
 

            if (type.IsContainer())
            {
                dataType.Type = "array";

                var itemsType = type.GetElementType() ?? type.GetTypeInfo().GetGenericArguments().FirstOrDefault();

                if (Primitive.IsPrimitive(itemsType))
                {
                    var primitive = Primitive.FromType(itemsType);

                    dataType.Items = new Item
                    {
                        Type = primitive.Type,
                        Format = primitive.Format
                    };

                    return dataType;
                }

                dataType.Items = new Item { Ref = SwaggerConfig.ModelIdConvention(itemsType) };

                return dataType;
            }

            if (isTopLevel)
            {
                dataType.Ref = SwaggerConfig.ModelIdConvention(type);
                return dataType;
            }

            dataType.Type = SwaggerConfig.ModelIdConvention(type);

            return dataType;
        }

        public static IEnumerable<Model> ToModel(this SwaggerModelData model, IEnumerable<SwaggerModelData> knownModels = null, bool getSubModels = true)
        {
            var classProperties = model.Properties.Where(x => !Primitive.IsPrimitive(x.Type) && !x.Type.GetTypeInfo().IsEnum && !x.Type.GetTypeInfo().IsGenericType);

            var modelsData = knownModels ?? Enumerable.Empty<SwaggerModelData>();
            if (getSubModels)
            {
                foreach (var swaggerModelPropertyData in classProperties)
                {
                    var properties = GetPropertiesFromType(swaggerModelPropertyData.Type);

                    var modelDataForClassProperty =
                        modelsData.FirstOrDefault(x => x.ModelType == swaggerModelPropertyData.Type);

                    var id = modelDataForClassProperty == null
                        ? swaggerModelPropertyData.Type.Name
                        : SwaggerConfig.ModelIdConvention(modelDataForClassProperty.ModelType);

                    var description = modelDataForClassProperty == null
                        ? swaggerModelPropertyData.Description
                        : modelDataForClassProperty.Description;

                    var required = modelDataForClassProperty == null
                        ? properties.Where(p => p.Required || p.Type.IsImplicitlyRequired())
                            .Select(p => p.Name)
                            .OrderBy(name => name)
                            .ToList()
                        : modelDataForClassProperty.Properties
                            .Where(p => p.Required || p.Type.IsImplicitlyRequired())
                            .Select(p => p.Name)
                            .OrderBy(name => name)
                            .ToList();

                    if (!required.Any()) required = null;

                    var modelproperties = modelDataForClassProperty == null
                        ? properties.OrderBy(x => x.Name).ToDictionary(p => p.Name, ToModelProperty)
                        : modelDataForClassProperty.Properties.OrderBy(x => x.Name)
                            .ToDictionary(p => p.Name, ToModelProperty);

                    yield return new Model
                    {
                        Id = id,
                        Description = description,
                        Required = required,
                        Properties = modelproperties
                    };
                }
            }

            var topLevelModel = new Model
            {
                Id = SwaggerConfig.ModelIdConvention(model.ModelType),
                Description = model.Description,
                Required = model.Properties
                    .Where(p => p.Required || p.Type.IsImplicitlyRequired())
                    .Select(p => p.Name)
                    .OrderBy(name => name)
                    .ToList(),
                Properties = model.Properties
                    .OrderBy(p => p.Name)
                    .ToDictionary(p => p.Name, ToModelProperty)

                // TODO: SubTypes and Discriminator
            };

            if (!topLevelModel.Required.Any()) topLevelModel.Required = null;
            
            yield return topLevelModel;
        }

        public static ModelProperty ToModelProperty(this SwaggerModelPropertyData modelPropertyData)
        {
            var propertyType = modelPropertyData.Type;

            var isClassProperty = !Primitive.IsPrimitive(propertyType);

            var modelProperty = modelPropertyData.Type.ToDataType<ModelProperty>(isClassProperty);

            modelProperty.Default = modelPropertyData.DefaultValue;
            modelProperty.Description = modelPropertyData.Description;
            modelProperty.Enum = modelPropertyData.Enum;
            modelProperty.Minimum = modelPropertyData.Minimum;
            modelProperty.Maximum = modelPropertyData.Maximum;

            if (modelPropertyData.Type.IsContainer())
            {
                modelProperty.UniqueItems = modelPropertyData.UniqueItems ? true : (bool?)null;
            }

            return modelProperty;
        }

        private static IList<SwaggerModelPropertyData> GetPropertiesFromType(Type type)
        {
            return type.GetTypeInfo().GetProperties()
                .Select(property => new SwaggerModelPropertyData
                {
                    Name = property.Name,
                    Type = property.PropertyType
                }).ToList();
        }

        public static bool IsContainer(this Type type)
        {
            return typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(type)
                && !typeof(string).IsAssignableFrom(type);
        }

        public static string ToCamelCase(this string val)
        {
            if (string.IsNullOrEmpty(val) || !char.IsUpper(val[0]))
            {
                return val;
            }
            char[] chars = val.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (i == 1 && !char.IsUpper(chars[i]))
                {
                    break;
                }

                bool hasNext = (i + 1 < chars.Length);
                if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
                {
                    if (char.IsSeparator(chars[i + 1]))
                    {
                        chars[i] = char.ToLower(chars[i]);
                    }

                    break;
                }

                chars[i] = char.ToLower(chars[i]);
            }

            return new string(chars);
        }

        internal static bool IsImplicitlyRequired(this Type type)
        {
            return type.GetTypeInfo().IsValueType && !type.IsNullable();
        }

        internal static bool IsNullable(this Type type)
        {
            return type.GetTypeInfo().IsGenericType && type.GetTypeInfo().GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static HttpMethod ToHttpMethod(this string method)
        {
            switch (method)
            {
                case "DELETE":
                    return HttpMethod.Delete;

                case "GET":
                    return HttpMethod.Get;

                case "OPTIONS":
                    return HttpMethod.Options;

                case "PATCH":
                    return HttpMethod.Patch;

                case "POST":
                    return HttpMethod.Post;

                case "PUT":
                    return HttpMethod.Put;

                case "HEAD":
                    return HttpMethod.Head;

                default:
                    throw new NotSupportedException(string.Format("HTTP method '{0}' is not supported.", method));
            }
        }

        public static IEnumerable<Type> GetDistinctModelTypes(this IDictionary<string, SwaggerRouteData> routeData)
        {
            return GetOperationModels(routeData)
                .Select(GetType)
                .Where(type => !Primitive.IsPrimitive(type))
                .Distinct();
        }

        /// <summary>
        /// This forces the response builder to use Nancy.Swagger's created schema
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseBuilder"></param>
        /// <param name="modelCatalog"></param>
        /// <returns></returns>
        public static ResponseBuilder Schema<T>(this ResponseBuilder responseBuilder, ISwaggerModelCatalog modelCatalog)
        {
            var schema = GetSchema<T>(modelCatalog, false);
            responseBuilder.Schema(schema);
            return responseBuilder;
        }

        public static OperationBuilder AddResponseSchema<T>(this OperationBuilder operationBuilder, ISwaggerModelCatalog modelCatalog)
        {
            var schema = GetSchema<T>(modelCatalog, false);
            operationBuilder.Response(r => r.Description("default").Schema(schema));
            return operationBuilder;
        }

        public static BodyParameter AddBodySchema<T>(this BodyParameter bodyParameter, ISwaggerModelCatalog modelCatalog)
        {
            return bodyParameter.AddBodySchema(typeof(T), modelCatalog);
        }

        public static BodyParameter AddBodySchema(this BodyParameter bodyParameter, Type type, ISwaggerModelCatalog modelCatalog)
        {
            var schema = GetSchema(modelCatalog, type, false);
            bodyParameter.Schema = schema;
            return bodyParameter;
        }

        public static Schema GetSchema<T>(ISwaggerModelCatalog modelCatalog, bool isDefinition)
        {
            return GetSchema(modelCatalog, typeof(T), isDefinition);
        }

        public static Schema GetSchema(ISwaggerModelCatalog modelCatalog, Type t, bool isDefinition)
        {
            if (SwaggerTypeMapping.IsMappedType(t))
            {
                t = SwaggerTypeMapping.GetMappedType(t);
            }
            var model = modelCatalog.GetModelForType(t);
            var schema = new Schema();
            if (model != null)
            {
                schema = model.GetSchema(isDefinition);
            }
            else if (Primitive.IsPrimitive(t))
            {
                var primitive = Primitive.FromType(t);

                schema.Type = primitive.Type.ToLower();
                if (!string.IsNullOrEmpty(primitive.Format))
                {
                    schema.Format = primitive.Format.ToLower();
                }
            }
            return schema;
        }

        private static Type GetType(Type type)
        {
            if (type.IsContainer())
            {
                return type.GetElementType() ?? type.GetTypeInfo().GetGenericArguments().First();
            }

            return type;
        }

        private static IEnumerable<Type> GetOperationModels(IDictionary<string, SwaggerRouteData> metadata)
        {
            return metadata.SelectMany(d => d.Value.Types.Values);
        }
    }
}

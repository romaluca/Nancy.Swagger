﻿using System;
using Swagger.ObjectModel.Alyce;

namespace Nancy.Swagger.Alyce.Annotations.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class RouteAttribute : Attribute
    {
        public RouteAttribute()
            : this(default(HttpMethod), null)
        {
        }

        public RouteAttribute(string name)
        {
            Name = name;
        }

        public RouteAttribute(HttpMethod method, string path)
        {
            Method = method;
            Path = path;
        }

        public HttpMethod Method { get; private set; }

        public string Name { get; private set; }

        public string Notes { get; set; }

        public string Path { get; private set; }

        public Type Response { get; set; }

        public string Summary { get; set; }

        public string[] Produces { get; set; }

        public string[] Consumes { get; set; }

        public string[] Tags { get; set; }
    }
}
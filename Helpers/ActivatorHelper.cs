using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PROYECT.WebAPI.Helpers
{
    public static class ActivatorHelper
    {
        public static Object GetService(Type type)
        {
            return GlobalConfiguration.Configuration.DependencyResolver.GetService(type);
        }

        public static Object CreateInstance(Type type)
        {
            return Activator.CreateInstance(type, GetArgumentsForAction(type));
        }

        private static Object[] GetArgumentsForAction(Type type)
        {
            var arguments = new List<Object>();

            if (type.GetConstructors().Any())
                return new List<Object>().ToArray();

            foreach (var param in type.GetConstructors().First().GetParameters())
            {
                arguments.Add(GlobalConfiguration.Configuration.DependencyResolver.GetService(Type.GetType(param.ParameterType.FullName)));
            }

            return arguments.ToArray();
        }
    }
}

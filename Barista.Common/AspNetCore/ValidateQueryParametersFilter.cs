using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Barista.Common.AspNetCore
{
    public class ValidateQueryParametersFilter : ActionFilterAttribute
    {
        private static void AssertAdd(IDictionary<string, bool> supportedParams, string param, bool isArray)
        {
            if (!supportedParams.TryAdd(param, isArray))
                throw new InvalidOperationException($"The query parameter '{param}' was requested to be bound more than once");
        }

        private static void LoadQueryParameters(Type paramType, string paramName,
            IDictionary<string, bool> supportedParams, FromQueryAttribute attribute)
        {
            if (paramType.IsArray)
                AssertAdd(supportedParams, attribute?.Name ?? paramName, true);
            else if (paramType.IsClass && paramType.FullName.StartsWith("Barista"))
                foreach (var property in paramType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                    LoadQueryParameters(property.PropertyType, property.Name, supportedParams, property.GetCustomAttribute<FromQueryAttribute>()); 
            else
                AssertAdd(supportedParams, attribute?.Name ?? paramName, false);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var supportedQueryParameters = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

            if (descriptor != null)
            {
                var parameters = descriptor.MethodInfo.GetParameters();

                foreach (var parameter in parameters.Where(param => param.CustomAttributes.Any(ca => typeof(FromQueryAttribute).IsAssignableFrom(ca.AttributeType))))
                    LoadQueryParameters(parameter.ParameterType, parameter.Name, supportedQueryParameters, parameter.GetCustomAttribute<FromQueryAttribute>());

                foreach (var requestQueryParamName in context.HttpContext.Request.Query.Keys)
                {
                    if (!supportedQueryParameters.TryGetValue(requestQueryParamName, out var isArrayParameter))
                    {
                        /*if (requestQueryParamName.EndsWith("[]") && requestQueryParamName.Length > 2) TODO: binding does not work for this format
                        {
                            var withoutBrackets = requestQueryParamName.Substring(0, requestQueryParamName.Length - 2);
                            if (supportedQueryParameters.TryGetValue(withoutBrackets, out isArrayParameter) && isArrayParameter)
                                continue;
                        }*/

                        context.ModelState.AddModelError(requestQueryParamName, "The query parameter is not supported by this endpoint.");
                        continue;
                    }

                    if (isArrayParameter)
                        continue;
                    else if (context.HttpContext.Request.Query[requestQueryParamName].Count > 1)
                        context.ModelState.AddModelError(requestQueryParamName, "The query parameter cannot be specified more than once.");
                }
            }

            if (!context.ModelState.IsValid) 
                context.Result = new BadRequestObjectResult(new ValidationProblemDetails(context.ModelState));
        }
    }
}

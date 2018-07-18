using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sgw.KebabCaseRouteTokens
{
    /// <summary>
    /// Allows for "[action]" tokens in [Route()] attributes to be replaced with a kebab-case
    /// version of the action method name the RouteAttribute is applied to.
    /// Optionally allows for standard prefixes on action method names to be removed prior to kebab-casing.
    /// </summary>
    public class KebabCaseRouteTokenReplacementActionModelConvention : IActionModelConvention
    {
        private readonly IEnumerable<string> actionMethodPrefixes;

        /// <summary>
        /// Initialize the convention with optional action method prefixes to remove from path creation
        /// </summary>
        /// <param name="actionMethodPrefixes">Prefixes of action method names to be removed before kebaberization</param>
        public KebabCaseRouteTokenReplacementActionModelConvention(params string[] actionMethodPrefixes) =>
            this.actionMethodPrefixes = actionMethodPrefixes ?? (new string[] { });

        /// <summary>
        /// Initialize the convention with optional action method prefixes to remove from path creation
        /// </summary>
        /// <param name="actionMethodPrefixes">Prefixes of action method names to be removed before kebaberization</param>
        public KebabCaseRouteTokenReplacementActionModelConvention(IEnumerable<string> actionMethodPrefixes) =>
            this.actionMethodPrefixes = actionMethodPrefixes ?? (new string[] { });

        public void Apply(ActionModel actionModel)
        {
            string actionMethodName = actionModel.ActionMethod.Name;

            // If the assigned ActionName doesn't match the method name of the action
            // a custom [ActionName] attribute is being used.
            // In this case we don't want implement our conventions.
            if (actionMethodName != actionModel.ActionName)
            {
                return;
            }

            var (containsMethodPrefix, methodPrefix) = FindMethodPrefix(actionMethodName);

            string actionMethodNameSuffix = containsMethodPrefix
                ? actionMethodName.Substring(methodPrefix.Length)
                : actionMethodName;

            actionModel.ActionName = Kebaberizer.PascalToLowerKebabCase(actionMethodNameSuffix);
        }


        private (bool containsMethodPrefix, string methodPrefix) FindMethodPrefix(string actionMethodName)
        {
            string foundPrefix = actionMethodPrefixes
                .FirstOrDefault(prefix => actionMethodName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));

            return (foundPrefix != null, foundPrefix);
        }
    }
}
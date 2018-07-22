using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;

namespace Sgw.KebabCaseRouteTokens
{
    /// <summary>
    /// Allows for "[controller]" tokens in [Route()] attributes to be replaced with a kebab-case
    /// version of the controller type name the RouteAttribute is applied to.
    /// The convention is only applied if the controller type name ends with the "Controller" suffix
    /// </summary>
    public class KebabCaseRouteTokenReplacementControllerModelConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controllerModel)
        {
            bool controllerNameLacksSuffix = !controllerModel
                .ControllerType
                .Name
                .EndsWith("Controller", StringComparison.OrdinalIgnoreCase);

            // If the type name of the Controller class does not end in the "Controller" suffix
            // we don't wnat to implement our conventions.
            if (controllerNameLacksSuffix)
            {
                return;
            }

            controllerModel.RouteValues["controller"] = Kebaberizer.PascalToLowerKebabCase(controllerModel.ControllerName);
        }
    }
}

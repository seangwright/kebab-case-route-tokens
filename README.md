# kebab-case-route-tokens

ASP.NET Core model conventions to turn route tokens into kebab case urls

## Purpose

When you normally use the `Microsoft.AspNetCore.Mvc.RouteAttribute` type to annotate your Controllers and Actions you can use tokens in your route paths.

Example:

```csharp
[Route("api/[controller]")]
public class UserRegistrationController
{
    [HttpPost]
    [Route("[action]")]
    public ActionResult CreateNewUserRegistration(UserRegistration registration)
    {
        // ...
    }

    [HttpGet]
    [ActionName("Test")]
    [Route("[action]")]
    public ActionResult GetData()
    {
        // ...
    }
}
```

These tokens will automatically be replaced by Mvc with the name of the controller (in the case of `"[controller]"` without the `"Controller"` suffix).

The resulting url for the first action above would be `POST /api/UserRegistration/CreateNewUserRegistration`

If you prefer kebab-case over PascalCase for your urls and you want to use the `[controller]`/`[action]` token replacement in the `RouteAttribute`s you can use the convention classes in this project.

## Usage

In your `Startup.cs` add the Convention classes to your `MvcOptions.Conventions` collection.

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc(options =>
        {
            options
                .Conventions
                .Add(new KebabCaseRouteTokenReplacementControllerModelConvention());

            var methodNamePrefixes = new string[]
            {
                "Create", "Delete", "Update", "Get", "Find"
            };

            options
                .Conventions
                .Add(new KebabCaseRouteTokenReplacementActionModelConvention(methodNamePrefixes));
        });
    }
}
```

Any method prefixes supplied to the `KebabCaseRouteTokenReplacementActionModelConvention` will be removed from the action method name before performing kebab-case conversion.

With the above example controller and action method the resulting generated url would be `POST /api/user-registration/new-user-registration`.

Since the second action method `GetData` has a custom action name set by `[ActionName("Test")]`, no kebab-casing is performed on the action method url. The resulting url would be `GET /api/user-registration/Test`

If the `ActionNameAttribute` was not applied the resulting url would be `GET /api/user-registration/data`
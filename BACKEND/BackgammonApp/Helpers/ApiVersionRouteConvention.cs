using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace WebAPI.Helpers;

public sealed class ApiVersionRouteConvention : IApplicationModelConvention
{
    private static readonly AttributeRouteModel ApiPrefix =
        new(new RouteAttribute("api/v{version:apiVersion}"));

    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            if (!controller.Attributes.OfType<ApiControllerAttribute>().Any())
                continue;

            foreach (var selector in controller.Selectors)
            {
                if (selector.AttributeRouteModel == null)
                {
                    selector.AttributeRouteModel = ApiPrefix;
                }
                else
                {
                    selector.AttributeRouteModel =
                        AttributeRouteModel.CombineAttributeRouteModel(
                            ApiPrefix,
                            selector.AttributeRouteModel
                        );
                }
            }
        }
    }
}

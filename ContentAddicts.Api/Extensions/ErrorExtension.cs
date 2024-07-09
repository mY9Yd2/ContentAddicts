using ErrorOr;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ContentAddicts.Api.Extensions;

public static class ErrorExtension
{
    public static ModelStateDictionary ToModelStateDictionary(this List<Error> errors)
    {
        var modelStateDictionary = new ModelStateDictionary();
        errors.ForEach(error => modelStateDictionary.AddModelError(error.Code, error.Description));
        return modelStateDictionary;
    }
}

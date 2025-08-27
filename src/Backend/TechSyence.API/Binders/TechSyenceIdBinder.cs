using Microsoft.AspNetCore.Mvc.ModelBinding;
using TechSyence.Application.Services.Encoder;

namespace TechSyence.API.Binders;

public class TechSyenceIdBinder(
    IIdEncoder idEncoder
    ) : IModelBinder
{
    private readonly IIdEncoder _idEnconder = idEncoder;

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var modelName = bindingContext.ModelName;

        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None)
            return Task.CompletedTask;

        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

        var value = valueProviderResult.FirstValue;

        if (string.IsNullOrWhiteSpace(value))
            return Task.CompletedTask;

        var id = _idEnconder.Decode(value);
        bindingContext.Result = ModelBindingResult.Success(id);

        return Task.CompletedTask;
    }
}

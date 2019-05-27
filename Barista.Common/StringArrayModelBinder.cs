using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Barista.Common
{
    public class StringArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var modelName = bindingContext.ModelName;
            var modelValue = bindingContext.ValueProvider.GetValue(modelName);
            var another = bindingContext.ValueProvider.GetValue(modelName + "[]");

            if (modelValue.Length == 0 && another.Length == 0)
                return Task.CompletedTask;

            var model = modelValue.Values.AsEnumerable().Concat(another.Values.AsEnumerable()).ToArray();
            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }
    }
}

using DataTransferLayer.OtherObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Service.Generic
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ControllerGeneric<Bu, So> : ControllerBase 
        where So : new() 
        where Bu : new()
    {
        protected Bu _business;
        public So _so;

        
        public ControllerGeneric()
        {
            _business = (Bu)Activator.CreateInstance(typeof(Bu), Array.Empty<object>());
            _so = (So)Activator.CreateInstance(typeof(So), Array.Empty<object>());
        }
        
        /*public ControllerGeneric()
        {
            _business = new Bu();
            _so = new So();
        }*/
        protected DtoMessage ValidatePartDto(object dto, List<string> listField)
        {
            DtoMessage dtoMessage = new DtoMessage();
            List<string> errors = new List<string>();
    
            ModelState.Clear();
            TryValidateModel(dto);

            foreach (string fieldName in listField)
            {
                ModelState.TryGetValue(fieldName, out ModelStateEntry modelState);

                if (modelState is not null && modelState.Errors.Count > 0)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        string customErrorMessage = error.ErrorMessage;

                        if (customErrorMessage.Contains("required"))
                        {
                            customErrorMessage = "El campo es obligatorio.";
                            //customErrorMessage = $"El campo '{fieldName}' es obligatorio.";
                        }

                        if (!string.IsNullOrWhiteSpace(customErrorMessage))
                        {
                            errors.Add($"'{fieldName}': {customErrorMessage}");
                        }
                    }
                }
            }
            
            if (errors.Count > 0)
            {
                dtoMessage.ListMessage = errors;
                dtoMessage.Error();
            }

            return dtoMessage;
        }
    }
}

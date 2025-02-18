using BusinessLayer.Business.Services;
using DataTransferLayer.OtherObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Generic;
using Service.ServiceObject;

namespace Service.Controller
{
    public class ServicesController : ControllerGeneric<BusinessServices, SoServices>
    {
        public ServicesController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        public ActionResult<DtoMessage> RegisterService([FromBody] SoServices so)
        {
            try
            {
                _so.message = ValidatePartDto(so.body.dto, new List<string>()
                {
                    nameof(so.body.dto.name),
                    nameof(so.body.dto.description)
                });
                if (_so.message.ExistsMessage()) return BadRequest(_so.message);

                _so.message = _business.registerService(so.body.dto);
                if (_so.message.type == "success") return StatusCode(201, _so.message);
                if (_so.message.type == "error") return Conflict(_so.message);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (ex.InnerException != null) errorMessage += " -> " + ex.InnerException.Message;
                _so.message.ListMessage.Add(errorMessage);
                _so.message.Exception();
                return StatusCode(500, _so.message);
            }
            return BadRequest(_so.message);
        }

        [AllowAnonymous]
        [HttpPatch]
        [Route("[action]")]
        public ActionResult<DtoMessage> UpdateService([FromBody] SoServices so)
        {
            try
            {
                _so.message = ValidatePartDto(so.body.dto, new List<string>()
                {
                    nameof(so.body.dto.name),
                    nameof(so.body.dto.description)
                });
                if (_so.message.ExistsMessage()) return BadRequest(_so.message);

                _so.message = _business.updateService(so.body.dto);
                if (_so.message.type == "success") return StatusCode(201, _so.message);
                if (_so.message.type == "error") return Conflict(_so.message);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (ex.InnerException != null) errorMessage += " -> " + ex.InnerException.Message;
                _so.message.ListMessage.Add(errorMessage);
                _so.message.Exception();
                return StatusCode(500, _so.message);
            }
            return BadRequest(_so.message);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public ActionResult<SoServices> GetAllServices()
        {
            (_so.message, _so.body.listDto) = _business.GetAllServices();
            return _so;
        }
        
        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public ActionResult<SoServices> GetServicesById(Guid id)
        {
            (_so.message, _so.body.dto) = _business.GetServiceById(id);
            return _so;
        }
    }
}


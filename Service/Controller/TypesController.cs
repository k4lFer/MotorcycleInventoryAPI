using BusinessLayer.Business.Types;
using DataTransferLayer.OtherObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Generic;
using Service.ServiceObject;

namespace Service.Controller
{
    public class TypesController(BusinessTypes businessTypes) : ControllerGeneric<BusinessTypes, SoTypes>(businessTypes)
    {

        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        public ActionResult<DtoMessage> RegisterTypes([FromBody] SoTypes so)
        {
            try
            {
                _so.message = ValidatePartDto(so.body.dto, new List<string>()
                {
                    nameof(so.body.dto.name),
                    nameof(so.body.dto.description),
                });
                if(_so.message.ExistsMessage()) return BadRequest(_so.message);

                _so.message = _business.registerType(so.body.dto);
                if (_so.message.type == "success") return StatusCode(201, _so.message);
                if (_so.message.type == "error") return Conflict(_so.message);

            }catch(Exception ex)
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
        public ActionResult<DtoMessage> UpdateTypes([FromBody] SoTypes so)
        {
            try
            {
                _so.message = ValidatePartDto(so.body.dto, new List<string>()
                {
                    nameof(so.body.dto.name),
                    nameof(so.body.dto.description),
                });
                if(_so.message.ExistsMessage()) return BadRequest(_so.message);

                _so.message = _business.updateType(so.body.dto);
                if (_so.message.type == "success") return StatusCode(201, _so.message);
                if (_so.message.type == "error") return Conflict(_so.message);

            }catch(Exception ex)
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
        public ActionResult<SoTypes> GetAllTypes()
        {
            (_so.message, _so.body.listDto) = _business.GetAllTypes();
            return _so;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public ActionResult<SoTypes> GetTypesById(Guid id)
        {
            (_so.message, _so.body.dto) = _business.GetTypesById(id);
            return _so;
        }
    
    }
}


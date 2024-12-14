using BusinessLayer.Business.Motorcycle;
using DataTransferLayer.OtherObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Generic;
using Service.ServiceObject;

namespace Service.Controller
{
    public class MotorcycleController : ControllerGeneric<BusinessMotorcycle, SoMotorcycle>
    {
        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        public ActionResult<DtoMessage> Register([FromBody] SoMotorcycle so)
        {
            try
            {
                _so.message = ValidatePartDto(so.body.dto, new List<string>()
                {
                    nameof(so.body.dto.name),
                    nameof(so.body.dto.brandId),
                    nameof(so.body.dto.typeId),
                    nameof(so.body.dto.displacement),
                    nameof(so.body.dto.price),
                    nameof(so.body.dto.quantity),
                });
                if (_so.message.ExistsMessage()) return BadRequest(_so.message);

                _so.message = _business.registerMotorcycle(so.body.dto);
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
        public ActionResult<DtoMessage> Update([FromBody] SoMotorcycle so)
        {
            try
            {
                _so.message = ValidatePartDto(so.body.dto, new List<string>()
                {
                    nameof(so.body.dto.name),
                    nameof(so.body.dto.brandId),
                    nameof(so.body.dto.typeId),
                    nameof(so.body.dto.displacement),
                    nameof(so.body.dto.price),
                    nameof(so.body.dto.quantity),
                });
                if (_so.message.ExistsMessage()) return BadRequest(_so.message);

                _so.message = _business.updateMotocycle(so.body.dto);
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
        public ActionResult<DtoMessage> DisableMotorcycle(Guid id)
        {
            _so.message = _business.disableMotorcycle(id);
            return _so.message;
        }

        [AllowAnonymous]
        [HttpPatch]
        [Route("[action]")]
        public ActionResult<DtoMessage> EnableMotorcycle(Guid id)
        {
            _so.message = _business.enableMotorcycle(id);
            return _so.message;
        }
        
        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public ActionResult<SoMotorcycle> GetAllMotorcycles()
        {
            (_so.message, _so.body.listDto) = _business.GetAllMotorcycle();
            return _so;
        }
        
        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public ActionResult<SoMotorcycle> GetMotorcyclesById(Guid id)
        {
            (_so.message, _so.body.dto) = _business.GetMotorcycleId(id);
            return _so;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public ActionResult<SoMotorcycle> GetMotorcyclesByVin(string vin)
        {
            (_so.message, _so.body.dto) = _business.GetMotorcycleVin(vin);
            return _so;
        }
    }
}


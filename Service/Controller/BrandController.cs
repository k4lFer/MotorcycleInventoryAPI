using BusinessLayer.Business.Brands;
using BusinessLayer.ExternalApi;
using DataTransferLayer.OtherObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Generic;
using Service.ServiceObject;

namespace Service.Controller
{
    public class BrandController : ControllerGeneric<BusinessBrand, SoBrand>
    {
        private readonly ApisNetPe _apisNetPe;

        public BrandController(ApisNetPe apisNetPe)
        {
            _apisNetPe = apisNetPe;
        }
        
        //[Authorize(Roles = "Admin, Manager")]
        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        public ActionResult<DtoMessage> RegisterBrand([FromBody] SoBrand so)
        {
            try
            {
                _so.message = ValidatePartDto(so.body.dto, new List<string>()
                {
                    nameof(so.body.dto.name),
                    nameof(so.body.dto.ruc),
                });
                if(_so.message.ExistsMessage()) return BadRequest(_so.message);

                _so.message = _business.registerBrand(so.body.dto);
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

        //[Authorize(Roles = "Admin, Manager")]
        [AllowAnonymous]
        [HttpPatch]
        [Route("[action]")]
        public ActionResult<DtoMessage> UpdateBrand([FromBody] SoBrand so)
        {
            try
            {
                _so.message = ValidatePartDto(so.body.dto, new List<string>()
                {
                    nameof(so.body.dto.name),
                    nameof(so.body.dto.ruc),
                });
                if(_so.message.ExistsMessage()) return BadRequest(_so.message);

                _so.message = _business.updateBrand(so.body.dto);
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

        //[Authorize(Roles = "Admin, Manager")]
        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public ActionResult<SoBrand> GetAllBrands()
        {
            (_so.message, _so.body.listDto) = _business.GetAllBrands();
            return _so;
        }
        
        //[Authorize(Roles = "Admin, Manager")]
        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public ActionResult<SoBrand> GetBrandById(Guid id)
        {
            (_so.message, _so.body.dto) = _business.GetBrandById(id);
            return _so;
        }
        
        //[Authorize(Roles = "Admin, Manager")]
        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CheckByRuc(string ruc)
        {
            try
            {
                string responseBody = await _apisNetPe.CheckRucAsync(ruc);

                if (string.IsNullOrEmpty(responseBody))
                {
                    _so.message.ListMessage.Add("Error al consultar Ruc.");
                    _so.message.type = "error";
                    return StatusCode(500, _so.message);
                }

                return Content(responseBody, "application/json");
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (ex.InnerException != null) errorMessage += " -> " + ex.InnerException.Message;
                _so.message.ListMessage.Add(errorMessage);
                _so.message.Exception();
                return StatusCode(500, _so.message);
            }
        }
    }
}


using BusinessLayer.Business.User;
using BusinessLayer.ExternalApi;
using DataTransferLayer.Object;
using DataTransferLayer.OtherObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Config;
using Service.Generic;
using Service.ServiceObject;

namespace Service.Controller
{
    public class UserController : ControllerGeneric<BusinessUser, SoUser>
    {
        private readonly ApisNetPe _apisNetPe;

        public UserController(ApisNetPe apisNetPe)
        {
            _apisNetPe = apisNetPe;
        }
        
        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        [Route("[action]")]
        public ActionResult<DtoMessage> Register([FromBody] SoUser so)
        {
            try
            {
                _so.message = ValidatePartDto(so.body.dto, new List<string>
                {
                    nameof(so.body.dto.firstName),
                    nameof(so.body.dto.lastName),
                    nameof(so.body.dto.email)
                });
                if (_so.message.ExistsMessage()) return BadRequest(_so.message);

                _so.message = _business.RegisterUser(so.body.dto);
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
        
        [Authorize(Roles="Admin, Manager")]
        [HttpPut]
        [Route("[action]")]
        public ActionResult<DtoMessage> Update([FromBody] SoUser so)
        {
            try
            {
                string accessToken = Request.Headers["Authorization"].ToString();
                Guid userId = Guid.Parse(TokenUtil.GetUserIdFromAccessToken(accessToken));
                _so.message = ValidatePartDto(so.body.dto, new List<string>
                {
                    nameof(so.body.dto.firstName),
                    nameof(so.body.dto.lastName),
                    nameof(so.body.dto.email)
                });
                if (_so.message.ExistsMessage()) return BadRequest(_so.message);
                
                _so.message = _business.UpdateUser(so.body.dto, userId);
                if (_so.message.type == "success") return StatusCode(200, _so.message);
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
        
        [Authorize(Roles="Admin, Manager")]
        [HttpGet]
        [Route("[action]")]
        public ActionResult<SoUser> GetUserById(Guid userId)
        {
            (_so.message, _so.body.dto) = _business.getById(userId);
            return _so;
        }
        
        [Authorize(Roles="Admin, Manager")]
        [HttpGet]
        [Route("[action]")]
        public ActionResult<SoUser> GetUserByDocumentNumber(string documentNumber)
        {
            (_so.message, _so.body.dto) = _business.getByDocumentNumber(documentNumber);
            return _so;
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpGet]
        [Route("[action]")]
        public ActionResult GetAllByDocument(string documentNumber)
        {
            (DtoMessage message, List<string> listDocuments) = _business.GetAllByDocumentNumber(documentNumber);
            
            if (message.type == "success")
                return Ok(new { Message = message, Documents = listDocuments });
    
            if (message.type == "warning")
                return Conflict(new { Message = message, Documents = listDocuments });
            
            return StatusCode(500, new { Message = "Ocurrió un error inesperado." });
        }

        
        /*
        [Authorize(Roles = "Admin, Manager")]
        [HttpDelete]
        [Route("[action]")]
        public ActionResult<DtoMessage> Delete([FromBody] PasswordRequest so, Guid id)
        {
            try
            {
                string accessToken = Request.Headers["Authorization"].ToString();
                Guid userId = Guid.Parse(TokenUtil.GetUserIdFromAccessToken(accessToken));
                Guid ownerId = Guid.Parse(TokenUtil.GetUserIdFromAccessToken(accessToken));
                _so.message = ValidatePartDto(so, new List<string>
                {
                    nameof(so.password)
                });
                if (_so.message.ExistsMessage()) return BadRequest(_so.message);

                _so.message = _business.DeleteUser(so, id, ownerId);
                if (_so.message.type == "success") return StatusCode(200, _so.message);
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
        */
        [Authorize(Roles="Manager,Admin")]
        [HttpGet]
        [Route("[action]")]
        public ActionResult<SoUser> GetAll()
        {   
            (_so.message, _so.body.listDto) = _business.GetAll();
            return _so;
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CheckByDni(string dni)
        {
            try
            {
                string responseBody = await _apisNetPe.CheckDniAsync(dni);
                
                if (string.IsNullOrEmpty(responseBody))
                {
                    _so.message.ListMessage.Add("Error al consultar DNI.");
                    _so.message.type = "error";
                    return StatusCode(500, _so.message);
                }
                
                return Content(responseBody, "application/json");
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (ex.InnerException != null) 
                    errorMessage += " -> " + ex.InnerException.Message;

                _so.message.ListMessage.Add(errorMessage);
                _so.message.Exception();
                return StatusCode(500, _so.message);
            }
        }

        [Authorize(Roles = "Manager,Admin")]
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

using BusinessLayer.Business.User;
using BusinessLayer.ExternalApi;
using DataTransferLayer.Object;
using DataTransferLayer.Object.ResponseAPIsNet;
using DataTransferLayer.OtherObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Config;
using Service.Generic;
using Service.ServiceObject;

namespace Service.Controller
{
    public class UserController(BusinessUser businessUser) : ControllerGeneric<BusinessUser, SoUser>(businessUser)
    {
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

       // [Authorize(Roles="Manager,Admin")]
        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public ActionResult<SoUser> GetAll()
        {   
            (_so.message, _so.body.listDto) = _business.GetAll();
            return _so;
        }

        [AllowAnonymous]
        //[Authorize(Roles = "Manager,Admin")]
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CheckByDni(string dni)
        {
            (DtoMessage message, ResponseDni? responseDni) = await _business.CheckDniAsync(dni);

            if (message.type == "success")
                return Ok(new { Message = message, Dni = responseDni });
            
            if (message.type == "warning")
                return Conflict(new { Message = message, Dni = responseDni });
            
            return StatusCode(500, new { Message = "Ocurrio un error inesperado." });
        }
        
    }
}

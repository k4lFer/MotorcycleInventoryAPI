using System.Net.Http.Headers;
using BusinessLayer.Business.Owner;
using BusinessLayer.ExternalApi;
using DataTransferLayer.OtherObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Config;
using Service.Generic;
using Service.ServiceObject;

namespace Service.Controller
{
    public class OwnerController(BusinessOwner businessOwner) : ControllerGeneric<BusinessOwner, SoOwner>(businessOwner)
    {

        [Authorize(Roles="Manager,Admin")]
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<DtoMessage>> Register([FromBody] SoOwner so)
        {
            try
            {
                string accessToken = Request.Headers["Authorization"].ToString();
                Guid ownerId = Guid.Parse(TokenUtil.GetUserIdFromAccessToken(accessToken));
                _so.message = ValidatePartDto(so.body.dto, new List<string>
                {
                    nameof(so.body.dto.username),
                    nameof(so.body.dto.password),
                    nameof(so.body.dto.firstName),
                    nameof(so.body.dto.lastName),
                    nameof(so.body.dto.email),
                    nameof(so.body.dto.dni),
                    nameof(so.body.dto.ruc),
                    nameof(so.body.dto.address),
                    nameof(so.body.dto.phoneNumber),
                    nameof(so.body.dto.role)
                });
                if (_so.message.ExistsMessage()) return BadRequest(_so.message);

                _so.message = await _business.RegisterOwner(so.body.dto, ownerId);
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
        
        [Authorize(Roles="Manager,Admin,User,Guest")]
        [HttpPut]
        [Route("[action]")]
        public ActionResult<DtoMessage> Update([FromBody] SoOwner so)
        {
            try
            {
                string accessToken = Request.Headers["Authorization"].ToString();
                Guid ownerId = Guid.Parse(TokenUtil.GetUserIdFromAccessToken(accessToken));
                _so.message = ValidatePartDto(so.body.dto, new List<string>
                {
                    nameof(so.body.dto.username),
                    nameof(so.body.dto.firstName),
                    nameof(so.body.dto.lastName),
                    nameof(so.body.dto.email),
                    nameof(so.body.dto.dni),
                    nameof(so.body.dto.ruc),
                    nameof(so.body.dto.address),
                    nameof(so.body.dto.phoneNumber)
                });
                if (_so.message.ExistsMessage()) return BadRequest(_so.message);
                
                _so.message = _business.UpdateOwner(so.body.dto, ownerId);
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
        
        [Authorize(Roles="Manager,Admin,User,Guest")]
        [HttpPatch]
        [Route("[action]")]
        public ActionResult<DtoMessage> UpdateProfilePicture(IFormFile file){
            try
            {
                string accessToken = Request.Headers["Authorization"].ToString();
                Guid ownerId = Guid.Parse(TokenUtil.GetUserIdFromAccessToken(accessToken));
                _so.message = _business.UpdateProfilePicture(ownerId, file);
                if(_so.message.type == "success") return StatusCode(200, _so.message);
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

        [Authorize(Roles="Manager,Admin,User,Guest")]
        [HttpPatch]
        [Route("[action]")]
        public ActionResult<DtoMessage> ChangePassword([FromBody] DtoPassword so)
        {
            try
            {
                string accessToken = Request.Headers["Authorization"].ToString();
                Guid ownerId = Guid.Parse(TokenUtil.GetUserIdFromAccessToken(accessToken));
                _so.message = ValidatePartDto(so, new List<string>
                {
                    nameof(so.newpassword),
                    nameof(so.oldpassword)
                });
                if (_so.message.ExistsMessage()) return BadRequest(_so.message);

                if (so.newpassword == so.oldpassword)
                {
                    _so.message.AddMessage("La contraseña no puede ser la misma");
                    _so.message.Error();
                    return Conflict(_so.message);
                }
                
                _so.message = _business.ChangePassword(so, ownerId);
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
            return  BadRequest(_so.message);
        }
        
        [Authorize(Roles="Manager,Admin,User,Guest")]
        [HttpGet]
        [Route("[action]")]
        public ActionResult<SoOwner> MyProfile()
        {
            string accessToken = Request.Headers["Authorization"].ToString();
            Guid ownerId = Guid.Parse(TokenUtil.GetUserIdFromAccessToken(accessToken));
            (_so.message, _so.body.dto) = _business.MyProfile(ownerId);
            return _so;
        }
        
        [Authorize(Roles="Manager,Admin,User,Guest")]
        [HttpDelete]
        [Route("[action]")]
        public ActionResult<DtoMessage> Delete([FromBody] PasswordRequest so)
        {
            try
            {
                string accessToken = Request.Headers["Authorization"].ToString();
                Guid ownerId = Guid.Parse(TokenUtil.GetUserIdFromAccessToken(accessToken));
                _so.message = ValidatePartDto(so, new List<string>
                {
                    nameof(so.password)
                });
                if (_so.message.ExistsMessage()) return BadRequest(_so.message);

                _so.message = _business.DeleteUser(so, ownerId);
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

        [Authorize(Roles="Manager,Admin,User,Guest")]
        [HttpDelete]
        [Route("[action]")]
        public ActionResult<DtoMessage> DeletePictureProfileOwner(){
            try 
            {
                string accessToken = Request.Headers["Authorization"].ToString();
                Guid ownerId = Guid.Parse(TokenUtil.GetUserIdFromAccessToken(accessToken));
                _so.message = _business.DeleteProfilePicture(ownerId);
                if (_so.message.type == "success") return StatusCode(200, _so.message);
                if (_so.message.type == "error") return BadRequest(_so.message);
                if (_so.message.type == "conflict") return Conflict(_so.message);
            }
            catch(Exception ex)
            {
                string errorMessage = ex.Message;
                if (ex.InnerException != null) errorMessage += " -> " + ex.InnerException.Message;
                _so.message.ListMessage.Add(errorMessage);
                _so.message.Exception();
                return StatusCode(500, _so.message);
            }
            return BadRequest(_so.message);
        }

        
        [Authorize(Roles="Manager,Admin")]
        [HttpGet]
        [Route("[action]")]
        public ActionResult<SoOwner> GetAll()
        {   
            string accessToken = Request.Headers["Authorization"].ToString();
            Guid ownerId = Guid.Parse(TokenUtil.GetUserIdFromAccessToken(accessToken));
            (_so.message, _so.body.listDto) = _business.GetAll(ownerId);
            return _so;
        }
        
        [Authorize(Roles="Manager,Admin")]
        [HttpPut]
        [Route("[action]")]
        public ActionResult<DtoMessage> PromoteOrDemoteUser([FromBody] SoOwner so)
        {
            try
            {
                string accessToken = Request.Headers["Authorization"].ToString();
                Guid ownerId = Guid.Parse(TokenUtil.GetUserIdFromAccessToken(accessToken));
                if (so.body.dto.id == Guid.Empty)
                {
                    _so.message.ListMessage.Add("ID de usuario no válido.");
                    _so.message.Error();
                    return BadRequest(_so.message);
                }
                _so.message = ValidatePartDto(so, new List<string>
                {
                    nameof(so.body.dto.role),
                    nameof(so.body.dto.status)
                });
                if (_so.message.ExistsMessage()) return BadRequest(_so.message);

                _so.message = _business.PromoteOrDemoteOwner(so.body.dto, ownerId);
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
        
        //[Authorize(Roles = "Manager,Admin")]
      /*  [AllowAnonymous]
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
        }*/


    }
}

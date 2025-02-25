using BusinessLayer.Business.Authentication;
using DataTransferLayer.OtherObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Config;
using Service.Helper;
using Service.ServiceObject;
using Service.Generic;

namespace Service.Controller
{
    public class AuthenticationController(BusinessAuthentication businessAuthentication) : ControllerGeneric<BusinessAuthentication, SoAuthentication>(businessAuthentication)
    {
        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<SoAuthentication>> Login([FromBody] SoAuthentication so)
        {
            try
            {
                _so.message = ValidatePartDto(so.body.dto, new List<string>() 
                { 
                    nameof(so.body.dto.username), 
                    nameof(so.body.dto.password)
                });
                if (_so.message.ExistsMessage()) return BadRequest(_so.message);

                (_so.message, _so.body.dto) = _business.SearchByUser(so.body.dto.username);
                if (_so.body.dto.username == null)
                {
                    _so.message.AddMessage("Este usuario no se encuentra registrado en el sistema.");
                    _so.message.Error();
                    return BadRequest(_so.message);
                }
                
                if (!_so.body.dto.status)
                {
                    _so.body = null;
                    _so.message.ListMessage.Add("Esta cuenta está deshabilitado.");
                    _so.message.Error();
                    return Unauthorized(_so.message);
                }
                
                if (_business.Authenticate(_so.body.dto.password, so.body.dto.password))
                {
                    _so.body.dto.password = null;
                    _so.body.other.accessToken = await TokenUtil.GenerateAccessToken(_so.body.dto);
                    _so.body.other.refreshToken = await TokenUtil.GenerateRefreshToken(_so.body.dto);
                    _so.message.ListMessage.Add("Bienvenido al sistema.");
                    _so.message.Success();
                }
                else
                {
                    _so.body = null;
                    _so.message.ListMessage.Add("Contraseña incorrecta.");
                    _so.message.Error();
                    return Unauthorized( _so.message);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (ex.InnerException != null) errorMessage += " -> " + ex.InnerException.Message;
                _so.message.ListMessage.Add(errorMessage);
                _so.message.Exception();
                return StatusCode(500, _so.message);
            }
            return StatusCode(200, _so);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")] 
        public async Task<ActionResult<SoAuthentication>> RefreshToken([FromBody] Tokens so)
        {
            try
            {
                if (!string.IsNullOrEmpty(so.refreshToken))
                {
                    _so.body.dto = null;
                    var (tokenResponse, messages) = await TokenUtil.GenerateAccessTokenFromRefreshToken(
                        so.refreshToken, AppSettings.GetRefreshJwtSecret());
                    _so.body.other = tokenResponse;
                    _so.message = messages;
                    return _so;
                }
                _so.body = null;
                _so.message.ListMessage.Add("Proporcione el (Refresh Token).");
                _so.message.Error();
                return _so;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (ex.InnerException != null) errorMessage += " -> " + ex.InnerException.Message;
                _so.message.ListMessage.Add(errorMessage);
                _so.message.Exception();
                return StatusCode(500, _so.message);
            }
            return _so;
        }
    }
}

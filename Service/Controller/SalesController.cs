using BusinessLayer.Business.Sale;
using DataTransferLayer.OtherObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Config;
using Service.Generic;
using Service.ServiceObject;

namespace Service.Controller
{
    public class SalesController : ControllerGeneric<BusinessSales, SoSales>
    {
        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        public ActionResult<DtoMessage> CreateSale([FromBody] SoSales so){
           try
           {
                _so.message = ValidatePartDto(so.body.dto, new List<string>()
                {
                    nameof(so.body.dto.userId),
                });
                if (_so.message.ExistsMessage()) return BadRequest(_so.message);

                string accessToken = Request.Headers["Authorization"].ToString();
                Guid ownerId = Guid.Parse(TokenUtil.GetUserIdFromAccessToken(accessToken));
                so.body.dto.ownerId = ownerId;
                _so.message =  _business.CreateSale(so.body.dto);
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
        public ActionResult<SoSales> GetAllSales(){
            (_so.message, _so.body.listDto) = _business.GetAllSales();
            return _so;
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("[action]")]
        public ActionResult<SoSales> GetSaleById(Guid saleId)
        {
            try{
                (_so.message, _so.body.dto) = _business.GetSaleById(saleId);
                if (_so.message.type == "success") return Ok( _so);
                if (_so.message.type == "error") return Conflict(_so);
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

        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public ActionResult<SoSales> GetAllSalesByOwner()
        {
            string accessToken = Request.Headers["Authorization"].ToString();
            Guid ownerId = Guid.Parse(TokenUtil.GetUserIdFromAccessToken(accessToken));
            (_so.message, _so.body.dto) = _business.GetSaleByOwnerId(ownerId);
            return _so;
        }
    }
}

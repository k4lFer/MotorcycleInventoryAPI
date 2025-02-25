using BusinessLayer.Business.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Generic;
using Service.ServiceObject;

namespace Service.Controller
{
    public class DashBoardController(BusinessDashboard businessDashboard) : ControllerGeneric<BusinessDashboard, SoDashboard>(businessDashboard)
    {
    
        [Authorize(Roles = "Admin, Manager")]
        [HttpGet]
        [Route("[action]")]
        public ActionResult<SoDashboard> GetDashboard()
        {
            try
            {
                (_so.message, _so.body.dto) = _business.GetStatistics();
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
            return BadRequest(_so.message);
        }
    }
}


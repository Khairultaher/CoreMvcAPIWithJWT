using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CoreMvcAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoreMvcAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("Api/[controller]")]
    [ApiController]
    public class SafeController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IDataContext _dataContext;
        //private readonly ApplicationDbContext _dataContext;
        public SafeController(UserManager<AppUser> userManager, IDataContext dataContext)
        {
            _userManager = userManager;
            _dataContext = dataContext;
        }

        //[Route("api/students/{stdid:int?}")]
        [HttpGet]
        [Route("GetUser/{id:int?}")]
        [ClaimType("UserRole","Dev")]
        public async Task<IActionResult> GetMessageAsync( int id = 1)
        {
            var customers1 =  _dataContext.Customers.AsQueryable();
            var customers2 = _dataContext.Customers.ToList();
            var customers3 = await _dataContext.RunSqlQuery($@"
                           SELECT Id,JSON_QUERY(Name) AS Cars
                           FROM Customers
                           FOR JSON PATH, INCLUDE_NULL_VALUES");
            var customers4 = await _dataContext.Customers.FindAsync(1);
            
            // Check claim here
            //string data = "[{\"id\": 1,\"name\": [{\"firstName\": \"aaa, \"lastName\": \"ddd\"},{\"firstName\": \"ccc\",\"lastName\": \"ddd\"}]},{\"id\": 2},{\"id\": 3, \"name\": \"vvv\"}]";
            //string str = "{ 'context_name': { 'lower_bound': 'value', 'pper_bound': 'value', 'values': [ 'value1', 'valueN' ] } }";

            //var customers = customers3.AsEnumerable().Select(row => new Customer
            //{
            //    Id = row.Field<int>("Id"),
            //    Name = row.Field<string>("Name"),
            //});

            var currentUser = HttpContext.User;
            var currentUserRole = "";
            if (currentUser.HasClaim(c => c.Type == "UserRole"))
            {
                currentUserRole = currentUser.Claims.Where(w=>w.Type == "UserRole").FirstOrDefault().Value;
            }
            var user = await GetCurrentUserAsync();

            //str = customers.Where(w => w.Id == 1).FirstOrDefault().Name;

            if (user != null)
            {
                //var ss = JsonConvert.SerializeObject(customers3);
                //var myCleanJsonObject = JObject.Parse(customers3);
                return Ok(customers3);
                
            }
            return BadRequest("Error");
        }

        #region Helpers
        private async Task<AppUser> GetCurrentUserAsync()
        {
            string userName = HttpContext.User.Identity.Name;
            return await _userManager.FindByNameAsync(userName);
        }
        #endregion
    }
}

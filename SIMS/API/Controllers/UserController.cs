using ASPNETCoreApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration)
        {

            _configuration = configuration;
        }

        [HttpGet]
        public string GetUser()
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("UserAppCon").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Benutzer", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<User> userList = new List<User>();
            Response response = new Response();
            if (dt.Rows.Count > 0 )
            {
                for ( int i = 0; i < dt.Rows.Count; i++)
                {
                    User user = new User();
                    user.BenutzerID = Convert.ToInt32(dt.Rows[i]["BenutzerID"]);
                    user.BenutzerName = Convert.ToString(dt.Rows[i]["BenutzerName"]);
                    user.Rolle = Convert.ToString(dt.Rows[i]["Rolle"]);
                    user.Passwort = Convert.ToString(dt.Rows[i]["Passwort"]);
                    userList.Add(user);
                }
            }
            if (userList.Count > 0)
            {
                return JsonConvert.SerializeObject(userList);
            }
            else
            {
                response.StatusCode = 100;
                response.ErrorMessage = "No data my friend";
                return JsonConvert.SerializeObject(response);
            }
        }
    }
}

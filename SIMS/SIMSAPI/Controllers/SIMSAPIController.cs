using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIMSAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace SIMSAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SIMSAPIController : ControllerBase
	{
		private IConfiguration _configuration;
		public SIMSAPIController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[HttpGet]
		[Route("GetIncidents")]
		public JsonResult GetIncidents()
		{
			string query = "SELECT * FROM Incidents";
			DataTable dataTable = new DataTable();
			string sqlDatasource = _configuration.GetConnectionString("SIMSAPIConnection");
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDatasource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myReader = myCommand.ExecuteReader();
					dataTable.Load(myReader);
					myReader.Close();
					myCon.Close();
				}
			}
			return new JsonResult(dataTable);
		}

		[HttpPost]
		[Route("AddIncident")]
		public JsonResult AddIncident([FromForm] Incident newIncident)
		{
			string query = "INSERT INTO dbo.Incidents(bearbeiter, melder, schweregrad, bearbeitungsstatus, cve, systembetroffenheit, beschreibung, zeitstempel, eskalationslevel) VALUES (@Bearbeiter, @Melder, @Schweregrad, @Bearbeitungsstatus, @Cve, @Systembetroffenheit, @Beschreibung, @Zeitstempel, @Eskalationslevel)";
			DataTable dataTable = new DataTable();
			string sqlDatasource = _configuration.GetConnectionString("SIMSAPIConnection");
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDatasource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@Bearbeiter", newIncident.Bearbeiter);
					myCommand.Parameters.AddWithValue("@Melder", newIncident.Melder);
					myCommand.Parameters.AddWithValue("@Schweregrad", newIncident.Schweregrad);
					myCommand.Parameters.AddWithValue("@Bearbeitungsstatus", newIncident.Bearbeitungsstatus);
					myCommand.Parameters.AddWithValue("@Cve", newIncident.Cve);
					myCommand.Parameters.AddWithValue("@Systembetroffenheit", newIncident.Systembetroffenheit);
					myCommand.Parameters.AddWithValue("@Beschreibung", newIncident.Beschreibung);
					myCommand.Parameters.AddWithValue("@Zeitstempel", newIncident.Zeitstempel);
					myCommand.Parameters.AddWithValue("@Eskalationslevel", newIncident.Eskalationslevel);
					myReader = myCommand.ExecuteReader();
					dataTable.Load(myReader);
					myReader.Close();
					myCon.Close();
				}
			}
			return new JsonResult("Ok. Added.");
		}

		[HttpDelete]
		[Route("DeleteIncident")]
		public JsonResult DeleteIncident([FromForm] int incidentId)
		{
			string query = "DELETE FROM Incidents WHERE id=@incidentId";
			DataTable dataTable = new DataTable();
			string sqlDatasource = _configuration.GetConnectionString("SIMSAPIConnection");
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDatasource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@incidentId", incidentId);
					myReader = myCommand.ExecuteReader();
					dataTable.Load(myReader);
					myReader.Close();
					myCon.Close();
				}
			}
			return new JsonResult("Ok. Deleted.");

		}

        [HttpPut]
        [Route("UpdateIncident/{id}")]
        public JsonResult UpdateIncident(int id, [FromForm] Incident updatedIncident)
        {
            string query = "UPDATE dbo.Incidents SET bearbeiter = @Bearbeiter, melder = @Melder, schweregrad = @Schweregrad, bearbeitungsstatus = @Bearbeitungsstatus, cve = @Cve, systembetroffenheit = @Systembetroffenheit, beschreibung = @Beschreibung, zeitstempel = @Zeitstempel, eskalationslevel = @Eskalationslevel WHERE id = @Id";
            DataTable dataTable = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("SIMSAPIConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Id", id);
                    myCommand.Parameters.AddWithValue("@Bearbeiter", updatedIncident.Bearbeiter);
                    myCommand.Parameters.AddWithValue("@Melder", updatedIncident.Melder);
                    myCommand.Parameters.AddWithValue("@Schweregrad", updatedIncident.Schweregrad);
                    myCommand.Parameters.AddWithValue("@Bearbeitungsstatus", updatedIncident.Bearbeitungsstatus);
                    myCommand.Parameters.AddWithValue("@Cve", updatedIncident.Cve);
                    myCommand.Parameters.AddWithValue("@Systembetroffenheit", updatedIncident.Systembetroffenheit);
                    myCommand.Parameters.AddWithValue("@Beschreibung", updatedIncident.Beschreibung);
                    myCommand.Parameters.AddWithValue("@Zeitstempel", updatedIncident.Zeitstempel);
                    myCommand.Parameters.AddWithValue("@Eskalationslevel", updatedIncident.Eskalationslevel);
                    myReader = myCommand.ExecuteReader();
                    dataTable.Load(myReader);
                    myReader.Close();
                }
				query = "Insert into logs(nachricht,zeitstempel) values (@nachricht,@zeitstempel)";
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@nachricht", "incident geändert" + updatedIncident.Beschreibung);
                    myCommand.Parameters.AddWithValue("@zeitstempel", DateTime.Now);
                    myCommand.ExecuteNonQuery();
                }
                myCon.Close();

            }
            return new JsonResult("Ok. Updated.");
        }


    }
}

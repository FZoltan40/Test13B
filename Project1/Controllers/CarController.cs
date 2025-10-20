using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Project1.Controllers
{
    [Route("cars")]
    [ApiController]
    public class CarController : ControllerBase
    {
        Connect conn = new Connect();
        [HttpGet]
        public List<CarDto> GetAllData()
        {
            conn.connection.Open();
            List<CarDto> cars = new List<CarDto>();

            string sql = "SELECT * FROM cars";

            using (var cmd = new MySqlCommand(sql, conn.connection))
            {
                cmd.CommandText = sql;

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var car = new CarDto
                    {
                        Id = reader.GetInt32(0),
                        Brand = reader.GetString(1),
                        Type = reader.GetString(2),
                        License = reader.GetString(3),
                        Date = reader.GetInt32(4)
                    };
                    cars.Add(car);
                }

                conn.connection.Close();

                return cars;
            }
        }
    }
}

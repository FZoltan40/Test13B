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

        [HttpGet("getbyid")]
        public object GetCarById(int id)
        {
            conn.connection.Open();

            string sql = "SELECT * FROM cars WHERE Id=@id";

            MySqlCommand cmd = new MySqlCommand(sql, conn.connection);

            cmd.Parameters.AddWithValue("@id", id);

            MySqlDataReader dr = cmd.ExecuteReader();

            dr.Read();

            var car = new CarDto
            {
                Id = dr.GetInt32(0),
                Brand = dr.GetString(1),
                Type = dr.GetString(2),
                License = dr.GetString(3),
                Date = dr.GetInt32(4)
            };
            conn.connection.Close();

            return new { result = car };

        }

        [HttpPost]
        public object AddNewREcord(CreateCarDto createCarDto)
        {
            conn.connection.Open();

            string sql = "INSERT INTO `cars`(`Brand`, `Type`, `License`, `Date`) VALUES (@brand,@type,@license,@date)";

            MySqlCommand cmd = new MySqlCommand(sql, conn.connection);

            cmd.Parameters.AddWithValue("@brand", createCarDto.Brand);
            cmd.Parameters.AddWithValue("@type", createCarDto.Type);
            cmd.Parameters.AddWithValue("@license", createCarDto.License);
            cmd.Parameters.AddWithValue("@date", createCarDto.Date);

            cmd.ExecuteNonQuery();

            conn.connection.Close();
            return new { message = "Sikeres hozzáadás", result = createCarDto };
        }

        [HttpDelete]
        public object Delete(int id)
        {
            conn.connection.Open();

            string sql = "DELETE FROM cars WHERE Id = @id";

            MySqlCommand cmd = new MySqlCommand(sql, conn.connection);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            conn.connection.Close();

            return new { message = "Sikeres törlés" };
        }

        [HttpPut]
        public object Update(int id, CarDto carDto)
        {
            conn.connection.Open();

            string sql = "UPDATE `cars` SET `Brand`=@brand,`Type`=@type,`License`=@license,`Date`=@date WHERE Id = @id;";

            MySqlCommand cmd = new MySqlCommand(sql, conn.connection);

            cmd.Parameters.AddWithValue("@brand", carDto.Brand);
            cmd.Parameters.AddWithValue("@type", carDto.Type);
            cmd.Parameters.AddWithValue("@license", carDto.License);
            cmd.Parameters.AddWithValue("@date", carDto.Date);
            cmd.Parameters.AddWithValue("@id", id);


            cmd.ExecuteNonQuery();

            conn.connection.Close();

            return new { message = "Sikeres frissítés", result = carDto };
        }
    }
}

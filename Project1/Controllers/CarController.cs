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

        //Írjunk olyan végpontot ami visszadobja hogy hány rekord van a táblában.
        [HttpGet("numOfRecords")]
        public object GetNumberOfRecord()
        {
            conn.connection.Open();

            string sql = "SELECT Count(*) FROM `cars` AS `Db`;";
            MySqlCommand cmd = new MySqlCommand(sql, conn.connection);
            MySqlDataReader dr = cmd.ExecuteReader();

            dr.Read();

            var db = new
            {
                Count = dr.GetInt32(0)
            };

            conn.connection.Close();
            return new { message = "Sikeres lekérdezés", result = db };

        }
        //Írjon végpontot kiírja hogy adott márkából hány darab auto van az adatbázisban.
        [HttpGet("numOfBrands")]
        public object GetNumberOfBrands()
        {
            List<object> brandList = new List<object>();
            conn.connection.Open();

            string sql = "SELECT `Brand`,Count(*) FROM `cars` GROUP BY `Brand`;";
            MySqlCommand cmd = new MySqlCommand(sql, conn.connection);
            MySqlDataReader dr = cmd.ExecuteReader();



            while (dr.Read())
            {
                var brand = new
                {
                    Brand = dr.GetString(0),
                    Count = dr.GetInt32(1)
                };
                brandList.Add(brand);
            }


            conn.connection.Close();
            return new { message = "Sikeres lekérdezés", result = brandList };

        }
        //Írjon végpontot ami kilistázza  a 2020 utáni autokat
        [HttpGet("after2020")]
        public object GetBrandsAfter2020()
        {
            List<object> brandList = new List<object>();
            conn.connection.Open();

            string sql = "SELECT * FROM `cars` WHERE `date`>2020;";
            MySqlCommand cmd = new MySqlCommand(sql, conn.connection);
            MySqlDataReader dr = cmd.ExecuteReader();



            while (dr.Read())
            {
                var brand = new
                {
                    Id = dr.GetInt32(0),
                    Brand = dr.GetString(1),
                    Type = dr.GetString(2),
                    License = dr.GetString(3),
                    Date = dr.GetInt32(4)
                };
                brandList.Add(brand);
            }


            conn.connection.Close();
            return new { message = "Sikeres lekérdezés", result = brandList };

        }
        //Írjon vépontot ami megkeresi az autot amiben szerepel a "AE5SX3F03791" license szám
        [HttpGet("brandLikePattern")]
        public object GetBrandLikePattern()
        {
            conn.connection.Open();

            string sql = "SELECT `Brand`, `License` FROM `cars` WHERE `License` LIKE '%AE5SX3F03791%';";
            MySqlCommand cmd = new MySqlCommand(sql, conn.connection);
            MySqlDataReader dr = cmd.ExecuteReader();

            dr.Read();

            var license = new
            {
                Brand = dr.GetString(0),
                License = dr.GetString(1)

            };

            conn.connection.Close();
            return new { message = "Sikeres lekérdezés", result = license };

        }

        //Írjon végpontot ami minden 2010 utáni chevrolet márkájú autot hyundai-ra cseréli
        [HttpPut("replaceBrands")]
        public object UpdateBrand()
        {
            conn.connection.Open();

            string sql = "UPDATE `cars` SET `Brand`='Hyundai' WHERE `Brand`= 'chevrolet' AND `Date` > '1990';";

            MySqlCommand cmd = new MySqlCommand(sql, conn.connection);


            cmd.ExecuteNonQuery();

            conn.connection.Close();

            return new { message = "Sikeres frissítés" };
        }
    }




}


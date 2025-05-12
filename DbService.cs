using System;
using System.Data;

using MySql.Data;
using MySql.Data.MySqlClient;

namespace WebApplication1
{
    public class DbService
    {
        private readonly String _connStr = "server=localhost;user=root;database=test_db;port=3306;password=Hostpass1";
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(this._connStr);
        }

        public async Task<bool> CreateAsync(WeatherForecast weatherForecast)
        {
            using var conn = GetConnection();
            await conn.OpenAsync();
            var cmd = new MySqlCommand("INSERT INTO forcasts (Date, Temp, Summary) VALUES (@date, @temp, @summary)", conn);
            cmd.Parameters.AddWithValue("@date", weatherForecast.Date.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@temp", weatherForecast.TemperatureC);
            cmd.Parameters.AddWithValue("@summary", weatherForecast.Summary);
            var result = await cmd.ExecuteNonQueryAsync();
            Console.WriteLine("inserting into table");
            return result > 0;
        }
        public async Task<WeatherForecast> GetByDateAsync(DateTime date)
        {
            using var conn = GetConnection();
            await conn.OpenAsync();
            var cmd = new MySqlCommand("SELECT * FROM forcasts WHERE Date = @date", conn);
            cmd.Parameters.AddWithValue("@date", date);
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new WeatherForecast { 
                    Date = DateOnly.FromDateTime(reader.GetDateTime("Date")),
                    TemperatureC = reader.GetInt32("Temp"),
                    Summary = reader.GetString("Summary")
                };
            } else
            {
                return null;
            }
        }
        
        public async Task<bool> UpdateAsync(WeatherForecast weatherForecast)
        {
            using var conn = GetConnection();
            await conn.OpenAsync();
            var cmd = new MySqlCommand("UPDATE forcast SET Temp = @temp, Summary = @summary, WHERE Date = @date", conn);
            cmd.Parameters.AddWithValue("@date", weatherForecast.Date.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@temp", weatherForecast.TemperatureC);
            cmd.Parameters.AddWithValue("@summary", weatherForecast.Summary);
            int result = await cmd.ExecuteNonQueryAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(DateTime date)
        {
            using var conn = GetConnection();
            await conn.OpenAsync();
            var cmd = new MySqlCommand("DELETE FROM forcasts WHERE Date = @date", conn);
            cmd.Parameters.AddWithValue("@date", date);
            int result = await cmd.ExecuteNonQueryAsync();
            return result > 0;
        }
    }
}

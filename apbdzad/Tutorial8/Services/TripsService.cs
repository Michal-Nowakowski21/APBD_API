using Microsoft.Data.SqlClient;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public class TripsService : ITripsService
{
    private readonly string _connectionString = "Server=localhost\\SQLEXPRESS;Database=APBD1;Trusted_Connection=True;TrustServerCertificate=True;";
    
    public async Task<List<TripDTO>> GetTrips()
{
    var trips = new List<TripDTO>();
    var tripDict = new Dictionary<int, TripDTO>();
    string command = "SELECT IdTrip, Name, Description, DateFrom, DateTo, MaxPeople FROM Trip";
    
    string command2 = "SELECT Country.Name, Country_Trip.IdTrip FROM Country JOIN Country_Trip ON Country.IdCountry = Country_Trip.IdCountry";
    
    using (SqlConnection conn = new SqlConnection(_connectionString))
    {
        await conn.OpenAsync();

        using (SqlCommand cmd = new SqlCommand(command, conn))
        using (var reader = await cmd.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                int idOrdinal = reader.GetOrdinal("IdTrip");
                var trip = new TripDTO()
                {
                    Id = reader.GetInt32(idOrdinal),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    DateFrom = reader.GetDateTime(3),
                    DateTo = reader.GetDateTime(4),
                    MaxPeople = reader.GetInt32(5),
                    Countries = new List<CountryDTO>()
                };

                trips.Add(trip);
                tripDict[trip.Id] = trip;
            }
        }

        using (SqlCommand cmd = new SqlCommand(command2, conn))
        using (var reader = await cmd.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                string countryName = reader.GetString(0);
                int tripId = reader.GetInt32(1);

                if (tripDict.TryGetValue(tripId, out var trip))
                {
                    trip.Countries.Add(new CountryDTO { Name = countryName });
                }
            }
        }
    }

    return trips;
}

}
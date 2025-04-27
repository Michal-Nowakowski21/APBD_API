using Microsoft.Data.SqlClient;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public class ClientsService : IClientsService
{
    private readonly string _connectionString =
        "Server=localhost\\SQLEXPRESS;Database=APBD1;Trusted_Connection=True;TrustServerCertificate=True;";


    public async Task ClientDeleteTrip(int idClient, int IdTrip)
    {
        string com = "Delete from Client_Trip where IdClient = @idClient and IdTrip = @IdTrip";//usun wszystko z client_trip o ile id klienta i id wycieczki sa takie same jak podane

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (SqlCommand cmd = new SqlCommand(com, connection))
            {
                cmd.Parameters.AddWithValue("@idClient", idClient);
                cmd.Parameters.AddWithValue("@IdTrip", IdTrip);

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
    public async Task<bool> DoesClient_TripExist(int idClient, int tripId) 
    {
        string com = "SELECT IdClient,IdTrip FROM Client_Trip"; //wybierz id klienta i id wycieczki z client_trip
        List<int> a=new List<int>();
        List<int> b=new List<int>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (SqlCommand cmd = new SqlCommand(com, connection))
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
              
                    a.Add(reader.GetInt32(0));
                    b.Add(reader.GetInt32(1));
                }
            }
        }

        for (int i = 0; i < a.Count; i++)
        {
            if (idClient == a[i] && tripId == b[i])
            {
                return true;
            }
        }
        return false;
    }
    public async Task ClientTrip(int idClient, int tripId)
    {
        DateTime currentDateTime = DateTime.Now;
        int dateAsInt = currentDateTime.Year * 10000 + currentDateTime.Month * 100 + currentDateTime.Day;
        string com = "INSERT INTO Client_Trip (IdClient, IdTrip, RegisteredAt, PaymentDate)VALUES (@IdClient, @IdTrip, @RegisteredAt, @PaymentDate); SELECT SCOPE_IDENTITY();"; //dodaje wartosci do client_trip

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (SqlCommand cmd = new SqlCommand(com, connection))
            {
                cmd.Parameters.AddWithValue("@IdClient", idClient);
                cmd.Parameters.AddWithValue("@IdTrip", tripId);
                cmd.Parameters.AddWithValue("@RegisteredAt", dateAsInt);
                cmd.Parameters.AddWithValue("@PaymentDate", DBNull.Value); 

                var result = await cmd.ExecuteScalarAsync();
            }
        }
    }
    public async Task CreateClient(ClientDTO client)
    {
        string com = "INSERT INTO Client (FirstName, LastName, Email, Telephone, Pesel) VALUES (@FirstName,@LastName, @Email, @Telephone, @Pesel)SELECT SCOPE_IDENTITY();"; // dodaje wartosci do client

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (SqlCommand cmd = new SqlCommand(com, connection))
            {
                cmd.Parameters.AddWithValue("@FirstName", client.FirstName);
                cmd.Parameters.AddWithValue("@LastName", client.LastName);
                cmd.Parameters.AddWithValue("@Email", client.Email);
                cmd.Parameters.AddWithValue("@Telephone", client.Telephone);
                cmd.Parameters.AddWithValue("@Pesel", client.Pesel);
                var res = await cmd.ExecuteScalarAsync();
                client.IdClient = Convert.ToInt32(res);
            }
        }
        
    }
    public async Task<bool> DoesClientExist(int id)
    {
        string com = "SELECT IdClient FROM Client"; //wybiera id clienta z client
        List<int> a=new List<int>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (SqlCommand cmd = new SqlCommand(com, connection))
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
              
                    a.Add(reader.GetInt32(0));

                }
            }
        }

        for (int i = 0; i < a.Count; i++)
        {
            if (id == a[i])
            {
                return true;
            }
        }
        return false;
    }
    public async Task<bool> DoesTripExist(int id)
    {
        string com = "SELECT IdTrip FROM Trip"; // wybiera id wycieczki z wycieczki
        List<int> a=new List<int>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (SqlCommand cmd = new SqlCommand(com, connection))
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
              
                    a.Add(reader.GetInt32(0));

                }
            }
        }

        for (int i = 0; i < a.Count; i++)
        {
            if (id == a[i])
            {
                return true;
            }
        }
        return false;
    }
    public async Task<bool> MaxPeopleCount(int id)
    {
        string com = "SELECT t.MaxPeople, COUNT(ct.IdClient) AS CurrentPeopleCount FROM Trip t JOIN Client_Trip ct ON t.IdTrip = ct.IdTrip WHERE t.IdTrip = @IdTrip GROUP BY t.IdTrip, t.MaxPeople"; //wybiera max ilosc osob oraz liczy ile jest juz osob na wycieczce z tabeli trip i client_trip
        var max=0;
        var currentcount = 0;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (SqlCommand cmd = new SqlCommand(com, connection))
            {
                cmd.Parameters.AddWithValue("@IdTrip", id);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        max = reader.GetInt32(0);
                        currentcount = reader.GetInt32(1);
                    }
                }
            }
                
            
        }

        if (currentcount>=max)
        {
            return true;
        }
        return false;
    }
    public async Task<List<TripforclientDTO>> GetClients(int id)
    {
        var trips = new List<TripforclientDTO>();
        string command = "SELECT Trip.IdTrip, Trip.Name, Trip.Description, Trip.DateFrom, Trip.DateTo, Trip.MaxPeople, Client_Trip.RegisteredAt, Client_Trip.PaymentDate FROM Client JOIN Client_Trip ON Client_Trip.IdClient = Client.IdClient JOIN Trip ON Trip.IdTrip = Client_Trip.IdTrip WHERE Client.IdClient = @IdClient"; // wybiera wybrane wartosci z 3 tabel

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            using (SqlCommand cmd = new SqlCommand(command, conn))
            {
                cmd.Parameters.AddWithValue("@IdClient", id);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var trip = new TripforclientDTO()
                        {
                            idTrip = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            DateFrom = reader.GetDateTime(3),
                            DateTo = reader.GetDateTime(4),
                            MaxPeople = reader.GetInt32(5),
                            RegisteredAt = reader.GetInt32(6),
                        };
                        trips.Add(trip);
                    }
                }
            }
        }

        return trips;
    }
}

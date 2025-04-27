using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public interface IClientsService
{
    Task<List<TripforclientDTO>> GetClients(int id);
    Task<bool> DoesClientExist(int id);
    Task<bool> DoesTripExist(int id);
    Task<bool> DoesClient_TripExist(int id, int tripId);
    Task<bool> MaxPeopleCount(int id);
    Task CreateClient(ClientDTO client);
    Task ClientTrip(int idClient, int tripId);
    Task ClientDeleteTrip(int idClient, int tripId);
}
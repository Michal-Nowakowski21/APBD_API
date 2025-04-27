using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tutorial8.Models.DTOs;
using Tutorial8.Services;

namespace Tutorial8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientsService _clientsService;

        public ClientsController(IClientsService clientsService)
        {
            _clientsService = clientsService;
        }

        [HttpGet("{id}/trips")]
        public async Task<IActionResult> GetTripsClients(int id) //2.GET /api/clients/{id}/trips pobiera wszystkie wycieczki powiazane z klientem o danym id
        {
            if(! await _clientsService.DoesClientExist(id)){ 
                return NotFound();
            }
            var clients = await _clientsService.GetClients(id);
            return Ok(clients);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] ClientDTO client)//3. POST /api/clients tworzy nowego klienta
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
               
                await _clientsService.CreateClient(client);

           
                return CreatedAtAction(nameof(GetTripsClients), new { id = client.IdClient }, new { IdClient = client.IdClient });
            }
            catch (Exception ex)
            {
              
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut("{idClient}/trips/{tripId}")]
        public async Task<IActionResult> ClientTrip(int idClient, int tripId) //4. PUT /api/clients/{idClient}/trips/{tripsId} rejestruje klienta na konkretna wycieczke
        {
            if(! await _clientsService.DoesClientExist(idClient)){ 
                return NotFound();
            }

            if (!await _clientsService.DoesTripExist(tripId))
            {
                return NotFound();
            }

            if (await _clientsService.MaxPeopleCount(tripId))
            {
                return BadRequest();
            }
            await _clientsService.ClientTrip(idClient, tripId);
            return Created();
        }

        [HttpDelete("{idClient}/trips/{tripId}")]
        public async Task<IActionResult> DeleteClient(int idClient, int tripId) //5. DELETE /api/clients/{idClient}/trips/{IdTrip} usuwa klienta z wycieczki
        {
            if(! await _clientsService.DoesClient_TripExist(idClient,tripId)){ 
                return NotFound();
            }
            await _clientsService.ClientDeleteTrip(idClient, tripId);
            return Ok();
        }
        
    }
}
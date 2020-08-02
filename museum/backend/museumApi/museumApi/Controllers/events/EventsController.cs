using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using museum.Application.main.events;
using museum.Application.main.events.dto;
using System;

namespace museumApi.Controllers.events
{
    [Authorize]
    [Route("/api/[controller]")]
    public class EventsController : Controller
    {
        private readonly IEventService _service;
        public EventsController(IEventService service)
        {
            _service = service;
        }
        /// <summary>
        /// Gets all events
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var s = _service.GetAllEvents();
                return Ok(s);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Get event by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetEventById(int id)
        {
            try
            {
                var sub = _service.GetEventById(id);
                if (sub != null)
                    return new ObjectResult(sub);
                return NotFound();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Source);
            }
            return NotFound();

        }

        /// <summary>
        /// Add new event
        /// </summary>
        /// <param name="Event"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] ReservationDTO s)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var sub = _service.PostEvent(s);
                return Ok();

            }
            catch (Exception err)
            {
                return BadRequest(err);
            }  
        }

        /// <summary>
        /// Edit event
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Event"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ReservationDTO s)
        {
            try
            {
                _service.UpdateEvent(id, s);
                var result = _service.GetEventById(id);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Delete event
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteEvent(int id)
        {
            try
            {
                var sub = _service.GetEventById(id);
                _service.DeleteEvent(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
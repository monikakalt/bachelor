using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using museum.Application.main.teachers;
using museum.Application.main.teachers.dto;

namespace museumApi.Controllers.teacher
{
    [Authorize]
    [Route("/api/[controller]")]
    public class TeachersController : Controller
    {
        private readonly ITeachersService _service;
        public TeachersController(ITeachersService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets all Teachers
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            var s = _service.GetAllTeachers();
            return Ok(s);
        }

        /// <summary>
        /// Get Teacher by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetTeacherById(int id)
        {
            try
            {
                var sub = _service.GetTeacherById(id);
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
        /// Add new Teacher
        /// </summary>
        /// <param name="Teacher"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] TeacherDTO request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var sub = _service.PostTeacher(request);
                return Created("Teachers/" + sub.Id, sub);

            }
            catch (Exception err)
            {
                return BadRequest(err);
            }
        }

        /// <summary>
        /// Edit Teacher
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Teacher"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] TeacherDTO request)
        {
            try
            {
                _service.UpdateTeacher(id, request);
                var result = _service.GetTeacherById(id);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Delete Teacher
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteTeacher(int id)
        {
            try
            {
                var sub = _service.GetTeacherById(id);
                _service.DeleteTeacher(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
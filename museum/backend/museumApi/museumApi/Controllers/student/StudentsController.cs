using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using museum.Application.main;
using museum.Application.main.classes;
using museum.Application.main.students.dto;
using museum.Application.main.teachers;
using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace museumApi.Controllers.student
{
    [Authorize]
    [Route("/api/[controller]")]
    public class StudentsController : Controller
    {
        private readonly IStudentsService _service;
        private readonly IClassesService _classesService;
        private readonly ITeachersService _teachersService;
        private readonly IUsersService _userService;

        public StudentsController(IStudentsService service, IUsersService userService, ITeachersService teachersService, IClassesService classesService)
        {
            _service = service;
            _userService = userService;
            _classesService = classesService;
            _teachersService = teachersService;
        }

        [AllowAnonymous]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm(Name = "file")] IFormFile file)
        {
            if (file.Length == 0)
                return BadRequest();
            else
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream).ConfigureAwait(false);

                    using (var package = new ExcelPackage(memoryStream))
                    {
                        try
                        {
                            var worksheets = package.Workbook.Worksheets;
                            _service.ReadExcelPackageToString(package, worksheets);
                            return Ok();
                        }
                        catch (Exception e)
                        {
                            return BadRequest(e);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets all students
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var classes = _classesService.GetAllClasses();
                var s = _service.GetAllStudents();
                if (s.Any())
                {
                    return Ok(s);
                } 
                else
                {
                    return NotFound();
                }
               
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Get student by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            try
            {
                var classes = _classesService.GetAllClasses();
                var sub = _service.GetStudentById(id);
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
        /// Add new student
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult PostStudent([FromBody] StudentDTO request)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            try
            {
                var a = _classesService.GetClassById(request.FkClass);
                var sub = this._service.PostStudent(request);
                return Ok();

            }
            catch (Exception err)
            {
                return BadRequest(err);
            }
        }

        /// <summary>
        /// Edit student
        /// </summary>
        /// <param name="id"></param>
        /// <param name="student"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] StudentDTO request)
        {
            try
            {
                this._service.UpdateStudent(id, request);
                var result = this._service.GetStudentById(id);
                return this.Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Delete student
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            try
            {
                var sub = _service.GetStudentById(id);
                _service.DeleteStudent(id);
                return this.NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
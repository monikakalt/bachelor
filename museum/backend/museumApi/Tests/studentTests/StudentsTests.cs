using museum.Application.main.students.dto;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;

namespace Tests.studentTests
{
    [TestFixture]
    public class StudentsTests: ApiBaseTest
    {
        private readonly string prefix = "students/";

        [Test]
        public void GetStudentById()
        {
            var stud = new StudentDTO
            {
                Id = 1,
                FullName = "Monika Kaltenyte",
                Email = "kaltenytmonika3@gmail.com",
                Comment = "Test"
            };

            var url = this.url + this.prefix + stud.Id;
            var fetched = this.Get<StudentDTO>(url, true);
            Assert.NotNull(fetched);
            Assert.AreEqual(stud.FullName, fetched.FullName);
            Assert.AreEqual(stud.Email, fetched.Email);
            Assert.AreEqual(stud.Comment, fetched.Comment);

        }

        [Test]
        public void DeleteStudent()
        {
            var stud = new StudentDTO
            {
                Id = 1,
                FullName = "Monika Kaltenyte",
                Email = "kaltenytmonika3@gmail.com",
                Comment = "Test"
            };

            var url = this.url + this.prefix + stud.Id;

            try
            {
                this.Delete(this.url);
            }
            catch
            {
                Assert.Fail("Deleting student failed");
                return;
            }
            // Check that after deletion we really can't see any chronicle.
            var afterDelete = this.Get<IEnumerable<StudentDTO>>(this.url);
            if (afterDelete.Any())
            {
                Assert.Fail("There are still student present. Deleting student failed");
            }
        }

        [Test]
        public void GetAllStudents()
        {
            try
            {
                var students = this.Get<IEnumerable<StudentDTO>>(this.url);
                Assert.NotNull(students);

            }
            catch (Exception)
            {
                Assert.Fail("Reading students via API failed!");
                return;
            }
        }

        [Test]
        public void UpdateStudent()
        {
    
            var stud = new StudentDTO
            {
                Id = 1,
                FullName = "Kaltenyte",
                Email = "kaltenytmonika3@gmail.com",
                Comment = "Update"
            };
            try
            {
                var chronicles = this.Put(this.url, stud.Id, stud, true);

            }
            catch
            {
                Assert.Fail("Updating student via API failed!");
                return;
            }

            //check if it was updated
            var url = this.url + this.prefix + stud.Id;
            var fetched = this.Get<StudentDTO>(url, true);

            Assert.NotNull(fetched);
            Assert.AreEqual(stud.FullName, fetched.FullName);
            Assert.AreEqual(stud.Comment, fetched.Comment);
        }

        [Test]
        public void CreateStudent()
        {
            var stud = new StudentDTO
            {
                FullName = "Kaltenyte",
                Email = "kaltenytmonika3@gmail.com",
                Comment = "Create"
            };
            try
            {
                this.Post(this.url, stud);
            }

            catch (Exception)
            {
                Assert.Fail("Posting student via API failed!");
                return;
            }
        }
    }
}

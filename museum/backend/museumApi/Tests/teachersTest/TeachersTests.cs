using museum.Application.main.teachers.dto;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests.teachersTest
{
    [TestFixture]
    public class TeachersTests: ApiBaseTest
    {
        private readonly string prefix = "teachers/";
        public TeachersTests()
        {
            this.Authorize();
        }

        [Test]
        public void GetTeacherById()
        {
            try
            {
                var t = new TeacherDTO
                {
                    Id = 1,
                    FullName = "test"
                };

                var url = this.url + this.prefix + t.Id;
                var fetched = this.Get<TeacherDTO>(url, true);
                Assert.NotNull(fetched);
                Assert.AreEqual(t.FullName, fetched.FullName);
                Assert.AreEqual(t.Id, fetched.Id);
            }
            catch
            {
                Assert.Fail("Reading teacher via API failed!");
                return;
            }
        }

        [Test]
        public void DeleteTeacher()
        {
            var t = new TeacherDTO
            {
                Id = 1,
                FullName = "test"
            };

            var url = this.url + this.prefix + t.Id;

            try
            {
                this.Delete(this.url);
            }
            catch
            {
                Assert.Fail("Deleting teacher failed");
                return;
            }
            // Check that after deletion we really can't see any teachers.
            var gradsAfterDelete = this.Get<IEnumerable<TeacherDTO>>(this.url);
            if (gradsAfterDelete.Any())
            {
                Assert.Fail("There are still teachers present. Deleting graduates failed");
            }
        }

        [Test]
        public void GetAllTeachers()
        {
            try
            {
                var t = this.Get<IEnumerable<TeacherDTO>>(this.url);
                Assert.NotNull(t);

            }
            catch
            {
                Assert.Fail("Reading teachers via API failed!");
                return;
            }
        }

        [Test]
        public void UpdateTeacher()
        {
            var t = new TeacherDTO
            {
                Id = 1,
                FullName = "update"
            };

            try
            {
               this.Put(this.url, t.Id, t, true);
            }
            catch (Exception)
            {
                Assert.Fail("Updating chronicle via API failed!");
                return;
            }

            //check if it was updated
            var url = this.url + this.prefix + t.Id;
            var fetched = this.Get<TeacherDTO>(url, true);

            Assert.NotNull(fetched);
            Assert.AreEqual(t.Id, fetched.Id);
            Assert.AreEqual(t.FullName, fetched.FullName);
        }

        [Test]
        public void AddNewGrad()
        {
            var t = new TeacherDTO
            {
                FullName = "create"
            };

            try
            {
                var chronicles = this.Post(this.url, t);
            }

            catch (Exception)
            {
                Assert.Fail("Posting teachers via API failed!");
                return;
            }
        }
    }
}

using AutoMapper;
using museum.Application.main.classes.dto;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests.classesTests
{
    [TestFixture]
    public class ClassesTests : ApiBaseTest
    {
        private readonly string prefix = "schoolclasses/";
        protected readonly IMapper mapper;
        public ClassesTests()
        {
            this.Authorize();
        }
        [Test]
        public void AddNewClass()
        {
            var newClass = new ClassDTO
            {
                Title = "test",
            };

            try
            {
                var chronicles = this.Post(this.url, newClass);
            }

            catch (Exception)
            {
                Assert.Fail("Posting class via API failed!");
                return;
            }
        }

        [Test]
        public void GetClassById()
        {
            try
            {
                var classInfo = new ClassDTO
                {
                    Id = 1,
                    Title = "test",
                };

                var url = this.url + this.prefix + classInfo.Id;
                var fetchedclassInfo = this.Get<ClassDTO>(url, true);
                Assert.NotNull(fetchedclassInfo);
                Assert.AreEqual(classInfo.Title, fetchedclassInfo.Title);
       
            }

            catch
            {

                Assert.Fail("Reading class via API failed!");
                return;
            }
        }

        [Test]
        public void DeleteClass()
        {
            var url = this.url + this.prefix + 1;

            try
            {
                this.Delete(this.url);
            }
            catch
            {
                Assert.Fail("Deleting chronicle failed");
                return;
            }
            // Check that after deletion we really can't see any chronicle.
            var classAfterDelete = this.Get<IEnumerable<ClassDTO>>(this.url);
            if (classAfterDelete.Any())
            {
                Assert.Fail("There are still classes present. Deleting chronicle failed");
            }
        }

        [Test]
        public void GetAllClasses()
        {
            try
            {
                var url = this.url + this.prefix ;
                var getClass = this.Get<ClassDTO>(url, true);
                Assert.NotNull(getClass);
            }
            catch
            {
                Assert.Fail("Reading classes via API failed!");
                return;
            }
        }

        [Test]
        public void UpdateClass()
        {
            var classInfo = new ClassDTO
            {
                Id = 1,
                Title = "test update",
            };

            try
            {
                var cl = this.Put(this.url, classInfo.Id, classInfo, true);
            }
            catch (Exception)
            {
                Assert.Fail("Updating chronicle via API failed!");
                return;
            }

            //check if it was updated
            var url = this.url + this.prefix + classInfo.Id;
            var fetchedClass = this.Get<ClassDTO>(url, true);

            Assert.NotNull(fetchedClass);
            Assert.AreEqual(classInfo.Title, fetchedClass.Title);
        }
    }
}

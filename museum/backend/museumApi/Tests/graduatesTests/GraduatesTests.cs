using museum.Application.main.graduates.dto;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests.graduatesTests
{
    [TestFixture]
    public class GraduatesTests: ApiBaseTest
    {
        private readonly string prefix = "graduates/";
        public GraduatesTests()
        {
            this.Authorize();
        }

        [Test]
        public void GetGraduatesById()
        {
            try
            {
                var grad = new GraduatesDTO
                {
                    Id = 1,
                    Title = "test",
                    Year = 2015
                };

                var url = this.url + this.prefix + grad.Id;
                var fetchedGrad = this.Get<GraduatesDTO>(url, true);
                Assert.NotNull(fetchedGrad);
                Assert.AreEqual(grad.Title, fetchedGrad.Title);
                Assert.AreEqual(grad.Id, fetchedGrad.Id);
                Assert.AreEqual(grad.Year, fetchedGrad.Year);

                // TO DO check with students
            }
            catch
            {

                Assert.Fail("Reading graduate via API failed!");
                return;
            }
        }

        [Test]
        public void DeleteGraduates()
        {
            var grad = new GraduatesDTO
            {
                Id = 1,
                Title = "test",
                Year = 2015
            };
            var url = this.url + this.prefix + grad.Id;

            try
            {
                this.Delete(this.url);
            }
            catch
            {
                Assert.Fail("Deleting graduates failed");
                return;
            }
            // Check that after deletion we really can't see any chronicle.
            var gradsAfterDelete = this.Get<IEnumerable<GraduatesDTO>>(this.url);
            if (gradsAfterDelete.Any())
            {
                Assert.Fail("There are still graduates present. Deleting graduates failed");
            }
        }

        [Test]
        public void GetAllGraduates()
        {
            try
            {
                var grads = this.Get<IEnumerable<GraduatesDTO>>(this.url);
                Assert.NotNull(grads);

            }
            catch (Exception)
            {
                Assert.Fail("Reading graduates via API failed!");
                return;
            }
        }

        [Test]
        public void UpdateGraduates()
        {
            var grad = new GraduatesDTO
            {
                Id = 1,
                Title = "test update",
                Year = 2016
            };

            try
            {
                var chronicles = this.Put(this.url, grad.Id, grad, true);
            }
            catch (Exception)
            {
                Assert.Fail("Updating chronicle via API failed!");
                return;
            }

            //check if it was updated
            var url = this.url + this.prefix + grad.Id;
            var fetched = this.Get<GraduatesDTO>(url, true);

            Assert.NotNull(fetched);
            Assert.AreEqual(grad.Title, fetched.Title);
            Assert.AreEqual(grad.Year, fetched.Year);
        }

        [Test]
        public void AddNewGrad()
        {
            var grad = new GraduatesDTO
            {
                Title = "test",
            };

            try
            {
                var chronicles = this.Post(this.url, grad);
            }

            catch (Exception)
            {
                Assert.Fail("Posting graduates via API failed!");
                return;
            }
        }
    }
}

using museum.Application.main.chronicles.dto;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests.chroniclesTests
{
    [TestFixture]
    public class ChroniclesTests : ApiBaseTest
    {
        private readonly string prefix = "chronicles/";

        public ChroniclesTests()
        {
            this.Authorize();
        }

        [Test]
        public void GetChronicleById()
        {
            try
            {
                var chronicle = new ChronicleDTO
                {
                    Id = 1,
                    Title = "test",
                    Date = new DateTime(2019, 5, 23),
                    FolderUrl = "../test",
                    FkUser = 1
                };
                var url = this.url + this.prefix + chronicle.Id;
                var fetchedChronicle = this.Get<ChronicleDTO>(url, true);
                Assert.NotNull(fetchedChronicle);
                Assert.AreEqual(chronicle.Title, fetchedChronicle.Title);
                Assert.AreEqual(chronicle.Date, fetchedChronicle.Date);
                Assert.AreEqual(chronicle.FolderUrl, fetchedChronicle.FolderUrl);
            }
            catch
            {
                Assert.Fail("Reading chronicle via API failed!");
                return;
            }

        }
        [Test]
        public void GetRecentChronicles()
        {
            var url = this.url + this.prefix + "recent";
            var fetchedChronicle = this.Get<ChronicleDTO>(url, true);
            Assert.NotNull(fetchedChronicle);

            // TO DO: calculate photos
        }

        [Test]
        public void DeleteChronicle()
        {
            var chronicle = new ChronicleDTO
            {
                Id = 1,
                Title = "test",
                Date = new DateTime(2019, 5, 23),
                FolderUrl = "../test",
                FkUser = 1
            };
            var url = this.url + this.prefix + chronicle.Id;

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
            var chronicleAfterDelete = this.Get<IEnumerable<ChronicleDTO>>(this.url);
            if (chronicleAfterDelete.Any())
            {
                Assert.Fail("There are still chronicles present. Deleting chronicle failed");
            }

        }

        [Test]
        public void GetAllChronicles()
        {
            try
            {
                var chronicles = this.Get<IEnumerable<ChronicleDTO>>(this.url);
                Assert.NotNull(chronicles);

            }
            catch (Exception)
            {
                Assert.Fail("Reading chronicles via API failed!");
                return;
            }
        }

        [Test]
        public void UpdateChronicle()
        {
            var chronicle = new ChronicleDTO
            {
                Id = 1,
                Title = "test update",
                Date = new DateTime(2019, 5, 23),
                FolderUrl = "../test",
                FkUser = 1
            };

            try
            {
                var chronicles = this.Put(this.url, chronicle.Id, chronicle, true);
            }
            catch (Exception)
            {
                Assert.Fail("Updating chronicle via API failed!");
                return;
            }

            //check if it was updated
            var url = this.url + this.prefix + chronicle.Id;
            var fetchedChronicle = this.Get<ChronicleDTO>(url, true);

            Assert.NotNull(fetchedChronicle);
            Assert.AreEqual(chronicle.Title, fetchedChronicle.Title);
        }

        [Test]
        public void AddChronicle()
        {
            var chronicle = new ChronicleDTO
            {
                Id = 1,
                Title = "test update",
                Date = new DateTime(2019, 5, 23),
                FolderUrl = "../test",
                FkUser = 1
            };

            try
            {
                var chronicles = this.Post(this.url, chronicle);
            }
            catch (Exception)
            {
                Assert.Fail("Posting chronicle via API failed!");
                return;
            }
        }
    }
}

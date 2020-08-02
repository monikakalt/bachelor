using museum.Application.main.events.dto;
using museum.Application.main.mailing;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests.reservationsTests
{
    [TestFixture]
    public class ReservationsTests: ApiBaseTest
    {
        private readonly string prefix = "events/";
        protected readonly IMailingService _mailingService;
        public ReservationsTests(IMailingService mailingService)
        {
            this.Authorize();
            _mailingService = mailingService;

        }

        [Test]
        public void PostReservation()
        {
            var reservation = new ReservationDTO
            {
                Title = "test",
                Start = new DateTime(2019, 11, 12, 12, 0, 0),
                End = new DateTime(2019, 11, 12, 13, 0, 0),
            };

            try
            {
                var chronicles = this.Post(this.url, reservation);
            }

            catch (Exception)
            {
                Assert.Fail("Posting reservation via API failed!");
                return;
            }
        }

        [Test]
        public void GetReservationById()
        {
            try
            {
                var reservation = new ReservationDTO
                {
                    Id = 1,
                    Start = new DateTime(2019, 11, 12, 12, 0, 0),
                    End = new DateTime(2019, 11, 12, 13, 0, 0),
                    Title = "test"
                };
                var url = this.url + this.prefix + reservation.Id;
                var fetched = this.Get<ReservationDTO>(url, true);
                Assert.NotNull(fetched);
                Assert.AreEqual(reservation.Title, fetched.Title);
                Assert.AreEqual(reservation.Start, fetched.Start);
                Assert.AreEqual(reservation.End, fetched.End);
            }
            catch
            {
                Assert.Fail("Reading reservation via API failed!");
                return;
            }
        }

        [Test]
        public void DeleteReservation()
        {
            try
            {
                var url = this.url + this.prefix + 1;
              //  var getClass = this.Get<ReservationsDTO>(url, true);
            }
            catch
            {
                Assert.Fail("Reading initiative via API failed!");
                return;
            }
        }

        [Test]
        public void GetAllReservations()
        {
            try
            {
                var events = this.Get<IEnumerable<ReservationDTO>>(this.url);
                Assert.NotNull(events);

            }
            catch (Exception)
            {
                Assert.Fail("Reading events via API failed!");
                return;
            }
        }

        [Test]
        public void UpdateReservation()
        {
            var reservation = new ReservationDTO
            {
                Id = 1,
                Start = new DateTime(2012, 11, 12, 12, 0, 0),
                End = new DateTime(2020, 11, 12, 13, 0, 0),
                Title = "update"
            };

            try
            {
                var reservationUpdate = this.Put(this.url, reservation.Id, reservation, true);
            }
            catch (Exception)
            {
                Assert.Fail("Updating chronicle via API failed!");
                return;
            }

            //check if it was updated
            var url = this.url + this.prefix + reservation.Id;
            var fetched = this.Get<ReservationDTO>(url, true);

            Assert.NotNull(fetched);
            Assert.AreEqual(reservation.Title, fetched.Title);
            Assert.AreEqual(reservation.Start, fetched.Start);
            Assert.AreEqual(reservation.End, fetched.End);
        }

        [Test]
        public void SendDeleteEmail()
        {
            try
            {
                var reservation = new ReservationDTO
                {
                    Id = 1,
                    Start = new DateTime(2012, 11, 12, 12, 0, 0),
                    End = new DateTime(2020, 11, 12, 13, 0, 0),
                    Title = "update",
                    Email = "kaltenytmonika3@gmail.com"
                };

                _mailingService.SendDeleteEmail(reservation);
            }
            catch (Exception)
            {

                Assert.Fail("Sending delete failed!");
                return;
            }
        }

        [Test]
        public void SendUpdateEmail()
        {
            try
            {
                var reservation = new ReservationDTO
                {
                    Id = 1,
                    Start = new DateTime(2012, 11, 12, 12, 0, 0),
                    End = new DateTime(2020, 11, 12, 13, 0, 0),
                    Title = "update",
                    Email = "kaltenytmonika3@gmail.com"
                };

                _mailingService.SendUpdateEmail(reservation);
            }
            catch (Exception)
            {

                Assert.Fail("Sending delete failed!");
                return;
            }
        }
    }
}

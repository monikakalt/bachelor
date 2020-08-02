using Hangfire;
using Microsoft.Extensions.Options;
using museum.Application.helpers;
using museum.Application.main.events.dto;
using museum.EF.repositories;
using museumApi.EF.entities;
using System;
using System.Net;
using System.Net.Mail;

namespace museum.Application.main.mailing
{
    public class MailingService: IMailingService
    {
        private readonly IOptions<AppSettings> _config;
        private readonly IEventRepository _eventRepository;
        public MailingService(IOptions<AppSettings> config, IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
            _config = config;
        }

        public void FireTheJob(int inverval)
        {
            RecurringJob.AddOrUpdate<IMailingService>(
                e => e.ManageQueue(),
                Cron.MinuteInterval(inverval));
        }

        public void ManageQueue()
        {
            var reservations = _eventRepository.GetEvents();

            foreach (var r in reservations)
            {
                if (r.ReminderSent != 1 && !String.IsNullOrEmpty(r.Email))
                {
                    SendReminderEmail(r);
                }
            }
        }

        public void SendReminderEmail(Reservation item)
        {
            try
            {
                var start = item.Start.ToString().Substring(0, Math.Max(0, item.Start.ToString().Length - 3));
                var end = item.End.ToString().Substring(0, Math.Max(0, item.End.ToString().Length - 3));
                string mailBodyhtml = "<div><p> Sveiki,</p> <br><p> Primename, kad esate užsirezervavę muziejaus patalpas nuo:  "
                    + start + " iki: " + end +
                    ".</p><br><p>Pagarbiai</p><p>JJG Muziejaus administracija</p></div>";
                var msg = new MailMessage(_config.Value.SenderEmail, "kaltenytmonika3@gmail.com", _config.Value.Subject, mailBodyhtml);
                msg.To.Add("kaltenytmonika3@gmail.com");
                msg.IsBodyHtml = true;
                var smtpClient = new System.Net.Mail.SmtpClient(_config.Value.SmtpClient, Int32.Parse(_config.Value.Port))
                {
                    UseDefaultCredentials = true,
                    Credentials = new NetworkCredential(_config.Value.SenderEmail, _config.Value.Password),
                    EnableSsl = true
                }; //**if your from email address is "from@hotmail.com" then host should be "smtp.hotmail.com"**
                smtpClient.Send(msg);

                // update reservation after email was sent
                item.ReminderSent = 1;
                _eventRepository.Update(item.Id, item);
            }
            catch
            {
                return;
            }
        }

        public void SendDeleteEmail(ReservationDTO item)
        {
            try
            {
                var reservation = _eventRepository.GetById(item.Id);
                string mailBodyhtml = "<div><p> Sveiki,</p> <br><p> Informuojame, kad jūsų rezervacija:  "
                    + item.Title + " yra atšaukta." +
                    " </p><br><p>Pagarbiai</p><p>JJG Muziejaus administracija</p></div>";
                var msg = new MailMessage(_config.Value.SenderEmail, "kaltenytmonika3@gmail.com", _config.Value.Subject, mailBodyhtml);
                msg.To.Add("kaltenytmonika3@gmail.com");
                msg.IsBodyHtml = true;
                var smtpClient = new System.Net.Mail.SmtpClient(_config.Value.SmtpClient, Int32.Parse(_config.Value.Port))
                {
                    UseDefaultCredentials = true,
                    Credentials = new NetworkCredential(_config.Value.SenderEmail, _config.Value.Password),
                    EnableSsl = true
                }; //**if your from email address is "from@hotmail.com" then host should be "smtp.hotmail.com"**
                smtpClient.Send(msg);

                // update reservation after email was sent
                item.IsDeleted = true;
                _eventRepository.Update(item.Id, reservation);
            }
            catch
            {
                return;
            }
        }

        public void SendUpdateEmail(ReservationDTO item)
        {
            var oldReservation = _eventRepository.GetById(item.Id);
            try
            {
                    string mailBodyhtml = "<div><p> Sveiki,</p> <br><p> Informuojame, kad pakeistas jūsų rezervacijos laikas. Muziejaus patalpos užregistruotos nuo: "
                        + item.Start.ToString().Substring(0, Math.Max(0, item.Start.ToString().Length - 3)) + " iki: " + item.End.ToString().Substring(0, Math.Max(0, item.End.ToString().Length - 3)) +
                        ".</p><br><p>Pagarbiai</p><p>JJG Muziejaus administracija</p></div>";
                    var msg = new MailMessage(_config.Value.SenderEmail, "kaltenytmonika3@gmail.com", _config.Value.Subject, mailBodyhtml);
                    msg.To.Add("kaltenytmonika3@gmail.com");
                    msg.IsBodyHtml = true;
                var smtpClient = new SmtpClient(_config.Value.SmtpClient, Int32.Parse(_config.Value.Port))
                {
                    UseDefaultCredentials = true,
                    Credentials = new NetworkCredential(_config.Value.SenderEmail, _config.Value.Password),
                    EnableSsl = true
                }; //**if your from email address is "from@hotmail.com" then host should be "smtp.hotmail.com"**
                smtpClient.Send(msg);

                    if (oldReservation.ReminderSent == 1)
                    {
                        oldReservation.ReminderSent = 0;
                    }
                    oldReservation.Start = item.Start;
                    oldReservation.End = item.End;
                    _eventRepository.Update(oldReservation.Id, oldReservation);

            }
            catch (Exception)
            {
                return;
            }
        }
    }
}

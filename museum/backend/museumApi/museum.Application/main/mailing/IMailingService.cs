using museum.Application.main.events.dto;

namespace museum.Application.main.mailing
{
    public interface IMailingService
    {
        void ManageQueue();
        void SendDeleteEmail(ReservationDTO item);
        void SendUpdateEmail(ReservationDTO item);
        void FireTheJob(int inverval);
    }
}

using AutoMapper;
using museum.Application.main.chronicles.dto;
using museum.Application.main.classes.dto;
using museum.Application.main.events.dto;
using museum.Application.main.graduates.dto;
using museum.Application.main.students.dto;
using museum.Application.main.teachers.dto;
using museum.Application.main.users.dto;
using museumApi.EF.entities;

namespace museum.Application.automapper
{
    public class MappingsProfile : Profile
    {
        public MappingsProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<Chronicle, ChronicleDTO>().ReverseMap();

            CreateMap<Graduates, GraduatesDTO>().ReverseMap();

            CreateMap<ClassInfo, ClassDTO>().ReverseMap();

            CreateMap<Student, StudentDTO>()
                .ReverseMap();

            CreateMap<Teacher, TeacherDTO>().ReverseMap();

            CreateMap<Reservation, ReservationDTO>().ReverseMap();

            CreateMap<ClassDTO, StudentDTO>().ForMember(dto => dto.FkClassNavigationFkTeacher,
                opt => opt.MapFrom(x => x.FkTeacher));
        }
    }
}

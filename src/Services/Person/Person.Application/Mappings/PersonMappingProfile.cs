using AutoMapper;
using Person.Application.Contracts.Requests;
using Person.Application.Contracts.Responses;
using Person.Domain.ValueObjects;

namespace Person.Application.Mappings;

public class PersonMappingProfile : Profile
{
    public PersonMappingProfile()
    {
        CreateMap<CreatePersonRequest, Domain.Entities.Person>()
            .ConstructUsing(src => Domain.Entities.Person.Create(
                new FullName(src.Surname, src.FirstName, src.Patronymic),
                new Email(src.Email),
                new Phone(src.Phone),
                src.DateBirth,
                src.Gender,
                src.Comment
            ));

        CreateMap<Domain.Entities.Person, PersonResponse>()
            .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.FullName.Surname))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FullName.FirstName))
            .ForMember(dest => dest.Patronymic, opt => opt.MapFrom(src => src.FullName.Patronymic))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName.ToString()))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone.Value));

        CreateMap<Domain.Entities.WorkExperience, WorkExperienceResponse>();

        CreateMap<Address, AddressResponse>()
            .ForMember(dest => dest.FullAddress, opt => opt.MapFrom(src => src.ToString()));
    }
}
using AutoMapper;
using SourceName.Application.Queries;
using SourceName.Domain;

namespace SourceName.Application.Common.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ExampleDomainEntity, GetAllExample.ExampleListItem>();
    }
}
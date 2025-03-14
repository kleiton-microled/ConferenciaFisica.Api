using AutoMapper;
using ConferenciaFisica.Application.ViewModels;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Entities.DescargaExportacao;

namespace ConferenciaFisica.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DescargaExportacao, DescargaExportacaoViewModel>()
                .ForMember(dest => dest.Registro, opt => opt.MapFrom(src => src.Id));

            CreateMap<Talie, TalieViewModel>();
            CreateMap<TalieItem, TalieItemViewModel>();
            CreateMap<TalieItemViewModel, TalieItem>();

            CreateMap<TalieViewModel, TalieDTO>();
            CreateMap<TalieDTO, TalieViewModel>();

            CreateMap<TalieItemViewModel, TalieItemDTO>();
            CreateMap<TalieItemDTO, TalieItemViewModel>();

            CreateMap<DescargaExportacaoViewModel, DescargaExportacao>();

            CreateMap<ArmazensViewModel, Armazens>();
            CreateMap<Armazens, ArmazensViewModel>();

        }
    }
}

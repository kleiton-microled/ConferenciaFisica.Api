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

            CreateMap<TalieViewModel, TalieDTO>()
                .ForMember(dest => dest.Conferente, opt => opt.Ignore()); // Ignora o campo Conferente no mapeamento
            CreateMap<TalieDTO, TalieViewModel>();

            CreateMap<TalieItemViewModel, TalieItemDTO>()
                .ForMember(dest => dest.Remonte, opt => opt.MapFrom(src => src.Remonte ? "S" : "N")) // Converte bool para string
                .ForMember(dest => dest.Embalagem, opt => opt.MapFrom(src => src.EmbalagemSigla ?? "")); // Mapeia EmbalagemSigla para Embalagem
            CreateMap<TalieItemDTO, TalieItemViewModel>()
                .ForMember(dest => dest.Remonte, opt => opt.MapFrom(src => src.Remonte == "S")); // Converte string para bool

            CreateMap<DescargaExportacaoViewModel, DescargaExportacao>();

            CreateMap<ArmazensViewModel, Armazens>();
            CreateMap<Armazens, ArmazensViewModel>();

            CreateMap<MarcantesViewModel, Marcante>();
            CreateMap<Marcante, MarcantesViewModel>();

            CreateMap<TipoProcessoModel, TipoProcessoViewModel>();
            CreateMap<TipoProcessoViewModel, TipoProcessoModel>();

            CreateMap<TipoProcessoFotoViewModel, TipoProcessoFotoModel>();
            CreateMap<TipoProcessoFotoModel, TipoProcessoFotoViewModel>();

            CreateMap<LocaisYardViewModel, Yard>();
            CreateMap<Yard, LocaisYardViewModel>();
        }
    }
}

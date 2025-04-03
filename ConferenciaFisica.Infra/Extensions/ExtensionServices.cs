using ConferenciaFisica.Application.Interfaces;
using ConferenciaFisica.Application.Services;
using ConferenciaFisica.Application.UseCases.Agendamento.Interfaces;
using ConferenciaFisica.Application.UseCases.Agendamento;
using ConferenciaFisica.Application.UseCases.Avarias.Interface;
using ConferenciaFisica.Application.UseCases.Avarias;
using ConferenciaFisica.Application.UseCases.Conferencia.Interfaces;
using ConferenciaFisica.Application.UseCases.Conferencia;
using ConferenciaFisica.Application.UseCases.DescargaExportacao.Interfaces;
using ConferenciaFisica.Application.UseCases.DescargaExportacao;
using ConferenciaFisica.Application.UseCases.Documentos.Interfaces;
using ConferenciaFisica.Application.UseCases.Documentos;
using ConferenciaFisica.Application.UseCases.Embalagens.Interfaces;
using ConferenciaFisica.Application.UseCases.Embalagens;
using ConferenciaFisica.Application.UseCases.Imagens.Interfaces;
using ConferenciaFisica.Application.UseCases.Imagens;
using ConferenciaFisica.Application.UseCases.Lacres.Interfaces;
using ConferenciaFisica.Application.UseCases.Lacres;
using ConferenciaFisica.Application.UseCases.Utils.Interfaces;
using ConferenciaFisica.Application.UseCases.Utils;
using ConferenciaFisica.Domain.Repositories.DescargaExportacaoReporitory;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Infra.Repositories.DescargaExportacaoRepository;
using ConferenciaFisica.Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ConferenciaFisica.Application.UseCases.Marcantes;
using ConferenciaFisica.Application.UseCases.MovimentacaoCargaSolta;
using ConferenciaFisica.Application.UseCases.SaidaCaminhao.Interfaces;
using ConferenciaFisica.Application.UseCases.SaidaCaminhao;
using ConferenciaFisica.Application.UseCases.estufagemConteiner.Interfaces;
using ConferenciaFisica.Application.UseCases.estufagemConteiner;
using ConferenciaFisica.Domain.Repositories.EstufagemConteiner;
using ConferenciaFisica.Infra.Repositories.EstufagemConteinerRepository;
using ConferenciaFisica.Application.UseCases.Conferentes;

namespace ConferenciaFisica.Infra.Extensions
{
    public static class ExtensionServices
    {
        public static IServiceCollection AddExtensionServices(this IServiceCollection services)
        {
            services.AddSingleton<SqlServerConnectionFactory>();
            services.AddScoped<IPatioAccessService, PatioAccessService>();
            
            services.AddScoped<IBuscarConferenciaUseCase, BuscarConferenciaUseCase>();
            services.AddScoped<IConferenciaRepository, ConferenciaRepository>();
            
            services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();
            services.AddScoped<ICarregarLotesAgendamentoUseCase, CarregarLotesAgendamentoUseCase>();
            services.AddScoped<ICarregarCntrAgendamentoUseCase, CarregarCntrAgendamentoUseCase>();
            services.AddScoped<IIniciarConferenciaUseCase, IniciarConferenciaUseCase>();
            services.AddScoped<IAtualizarConferenciaUseCase, AtualizarConferenciaUseCase>();
            services.AddScoped<ICadastrosAdicionaisUseCase, CadastrosAdicionaisUseCase>();
            services.AddScoped<ITiposLacresUseCase, TiposLacresUseCase>();
            services.AddScoped<ILacresUseCase, LacresUseCase>();
            services.AddScoped<IDocumentoConferenciaUseCase, DocumentoConferenciaUseCase>();
            services.AddScoped<ITiposAvariasUseCase, TiposAvariasUseCase>();
            services.AddScoped<IAvariasConferenciaUseCase, AvariasConfereciaUseCase>();
            services.AddScoped<IAvariasRepository, AvariasRepository>();
            
            services.AddScoped<ITiposDocumentosUseCase, TiposDocumentosUseCase>();
            services.AddScoped<IEmbalagensUseCase, EmbalagensUseCase>();
            services.AddScoped<ITiposDocumentosRepository, TiposDocumentosRepository>();
            services.AddScoped<IEmbalagensRepository, EmbalagensRepository>();
            
            
            services.AddScoped<IDescargaExportacaoUseCase, DescargaExportacaoUseCase>();
            services.AddScoped<IDescargaExportacaoRepository, DescargaExportacaoRepository>();
            services.AddScoped<ITipoFotoUseCase, ProcessoUseCase>();
            services.AddScoped<IImagemRepository, ImagemRepository>();
            services.AddScoped<ITiposProcessoRepository, TipoProcessoRepository>();
            services.AddScoped<ITiposProcessosUseCase, TiposProcessosUseCase>();
            services.AddScoped<ITipoProcessoFotoUtilUseCase, TipoProcessoFotoUtilUseCase>();
            services.AddScoped<ITiposProcessoFotoRepository, TiposProcessoFotoRepository>();

            services.AddScoped<IMarcantesUseCase, MarcantesUseCase>();
            services.AddScoped<IMarcantesRepository, MarcanteRepository>();

            services.AddScoped<IMovimentacaoCargaSoltaUseCase, MovimentacaoCargaSoltaUseCase>();
            services.AddScoped<IMovimentacaoCargaSoltaRepository, MovimentacaoCargaSoltaRepository>();
            services.AddScoped<ISaidaDeCaminhaoUseCase, SaidaDeCaminhaoUseCase>();
            services.AddScoped<ISaidaDoCaminhaoRepository, SaidaCaminhaoRepository>();

            services.AddScoped<IPlanejamentoUseCase, PlanejamentoUseCase>();
            services.AddScoped<IEstufagemConteinerRepository, EstufagemConteinerRepository>();

            services.AddScoped<IItensEstufadosUseCase, ItensEstufadosUseCase>();
            services.AddScoped<IEtiquetasUseCase, EtiquetasUseCase>();

            services.AddScoped<IConferentesUseCase, ConferenteUseCase>();
            services.AddScoped<IColetorRepository, ColetorRepository>();



            return services;
        }
    }
}

using System;
using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.UseCases.CarregamentoCargaSolta.Interface;
using ConferenciaFisica.Application.ViewModels;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Domain.Repositories;

namespace ConferenciaFisica.Application.UseCases.CarregamentoCargaSolta
{
    public class CarregamentoCargaSoltaUseCase : ICarregamentoCargaSoltaUseCase
    {
        private readonly ICarregamentoCargaSoltaRepository _carregamentoCargaSoltaRepository;

        public CarregamentoCargaSoltaUseCase(ICarregamentoCargaSoltaRepository carregamentoCargaSoltaRepository)
        {
            _carregamentoCargaSoltaRepository = carregamentoCargaSoltaRepository;
        }

        public async Task<ServiceResult<object>> BuscarMacantes(int marcante, int? patio)
        {
            try
            {
                var marcanteByQuery = await _carregamentoCargaSoltaRepository.GetMarcanteByIdEPatio(marcante, patio);

                return ServiceResult<object>.Success(marcanteByQuery);
            }
            catch (Exception exception)
            {
                return ServiceResult<object>.Failure(exception.Message);
            }
        }


        public async Task<ServiceResult<object>> SalvarMacantes(int marcante, int? patio, string local, string placa)
        {
            try
            {
                var marcanteByQuery = await _carregamentoCargaSoltaRepository.GetMarcanteByIdEPatio(marcante, patio);

                if (marcanteByQuery is null) return ServiceResult<object>.Failure("Marcante nao encontrado");

                var itensCarregados = await _carregamentoCargaSoltaRepository.GetAutonumCs(marcanteByQuery.AUTONUMCS, placa);

                if (itensCarregados == 0 || itensCarregados is null) ServiceResult<object>.Failure("Nao pertence a esse carregamento");

                if (local == "CAM") return ServiceResult<object>.Failure("Marcante já carregado");

                var quantidadeCarregamento = await _carregamentoCargaSoltaRepository.GetQuantidadeCarregamento(marcanteByQuery.AUTONUMCS, placa);

                if (quantidadeCarregamento is null) return ServiceResult<object>.Failure("Quantidade carregamento nao encontrado");
                if (quantidadeCarregamento.QtdCarregada > quantidadeCarregamento.QtdPrevista) return ServiceResult<object>.Failure("Quantidade superior ao registrado");

                await _carregamentoCargaSoltaRepository.UpdateMarcanteAndCargaSolta(marcante, patio, local, placa, marcanteByQuery, itensCarregados);

                return ServiceResult<object>.Success("Executado com sucesso");
            }
            catch (Exception exception)
            {
                return ServiceResult<object>.Failure(exception.Message);
            }
        }

        public async Task<ServiceResult<CarregamentoOrdem>> GetOrdens(int? patio, string? veiculo, string? local, int? quantidade, DateTime? inicio, string tipo = "I")
        {
            try
            {
                var ordens = await _carregamentoCargaSoltaRepository.GetOrdens(veiculo);
                var itensCarregados = await _carregamentoCargaSoltaRepository.GetItensCarregados(veiculo, tipo);


                return ServiceResult<CarregamentoOrdem>.Success(new CarregamentoOrdem()
                {
                    ItensCarregados = itensCarregados.ToArray(),
                    Ordens = ordens.ToArray()
                });
            }
            catch (Exception exception)
            {
                return ServiceResult<CarregamentoOrdem>.Failure(exception.Message);
            }
        }

        public async Task<ServiceResult<EnumValueDTO[]>> GetVeiculos(int patio)
        {
            var result = new List<EnumValueDTO>();
            try
            {
                var resultQuery = await _carregamentoCargaSoltaRepository.GetVeiculosByPatio(patio);
                if (resultQuery is null)
                {
                    return ServiceResult<EnumValueDTO[]>.Failure("Veiculos nao encontrados");
                }

                for (int i = 0; i < resultQuery.Count(); i++)
                {
                    var actual = resultQuery[i];
                    result.Add(new EnumValueDTO()
                    {
                        Id = i,
                        Codigo = i++,
                        Descricao = actual
                    });
                }

                return ServiceResult<EnumValueDTO[]>.Success(result.ToArray());
            }
            catch (Exception exception)
            {
                return ServiceResult<EnumValueDTO[]>.Failure(exception.Message);
            }
        }
    }
}

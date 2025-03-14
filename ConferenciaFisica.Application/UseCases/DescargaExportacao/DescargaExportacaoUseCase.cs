using AutoMapper;
using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.Inputs;
using ConferenciaFisica.Application.UseCases.DescargaExportacao.Interfaces;
using ConferenciaFisica.Application.ViewModels;
using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Domain.Entities.DescargaExportacao;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Domain.Repositories.DescargaExportacaoReporitory;
using Microsoft.Win32;
using System.ComponentModel;

namespace ConferenciaFisica.Application.UseCases.DescargaExportacao
{
    public class DescargaExportacaoUseCase : IDescargaExportacaoUseCase
    {
        private readonly IDescargaExportacaoRepository _repository;
        private readonly IMapper _mapper;
        public DescargaExportacaoUseCase(IDescargaExportacaoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResult<DescargaExportacaoViewModel>> BuscarPorRegistro(int registro)
        {
            DescargaExportacaoViewModel descarga = null;

            var _descarga = await _repository.BuscarRegistroAsync(registro);

            if (_descarga == null)
            {
                return ServiceResult<DescargaExportacaoViewModel>.Failure("Registro não encontrado.");
            }

            return ServiceResult<DescargaExportacaoViewModel>.Success(_mapper.Map<DescargaExportacaoViewModel>(_descarga), "Registro localizada com sucesso.");
        }

        public async Task<ServiceResult<bool>> CadastrarAvaria(AvariaInput input)
        {
            var _serviceResult = new ServiceResult<bool>();
            var avarias = new List<TiposAvariasCommand>();

            foreach (var item in input.TiposAvarias)
            {
                avarias.Add(TiposAvariasCommand.New(item.Id, item.Codigo, item.Descricao));
            }

            var command = CadastroAvariaCommand.New(input.TalieId ?? 0,
                                                    input.Local,
                                                    input.Divergencia,
                                                    input.QuantidadeAvariada,
                                                    input.PesoAvariado,
                                                    input.Observacao,
                                                    avarias
                                                   );


            var ret = await _repository.CadastrarAvaria(command);

            if (ret > 0)
            {
                _serviceResult.Result = true;
            }
            else
            {
                _serviceResult.Result = false;
            }



            return _serviceResult;
        }

        public async Task<ServiceResult<bool>> GravarOuAtualizarTalie(DescargaExportacaoViewModel request)
        {
            var _serviceResult = new ServiceResult<bool>();

            var talie = _mapper.Map<TalieDTO>(request.Talie);

            var command = DescargaExportacaoCommand.CreateNew(request.Registro, talie, request.Placa, request.Reserva, request.Cliente);

            var talieId = await _repository.AtualizarOuCriarTalie(command);
            if (talieId > 0)
            {
                var itensRelacionados = await _repository.BuscarTaliItens(talieId);

                if (talieId > 0 && itensRelacionados?.Count() == 0)
                {
                    await _repository.GerarDescargaAutomatica(command.Registro, talieId);
                }

                _serviceResult.Result = true;
            }
            else
            {
                _serviceResult.Result = false;
            }

            return _serviceResult;
        }

        public async Task<ServiceResult<TalieItemViewModel>> BuscarItemTalie(int id)
        {
            TalieItemViewModel item = null;

            var data = await _repository.BuscarTalieItem(id);

            if (data == null)
            {
                return ServiceResult<TalieItemViewModel>.Failure("Registro não encontrado.");
            }

            return ServiceResult<TalieItemViewModel>.Success(_mapper.Map<TalieItemViewModel>(data), "Registro localizada com sucesso.");
        }

        public async Task<ServiceResult<bool>> SalvarTalieItem(TalieItemViewModel request, int registro)
        {
            var _serviceResult = new ServiceResult<bool>();

            //var itemOriginal = _talieBusiness.BuscarTalieItem(itemAlterado.Id);
            var itemOriginal = await _repository.BuscarTalieItem(request.Id);

            if (itemOriginal == null)
            {
                _serviceResult.Mensagens.Add("Item não encontrado");
                return _serviceResult;
            }

            // Obtém a quantidade total permitida
            var quantidadeTotalPermitida = 20;// _talieBusiness.BuscarQuantidadeTotalDaNotaFiscal(itemAlterado.NotaFiscal);

            if (quantidadeTotalPermitida <= 0)
            {
                _serviceResult.Mensagens.Add("Não foi possível obter a quantidade total permitida para a nota fiscal.");
                return _serviceResult;
            }

            // Busca todos os itens relacionados à mesma NF
            var itensRelacionados = await _repository.BuscarTaliItens(itemOriginal.TalieId); //_talieBusiness.BuscarItensDoTalie(id).Result;
            if (itensRelacionados == null || !itensRelacionados.Any())
            {
                _serviceResult.Mensagens.Add("Nenhum item relacionado encontrado para a NF.");
                return _serviceResult;
            }

            // Soma as quantidades dos itens relacionados, ignorando o próprio item
            var quantidadeTotalUsada = itensRelacionados
                .Where(i => i.Id != request.Id) // Ignorar o próprio item
                .Sum(i => i.QtdDescarga);


            // Calcula a quantidade disponível
            var quantidadeDisponivel = quantidadeTotalPermitida - quantidadeTotalUsada;

            // Valida se a quantidade informada pode ser usada
            if (request.QuantidadeDescarga > quantidadeDisponivel)
            {
                _serviceResult.Mensagens.Add($"A quantidade deve ser menor ou igual ao total disponível: {quantidadeDisponivel}.");
                return _serviceResult;
            }

            if (request.QuantidadeDescarga < itemOriginal.QtdDescarga)
            {
                // Calcula a quantidade restante
                var quantidadeRestante = itemOriginal.QtdDescarga - request.QuantidadeDescarga;

                // Atualiza o item original com a nova quantidade
                itemOriginal.QtdDescarga = quantidadeRestante;

                var updateOriginal = await _repository.UpdateTalieItem(itemOriginal);

                if (!updateOriginal)
                {
                    _serviceResult.Mensagens.Add("Erro ao atualizar o item original.");
                    return _serviceResult;
                }

                // Cria um novo item com a quantidade restante
                var novoItem = new TalieItem
                {
                    Id = request.Id,
                    TalieId = itemOriginal.TalieId,
                    Quantidade = request.QuantidadeDescarga,
                    QtdDescarga = request.QuantidadeDescarga,
                    NotaFiscal = request.NotaFiscal,
                    CodigoEmbalagem = request.CodigoEmbalagem,
                    Peso = request.Peso,
                    Comprimento = request.Comprimento,
                    Largura = request.Largura,
                    IMO = request.IMO,
                    IMO2 = request.IMO2,
                    IMO3 = request.IMO3,
                    IMO4 = request.IMO4,
                    UNO = request.UNO,
                    UNO2 = request.UNO2,
                    UNO3 = request.UNO3,
                    UNO4 = request.UNO4,
                    Remonte = request.Remonte,
                    Fumigacao = request.Fumigacao
                };

                var inserirNovo = await _repository.CadastrarTalieItem(novoItem, registro);

                if (!inserirNovo)
                {
                    _serviceResult.Mensagens.Add("Erro ao criar o novo item.");
                    return _serviceResult;
                }
                else
                {
                    _serviceResult.Result = inserirNovo;
                    _serviceResult.Mensagens.Add("Item atualizado e novo item criado com sucesso!");
                    return _serviceResult;
                }

            }
            else
            {
                // Atualiza diretamente o item quando não há divisão de quantidade
                var updateResult = await _repository.UpdateTalieItem(_mapper.Map<TalieItem>(request));

                if (!updateResult)
                {
                    _serviceResult.Mensagens.Add("Erro ao atualizar o item.");
                }
                else
                {
                    _serviceResult.Result = updateResult;
                    _serviceResult.Mensagens.Add("Alterações salvas com sucesso!");
                }

            }

            return _serviceResult;
        }

        public async Task<ServiceResult<bool>> ExcluirTalieItem(int registroId, int talieId)
        {
            var serviceResult = new ServiceResult<bool>();
            var registro = await _repository.BuscarRegistroAsync(registroId);

            if (registro is null)
            {
                serviceResult.Mensagens.Add("Registro nao encontrado");

                return serviceResult;
            }

            var talieBase = registro.Talie?.TalieItem?.FirstOrDefault();
            if (talieBase is null)
            {
                serviceResult.Mensagens.Add("Talie do registro nao encontrado");

                return serviceResult;
            }

            if (registro.Talie?.TalieItem.Count <= 1 || talieBase.Id == talieId)
            {
                serviceResult.Mensagens.Add("Nao foi possivel exluir primeiro talie!");

                return serviceResult;
            }

            var talieToRemove = registro.Talie.TalieItem.FirstOrDefault(x => x.Id == talieId);
            if (talieToRemove is null)
            {
                serviceResult.Mensagens.Add("Talie nao encontrado!");
                return serviceResult;
            }

            talieBase.QuantidadeDescarga = talieBase.QuantidadeDescarga + talieToRemove.QuantidadeDescarga;

            await _repository.UpdateTalieItem(talieBase);

            var result = await _repository.ExcluirTalieItem(talieId);
            if (result)
            {
                serviceResult.Result = result;
                serviceResult.Mensagens.Add("Item excluido com sucesso!");
            }
            else
            {
                serviceResult.Mensagens.Add("Falha ao tentar excluir o registro!");
            }

            return serviceResult;
        }

        public async Task<ServiceResult<bool>> GravarObservacao(string observacao, int talieId)
        {
            var _serviceResult = new ServiceResult<bool>();

            var result = await _repository.GravarObservacao(observacao, talieId);
            if (result)
            {
                _serviceResult.Result = result;
                _serviceResult.Mensagens.Add("Observação atualizada com sucesso!");
            }
            else
            {
                _serviceResult.Mensagens.Add("Falha ao tentar salvar o registro!");
            }

            return _serviceResult;
        }

        public async Task<ServiceResult<IEnumerable<ArmazensViewModel>>> CarregarArmazens(int patio)
        {
            IEnumerable<ArmazensViewModel> armazens = null;

            var data = await _repository.CarregarArmazens(patio);

            if (data == null)
            {
                return ServiceResult<IEnumerable<ArmazensViewModel>>.Failure("Registros não encontrado.");
            }

            return ServiceResult<IEnumerable<ArmazensViewModel>>.Success(_mapper.Map<IEnumerable<ArmazensViewModel>>(data), "Armazens localizados com sucesso.");
        }

        public Task<ServiceResult<bool>> GravarMarcante(MarcanteInput input)
        {
            throw new NotImplementedException();
        }
    }
}

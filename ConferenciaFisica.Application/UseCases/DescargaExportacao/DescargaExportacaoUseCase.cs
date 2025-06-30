using AutoMapper;
using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.Inputs;
using ConferenciaFisica.Application.UseCases.DescargaExportacao.Interfaces;
using ConferenciaFisica.Application.ViewModels;
using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Entities.DescargaExportacao;
using ConferenciaFisica.Domain.Repositories.DescargaExportacaoReporitory;

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

            var idConferente = await _repository.BuscarIdConferente(request.NomeConferente);

            var command = DescargaExportacaoCommand.CreateNew(request.Registro,
                                                             talie,
                                                             request.Placa,
                                                             request.Reserva,
                                                             request.Cliente,
                                                             request.IdReserva,
                                                             idConferente,
                                                             request.Equipe,
                                                             request.Operacao,
                                                             request.IsCrossDocking,
                                                             request.Conteiner);

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
            var quantidadeTotalPermitida = request.QuantidadeNf; //_talieBusiness.BuscarQuantidadeTotalDaNotaFiscal(itemAlterado.NotaFiscal);

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


            var quantidadeByNF = itensRelacionados
                                        .Where(i => i.NotaFiscal == request.NotaFiscal);

            var quantidadeTotalUsadaV2 = quantidadeByNF
                                        .Where(i => i.Id != request.Id) // Ignorar o próprio item
                                        .Sum(i => i.QtdDescarga);

            // Calcula a quantidade disponível
            var quantidadeDisponivel = quantidadeTotalPermitida - quantidadeTotalUsadaV2;

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
                    Quantidade = request.Quantidade ?? 0,
                    QtdDescarga = request.QuantidadeDescarga,
                    QuantidadeDescarga = request.QuantidadeDescarga,
                    NotaFiscal = request.NotaFiscal,
                    CodigoEmbalagem = request.CodigoEmbalagem,
                    Peso = request.Peso,
                    Comprimento = request.Comprimento,
                    Largura = request.Largura,
                    Altura = request.Altura,
                    IMO = request.IMO,
                    IMO2 = request.IMO2,
                    IMO3 = request.IMO3,
                    IMO4 = request.IMO4,
                    UNO = request.UNO,
                    UNO2 = request.UNO2,
                    UNO3 = request.UNO3,
                    UNO4 = request.UNO4,
                    Remonte = request.Remonte,
                    Fumigacao = request.Fumigacao,
                    Madeira = request.Madeira,
                    Fragil = request.Fragil,
                    NotaFiscalId = itemOriginal.NotaFiscalId
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
            else if (request.QuantidadeDescarga > itemOriginal.QtdDescarga || request.QuantidadeDescarga > itemOriginal.Quantidade)
            {
                _serviceResult.Mensagens.Add("Quantidade nao deve ser maior que tamanho total NF.");
            }
            else
            {
                // Atualiza diretamente o item quando não há divisão de quantidade
                var model = _mapper.Map<TalieItem>(request);
                model.QtdDescarga = request.QuantidadeDescarga;
                model.QuantidadeDescarga = request.QuantidadeDescarga;
                var updateResult = await _repository.UpdateTalieItem(model);

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

        public async Task<ServiceResult<bool>> ExcluirTalieItem(int registroId, int talieId, string notaFiscal)
        {
            var serviceResult = new ServiceResult<bool>();
            var registro = await _repository.BuscarRegistroAsync(registroId);

            if (registro is null)
            {
                serviceResult.Mensagens.Add("Registro nao encontrado");

                return serviceResult;
            }

            //var talieBase = registro.Talie?.TalieItem?.FirstOrDefault();
            var talieBase = registro.Talie.TalieItem.Where(x => x.NotaFiscal == notaFiscal).FirstOrDefault();
            if (talieBase is null)
            {
                serviceResult.Mensagens.Add("Talie do registro nao encontrado");

                return serviceResult;
            }

            if (registro.Talie?.TalieItem.Where(x => x.NotaFiscal == notaFiscal).ToList().Count <= 1 || talieBase.Id == talieId)
            {
                serviceResult.Mensagens.Add("Não é possível excluir o o primeiro item de uma nota fiscal!");

                return serviceResult;
            }

            var talieToRemove = registro.Talie.TalieItem.Where(x => x.NotaFiscal == notaFiscal).FirstOrDefault(x => x.Id == talieId);
            if (talieToRemove is null)
            {
                serviceResult.Mensagens.Add("Talie nao encontrado!");
                return serviceResult;
            }

            talieBase.QuantidadeDescarga = talieBase.QuantidadeDescarga + talieToRemove.QuantidadeDescarga;
            talieBase.QtdDescarga = talieBase.QuantidadeDescarga;

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

        public async Task<ServiceResult<bool>> GravarMarcante(MarcanteInput input)
        {
            var _serviceResult = new ServiceResult<bool>();

            var command = new MarcanteCommand()
            {
                Registro = input.Registro,
                TalieId = input.TalieId,
                TalieItemId = input.TalieItemId,
                Marcante = input.Marcante,
                Quantidade = input.Quantidade,
                Armazem = input.Armazem,
                Local = input.Local
            };
            var result = await _repository.GravarMarcante(command);
            if (result)
            {
                _serviceResult.Result = result;
                _serviceResult.Mensagens.Add("Marcante associado com sucesso!");
            }
            else
            {
                _serviceResult.Mensagens.Add("Marcante não pertence ao registro atual!");
            }

            return _serviceResult;
        }

        public async Task<ServiceResult<IEnumerable<MarcantesViewModel>>> CarregarMarcantes(int talieItem)
        {
            var data = await _repository.CarregarMarcantesTalieItem(talieItem);

            if (data == null)
            {
                return ServiceResult<IEnumerable<MarcantesViewModel>>.Failure("Registros não encontrado.");
            }

            return ServiceResult<IEnumerable<MarcantesViewModel>>.Success(_mapper.Map<IEnumerable<MarcantesViewModel>>(data), "Marcantes localizados com sucesso.");
        }

        public async Task<ServiceResult<bool>> ExcluirMarcanteTalieItem(int talieId)
        {
            var _serviceResult = new ServiceResult<bool>();

            var result = await _repository.ExcluirMarcanteTalieItem(talieId);
            if (result)
            {
                _serviceResult.Result = result;
                _serviceResult.Mensagens.Add("Marcante excluido com sucesso!");
            }
            else
            {
                _serviceResult.Mensagens.Add("Falha ao tentar exlcuir o marcante!");
            }

            return _serviceResult;
        }

        public async Task<ServiceResult<bool>> FinalizarProcesso(int talie, bool crossdock, string usuario, string conteiner = "")
        {
            var _serviceResult = new ServiceResult<bool>();
            //Validar quantidade registrada X quantidade descarregada

            if (!await ValidarQuantidadeDescarga(talie))
            {
                _serviceResult.Result = false;
                _serviceResult.Mensagens.Add("Divergência entre quantidade registrada e descarregada.");
            }


            //if (!await VerificarEmissaoEtiquetasAsync(talie))
            //{
            //    _serviceResult.Result = false;
            //    _serviceResult.Mensagens.Add("Consta pendência de emissão de etiquetas deste registro. Deseja continuar assim mesmo?");
            //}

            var result = await _repository.FinalizarProcesso(talie);

            if (result)
            {
                var isValid = await _repository.ValidarCargaTransferidaAsync(talie);
                if (isValid)
                {
                    var fecharTalie = await _repository.FecharTalieAsync(talie);
                    if (fecharTalie)
                    {
                        await _repository.FinalizarReservaAsync(talie);

                        _serviceResult.Result = result;
                        _serviceResult.Mensagens.Add("Processo finalizado com sucesso!");
                    }
                }
                else
                {
                    _serviceResult.Mensagens.Add("Falha ao tentar finalizar o processo!");
                }
            }

            //Para casos de crossdocking gera estufagem automatica
            if (crossdock)
            {
                var processoCrossDocking = await _repository.FinalizarProcessoCrossDocking(talie, conteiner);

                //var dtRs = await _repository.BuscarTalieCrossDock(talie);
                //if (dtRs != null && dtRs.Any()) await _repository.CrossDockUpdatePatioF(conteiner);

                //var reservaContainer = await _repository.CrossDockGetNumeroReservaContainer(patioContainer) ?? 0;

                //int romaneioId = await _repository.GetCrossDockRomaneioId(patioContainer) ?? 0;
                //if (romaneioId == 0)
                //{
                //    //romaneioId = await _repository.GetCrossDockSequencialId();

                //    romaneioId = await _repository.InserirRomaneio(romaneioId, usuario, patioContainer, reservaContainer);

                //    foreach (var item in dtRs)
                //    {
                //        await _repository.InserirRomaneioCs(romaneioId, item.AutonumPcs, item.QtdeEntrada);
                //    }

                //    //DateTime dataInicioEstufagem = await _repository.CrossDockGetDataInicoEstufagem(patioContainer);
                //    //DateTime dataFimEstufagem = await _repository.CrossDockGetDataFimEstufagem(patioContainer);
                //    // TODO: Entender como pegar essa data
                //    DateTime dataInicioEstufagem = DateTime.Now;
                //    DateTime dataFimEstufagem = DateTime.Now;

                //    var talieByContainer = await _repository.CrossDockBuscarTaliePorContainer(patioContainer);
                //    if (talieByContainer == null || talieByContainer == 0)
                //    {
                //        // retornar o item criado
                //        await _repository.CrossDockCriarTalie(patioContainer, dataInicioEstufagem, dataFimEstufagem, reservaContainer, romaneioId, "");
                //        talieByContainer = await _repository.CrossDockGetLastTalie();

                //        if (talieByContainer != null) await _repository.UpdateRomaneio(talieByContainer.Value, romaneioId);
                //    }
                //    else
                //    {
                //        await _repository.CrossDockUpdateTalieItem(dataInicioEstufagem, dataFimEstufagem, patioContainer);
                //    }

                //    foreach (var item in dtRs)
                //    {
                //        await _repository.InserirSaidaNF(patioContainer, item.AutonumNf, item.QtdeEstufagem);
                //        await _repository.CrossDockAtualizarQuantidadeEstufadaNF(item.AutonumNf, item.QtdeEstufagem);

                //        await _repository.CrossDockInserirSaidaCarga(item.AutonumPcs, item.QtdeEntrada, item.AutonumEmb, item.Bruto, item.Altura, item.Comprimento, item.Largura, item.VolumeDeclarado, patioContainer, DateTime.Now.ToString(), item.AutonumNf, talieByContainer, romaneioId);

                //        int quantidadeSaida = await _repository.GetQuantidadeSaidaCarga(item.AutonumPcs);

                //        if (quantidadeSaida >= item.QtdeEntrada)
                //        {
                //            await _repository.UpdatepatioCsFlag(item.AutonumPcs);
                //        }
                //    }
                //}
            }

            return _serviceResult;


        }


        private async Task<bool> ValidarQuantidadeDescarga(int id)
        {
            return await _repository.ValidarQuantidadeDescargaAsync(id);
        }

        private async Task<bool> VerificarEmissaoEtiquetasAsync(int id)
        {
            return await _repository.VerificarEmissaoEtiquetasAsync(id);
        }

        public async Task<IEnumerable<LocaisYardViewModel>> BuscarYard(string search)
        {
            return _mapper.Map<IEnumerable<Yard>, IEnumerable<LocaisYardViewModel>>(await _repository.BuscarYard(search));
        }

        public Task<ServiceResult<bool>> FinalizarProcesso(int id, bool crossdock)
        {
            throw new NotImplementedException();
        }
    }
}

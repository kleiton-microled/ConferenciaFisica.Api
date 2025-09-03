# Schema Dinâmico - ConferenciaFisica.Api

## Visão Geral

Esta funcionalidade permite que a API use diferentes schemas de banco de dados (REDEX ou SGIPA) baseado no parâmetro enviado pelo frontend, sem necessidade de alterar o código ou configurações.

## Como Funciona

### 1. Frontend envia o ambiente via Header HTTP

O frontend deve incluir o header `X-Environment` em todas as requisições para a API:

```javascript
// Exemplo com fetch
fetch('/api/conferencia/buscar', {
  method: 'GET',
  headers: {
    'Content-Type': 'application/json',
    'X-Environment': 'REDEX' // ou 'SGIPA'
  }
});

// Exemplo com axios
axios.get('/api/conferencia/buscar', {
  headers: {
    'X-Environment': 'REDEX' // ou 'SGIPA'
  }
});
```

### 2. API intercepta e configura o schema

O middleware `EnvironmentMiddleware` intercepta automaticamente o header e configura o schema correspondente para toda a requisição.

### 3. Consultas SQL usam o schema correto

Todas as consultas SQL que usam as tabelas afetadas automaticamente usam o schema correto baseado no ambiente.

## Tabelas Afetadas

As seguintes tabelas são automaticamente configuradas para usar o schema correto:

- `TB_EFETIVACAO_CONF_FISICA`
- `TB_TIPO_LACRE`
- `TB_LACRES_CONFERENCIA`
- `TB_DOCUMENTOS_CONFERENCIA`
- `TB_TIPO_DOCUMENTO_CONFERENCIA`
- `TB_TIPOS_AVARIAS`
- `TB_AVARIAS_CONFERENCIA`
- `TB_AVARIA_CONFERENCIA_TIPO_AVARIA`
- `TB_EFETIVACAO_CONF_FISICA_ADC`
- `VW_CONF_FISICA_SELECAO_CNTR`

## Valores Aceitos

- `REDEX` - Usa o schema REDEX.dbo
- `SGIPA` - Usa o schema SGIPA.dbo
- Qualquer outro valor ou ausência do header - Usa o schema padrão (SGIPA)

## Exemplos de Uso

### Buscar Conferência por Container

```http
GET /api/conferencia/buscar-por-conteiner/12345
X-Environment: REDEX
```

### Iniciar Conferência

```http
POST /api/conferencia/iniciar
X-Environment: SGIPA
Content-Type: application/json

{
  "idConteiner": "12345",
  "nomeConferente": "João Silva"
}
```

### Carregar Tipos de Lacres

```http
GET /api/lacres/tipos
X-Environment: REDEX
```

## Implementação Técnica

### Serviços Criados

1. **ISchemaService** - Interface para gerenciar schemas
2. **SchemaService** - Implementação do serviço
3. **EnvironmentMiddleware** - Middleware para interceptar headers
4. **SqlSchemaHelper** - Utilitário para substituir schemas nas consultas

### Modificações nos Repositórios

Todos os repositórios que usam as tabelas afetadas foram modificados para:

1. Injetar o `ISchemaService`
2. Usar `SqlSchemaHelper.ReplaceSchema()` para consultas SQL
3. Usar `_schemaService.GetTableName()` para consultas hardcoded

### Registro de Serviços

O `SchemaService` é registrado automaticamente no container de DI:

```csharp
services.AddScoped<ISchemaService, SchemaService>();
```

### Middleware

O `EnvironmentMiddleware` é registrado no pipeline da aplicação:

```csharp
app.UseMiddleware<EnvironmentMiddleware>();
```

## Vantagens

1. **Flexibilidade** - Permite alternar entre ambientes sem alterar código
2. **Manutenibilidade** - Centraliza a lógica de schema em um serviço
3. **Escalabilidade** - Fácil adicionar novos ambientes no futuro
4. **Consistência** - Todas as consultas usam o mesmo schema para uma requisição
5. **Transparência** - Frontend controla qual ambiente usar

## Considerações

1. **Performance** - Substituição de schemas é feita em tempo de execução
2. **Segurança** - Validação de valores aceitos para o ambiente
3. **Fallback** - Schema padrão é usado quando não especificado
4. **Transações** - Schema é mantido durante toda a transação

## Troubleshooting

### Schema não está sendo aplicado

1. Verifique se o header `X-Environment` está sendo enviado
2. Confirme se o valor é `REDEX` ou `SGIPA`
3. Verifique se o middleware está registrado no `Program.cs`

### Erro de tabela não encontrada

1. Confirme se a tabela existe no schema especificado
2. Verifique se o nome da tabela está na lista de tabelas afetadas
3. Confirme se o `SqlSchemaHelper` está sendo usado corretamente

### Schema padrão sendo usado

1. Verifique se o header está sendo enviado corretamente
2. Confirme se o valor do header é válido
3. Verifique se o middleware está funcionando

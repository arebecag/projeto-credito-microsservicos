# projeto-credito-microsservicos

Projeto de Microsserviços de Crédito e Emissão de Cartões
Este projeto implementa um conjunto de microsserviços para cadastro de clientes, análise de crédito e emissão de cartões de crédito, utilizando .NET 8 e RabbitMQ para comunicação por mensageria.

# Estrutura do Projeto
CadastroClientes: Serviço responsável pelo cadastro de novos clientes e envio de eventos para comunicação.
PropostaCredito: Serviço responsável por analisar propostas de crédito com base nos dados do cliente e emitir eventos de aprovação ou rejeição.
CartaoCredito: Serviço responsável por receber eventos de propostas aprovadas e emitir um ou mais cartões de crédito.

# Pré-requisitos
.NET 8 SDK
RabbitMQ instalado e em execução localmente.
Configuração do RabbitMQ
No terminal, inicie o RabbitMQ:
rabbitmq-server

Verifique que o RabbitMQ está em execução no localhost na porta 5672, com o usuário guest e senha guest.

Certifique-se de que as filas e exchanges necessárias são declaradas nos consumidores de cada serviço.

Execute cada serviço separadamente: CadastroClientes, PropostaCredito e CartaoCredito

# Estrutura de Mensagens e Fila
Cadastro de Cliente (cliente_cadastrado):
json
{
  "id": "8a962810-4da4-4655-9f6c-1fa4afb35f06",
  "nome": "Nome Cliente",
  "email": "cliente@teste.com",
  "dataNascimento": "2000-11-09T15:52:18.984Z",
  "rendaMensal": 10000,
  "scoreDeCredito": 800,
  "ativo": true
}

Proposta Aprovada (proposta_aprovada_queue):
json
{
  "clienteId": "8a962810-4da4-4655-9f6c-1fa4afb35f06",
  "valorAprovado": 5000,
  "parcelas": 12,
  "dataAprovacao": "2023-11-10T15:52:18.984Z"
}
![image](https://github.com/user-attachments/assets/907f8625-5b86-490e-bcca-d3e18e82aed5)

Logs de Monitoramento:
Certifique-se de que cada serviço está exibindo logs confirmando as operações realizadas, como:
Cadastro de clientes.
Análise de crédito e aprovação/rejeição de propostas.
Emissão de cartões com limites de crédito.



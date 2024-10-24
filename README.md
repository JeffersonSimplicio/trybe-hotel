# Trybe Hotel

O **Trybe Hotel** é uma API Web desenvolvida em ASP.NET para gerenciamento de hotéis, incluindo funcionalidades como reservas, clientes, quartos e outros serviços. O projeto foi construído utilizando boas práticas de programação, arquitetura em camadas, padrão de repositório e banco de dados relacional, facilitando o acesso e gerenciamento dos dados.

## Funcionalidades
- **Gerenciamento de Clientes**: Cadastro e consulta de clientes.
- **Gerenciamento de Reservas**: Criar, visualizar reservas.
- **Gerenciamento de Quartos**: Lista quartos e suas inforamações, cria e deleta.
- **Distancia até os Hoteis**: Usa o endereço do usuaio para calcular a distancia até os hoteis.

## Tecnologias Utilizadas
- **ASP.NET Core**: Framework para desenvolvimento da API.
- **Entity Framework Core**: ORM utilizado para mapear o banco de dados e realizar operações CRUD.
- **SQL Server**: Banco de dados relacional utilizado pela aplicação.
- **Docker**: Contêinerização da aplicação para facilitar o deploy e ambiente de desenvolvimento.
- **Migrations**: Utilizado para versionamento e controle de mudanças no banco de dados.

## Estrutura do Projeto
O projeto é organizado da seguinte forma:
- **Controllers**: Controladores responsáveis por processar as requisições HTTP e retornar as respostas adequadas.
- **Dto**: Objetos de transferência de dados, usados para enviar e receber dados da API.
- **Exceptions**: Exceções personalizadas para tratamento de erros e validação.
- **Models**: Classes que representam as entidades do banco de dados.
- **Repository**: Implementação do padrão de repositório, usado para abstrair o acesso aos dados.
- **Services**: Contém a lógica de negócios da aplicação.
- **Migrations**: Scripts de migração do banco de dados, gerados pelo Entity Framework.

## Requisitos

- [.NET 6.0](https://dotnet.microsoft.com/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Docker](https://www.docker.com/get-started)
- ✨ **Dica:** Antes de instalar SDK do .NET e o SQL Server, vejá o docker-compose e o Dockerfile
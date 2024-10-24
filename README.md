# Trybe Hotel 🏨

O **Trybe Hotel** é uma API Web desenvolvida em ASP.NET para gerenciamento de hotéis, incluindo funcionalidades como reservas, clientes, quartos e outros serviços. O projeto foi construído utilizando boas práticas de programação, arquitetura em camadas, padrão de repositório e banco de dados relacional, facilitando o acesso e gerenciamento dos dados. A aplicação foi desenvolvida durante o curso Aceleração em C# da Trybe.

## Funcionalidades
- **Gerenciamento de Clientes**: Cadastro e consulta de clientes.
- **Gerenciamento de Reservas**: Criar, visualizar reservas.
- **Gerenciamento de Quartos**: Lista quartos e suas informações, cria e deleta.
- **Distancia até os Hotéis**: Usa o endereço do usuário para calcular a distancia até os hotéis.

## Tecnologias Utilizadas
- **ASP.NET Core**: Framework para desenvolvimento da API.
- **Entity Framework Core**: ORM utilizado para mapear o banco de dados e realizar operações CRUD.
- **SQL Server**: Banco de dados relacional utilizado pela aplicação.
- **Docker**: Conteinerização da aplicação para facilitar o deploy e ambiente de desenvolvimento.
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

✨ Dica: Antes de instalar o SDK do .NET e o SQL Server, veja o `docker-compose` e o `Dockerfile`.

## Executando

<details>
  <summary><strong>💻 Localmente</strong></summary>

1. Clone o projeto para sua maquina e acessa a pasta da solução:
```bash
git clone git@github.com:JeffersonSimplicio/trybe-hotel.git
cd trybe-hotel
```

2. Inicie o banco que dados. Caso queira, é possível usar o DB disponível no Docker-Compose projeto:
```bash
docker-compose up -d db
```

3. Acesse a pasta do projeto e execute as migrations:
```bash
cd .\src\TrybeHotel\
dotnet ef database update
```

4. Ainda dentro da pasta do projeto, execute a aplicação:
```bash
dotnet run
```

O programa estará disponível em `https://localhost:5000/` ou `http://localhost:5001/`
</details>

<details>
  <summary><strong>🐳 Docker</strong></summary> 

1. Clone o projeto para sua maquina e acessa a pasta da solução:
```bash
git clone git@github.com:JeffersonSimplicio/trybe-hotel.git
cd trybe-hotel
```

2. Suba os servidores
```bash
docker-compose up -d --build
```

3. Para parar ou remover os containers após o uso, execute:
```bash
docker-compose down
```

O programa estará disponível em `http://localhost:8080/`
</details>
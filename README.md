# BookManager

Sistema de gerenciamento de livros desenvolvido como desafio técnico. Permite o cadastro e gerenciamento de **Gêneros**, **Autores** e **Livros** por meio de uma REST API em .NET 10 e uma SPA em Angular 19.

---

## Tecnologias

| Camada     | Tecnologia                                   |
|------------|----------------------------------------------|
| Backend    | .NET 10, ASP.NET Core, EF Core 10, PostgreSQL |
| Frontend   | Angular 19, NgRx 19, TypeScript               |
| Testes BE  | xUnit, Moq, FluentAssertions                 |
| Testes FE  | Jasmine, Karma                               |
| Docs API   | Scalar (OpenAPI 3)                           |

---

## Executar com Docker (recomendado)

A forma mais simples de rodar tudo junto: banco de dados, API e frontend.

### Pré-requisito

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) ou Docker + Docker Compose v2

### Subir o ambiente

```bash
docker compose up --build
```

Aguarde o build e a inicialização. As migrations são aplicadas automaticamente.

| Serviço     | URL                                    |
|-------------|----------------------------------------|
| Frontend    | http://localhost                       |
| API (REST)  | http://localhost/api/v1                |
| API Docs    | http://localhost/scalar/v1             |
| API direta  | http://localhost:5001/api/v1           |
| PostgreSQL  | localhost:5432                         |

### Parar e remover containers

```bash
docker compose down          # para e remove containers
docker compose down -v       # também remove o volume do banco de dados
```

---



- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [Node.js 18+](https://nodejs.org/)
- [Angular CLI 19](https://angular.io/cli) (`npm install -g @angular/cli`)
- [PostgreSQL 14+](https://www.postgresql.org/)

---

## Estrutura do projeto

```
desafio-siemens/
├── back/          # API .NET 10
│   ├── BookManager.Domain/
│   ├── BookManager.Infrastructure/
│   ├── BookManager.API/
│   └── BookManager.Tests/
└── front/         # Angular 19 SPA
    └── book-manager/
```

---

## Backend — configuração e execução

### 1. Banco de dados

Crie um banco PostgreSQL e anote a string de conexão.

### 2. Connection string

Edite `back/BookManager.API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=bookmanager_dev;Username=postgres;Password=sua_senha"
  }
}
```

### 3. Executar migrations

```bash
cd back
dotnet ef database update --project BookManager.Infrastructure --startup-project BookManager.API
```

### 4. Iniciar a API

```bash
cd back
dotnet run --project BookManager.API
```

A API ficará disponível em `http://localhost:5001`.  
Documentação interativa: **http://localhost:5001/scalar/v1**

---

## Frontend — configuração e execução

### 1. Instalar dependências

```bash
cd front/book-manager
npm install
```

### 2. Iniciar o servidor de desenvolvimento

```bash
ng serve
```

A aplicação ficará disponível em **http://localhost:4200**.

> O proxy para a API está configurado via `environment.ts` (`http://localhost:5001/api/v1`).

---

## Executar testes

### Backend (26 testes unitários)

```bash
cd back
dotnet test BookManager.slnx
```

### Frontend

```bash
cd front/book-manager
ng test --watch=false --browsers=ChromeHeadless
```

---

## Endpoints principais

| Método | Rota                        | Descrição                      |
|--------|-----------------------------|--------------------------------|
| GET    | /api/v1/genres              | Listar gêneros                 |
| POST   | /api/v1/genres              | Criar gênero                   |
| PUT    | /api/v1/genres/{id}         | Atualizar gênero               |
| DELETE | /api/v1/genres/{id}         | Excluir gênero                 |
| GET    | /api/v1/authors             | Listar autores                 |
| POST   | /api/v1/authors             | Criar autor                    |
| PUT    | /api/v1/authors/{id}        | Atualizar autor                |
| DELETE | /api/v1/authors/{id}        | Excluir autor                  |
| GET    | /api/v1/books               | Listar livros                  |
| POST   | /api/v1/books               | Criar livro                    |
| PUT    | /api/v1/books/{id}          | Atualizar livro                |
| DELETE | /api/v1/books/{id}          | Excluir livro                  |

---

## Funcionalidades

- CRUD completo de Gêneros, Autores e Livros
- Validações de domínio (nome único, ISBN único, autor/gênero existentes)
- Proteção contra exclusão de entidade com livros vinculados
- Documentação automática com Scalar (OpenAPI 3)
- Versionamento de API (`v1`)
- State management com NgRx (Actions, Reducers, Effects, Selectors, EntityAdapter)
- Lazy loading de módulos Angular
- Interceptor HTTP centralizado
- Tratamento de erros centralizado (backend e frontend)

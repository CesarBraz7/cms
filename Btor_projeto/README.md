# API Autenticação e CRUD do Login de usuário - .NET Framework
Este projeto é baseado em .NET para gerenciamento de um login do sistema de CMS da empresa Btor. Essa API permite ao administrador a utilização das operações do CRUD (create, read, update, delete) para a entidade user via requisições HTTP, como também a realização de operações de autenticação e update da senha do usuário cadastrado. 

## Requisitos
* .NET Framework 7
* ASP.NET Core
* SQL Server 2022
* Dapper 2.1.24
* JWT Bearer 7.0.14
* SQL Server Management Studio 2019
  

## Instalação
1\. Clonar o repositório do projeto:
```
git clone https://github.com/btor-elysium-devs/cms_btor.git
```
2\. Ao abrir o projeto, ir na pasta "appsettings.json" para iniciar a configuração da conexão com o banco de dados.

3\. Em "appsettings.json" editar sua string de conexão para de acordo com as configurações que seu banco está configurado
```
"DefaultConnection": "Server=localhost;Database=cms_btor;Trusted_Connection=True;"
```
* Essa String de conexão está configurada para de acordo com o arquivo "data.sql" do projeto, também para o tipo de servidor local da máquina e utilizando o tipo de autenticação do Windows.

4\. Criar o banco de dados através dos comandos sql do arquivo "data.sql" do projeto:
* Abra o SQL Management Studio 2019.
* Crie uma nova consulta e cole e execute os comandos do arquivo "data.sql" na pasta "Data" do projeto.
* Comandos abaixo:
```
CREATE DATABASE cms_btor;
GO

USE cms_btor; 
GO

CREATE TABLE Btor_User (
    id INT PRIMARY KEY IDENTITY(1,1),
    login VARCHAR(50) UNIQUE NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    role VARCHAR(50) NOT NULL,
    password VARCHAR(100) NOT NULL
);
GO

```

5\. Gerar a Secret Key para utilização do token de autenticação:

* Acesse o Site abaixo:
  ```
  https://it-tools.tech/token-generator
  ```
* Esclolhas as opções: Uppercase, Lowercase, Numbers, Symbols.
* Gere uma chave como indicado.

* Vá na pasta "Services"
* Arquivo "Configuration.cs"
* Cole a chave entre as "" (aspas).

6\. Executar o servidor da aplicação


## Utilização
Uma vez que o servidor da aplicação está sendo executado, é possível acessar a API através da URL https://localhost:7102/swagger/index.html.

* URL do métodos http: https://localhost:7102

 Aqui estão alguns exemplos de requisições que é possível realizar e o que é esperado em cada requisição:

* Acount Controller
  * `POST /api/Acount/login`
  ```json
  body:
  {
    "login": "string",
    "password": "string"
  }
  ```

  * `POST /api/Acount/forgot-password`
  ```json
  body:
  {
    "email": "string"
  }
  
  ```

  * `GET /api/Acount/authenticated`
  ```json
  headers:
  {
    Authorization: "Bearer (token válido)"
  }
  EX:
  {
    Authorization: "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyX2lkIjoiMSIsInRva2VuX3R5cGUiOiJhY2Nlc3MiLCJyb2xlIjoidXNlciIsIm5iZiI6MTcwMjE2MT"
  }
  ```

  * `POST /api/Acount/update-password`
  ```json
  body:
  {
    "password": "string",
    "confirmpassword": "string"
  }
  headers:
  {
    Authorization: "Bearer (token válido)"
  }
  ```

* User

  * `GET /User`

  * `POST /User`
  ```json
  body:
  {
    "login": "string",
    "email": "string",
    "password": "string",
    "role": "string"
  }
  ```

  * `GET /User/{Id}`

  * `PUT /User/{Id}`
  ```json
  body:
  {
    "login": "string",
    "email": "string",
    "password": "string",
    "role": "string"
  }
  ```

  * `DELETE /User/{Id}`


* Ao clonar o projeto, todas as dependencias já estarão instaladas. A string de conexão com o banco de dados SQL Server irá depender do tipo de conexão com o servidor que se está utilizando. A Secret Key é necessária para o funcionamento da API.
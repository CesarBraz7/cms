/*
Query para criacao do banco de dados.
*/
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

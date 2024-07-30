# Desafio

## Descri��o
Cria��o de um sistema de cadastro de livros, onde o usu�rio pode adicionar, editar, excluir e visualizar os livros cadastrados.

## Requisitos do Projeto

### Funcionalidades Principais
1. **CRUD para Livro, Autor e Assunto**
   - Implementar as opera��es de Create, Read, Update e Delete para as entidades Livro, Autor e Assunto conforme o modelo de dados fornecido.

2. **Tela Inicial**
   - A tela inicial deve conter um menu simples ou links diretos para as telas de CRUD constru�das.

3. **Modelo de Banco de Dados**
   - Seguir integralmente o modelo de banco de dados fornecido, com ajustes permitidos apenas para melhorias de performance.

### Interface do Usu�rio
4. **Estilo e Formata��o**
   - A interface deve ser simples, mas deve utilizar CSS para definir, no m�nimo, a cor e o tamanho dos componentes em tela.
   - A utiliza��o do Bootstrap ser� considerada um diferencial.

5. **Formata��o de Campos**
   - Campos que requerem formata��o espec�fica (como data e moeda) devem ser formatados adequadamente.

### Relat�rios
6. **Relat�rio**
   - Implementar um relat�rio utilizando o componente de relat�rios de sua prefer�ncia (Crystal, ReportViewer, etc).
   - A consulta do relat�rio deve ser proveniente de uma view criada no banco de dados.
   - O relat�rio deve trazer informa��es das tr�s tabelas principais, agrupando os dados por autor (considerar que um livro pode ter mais de um autor).

### Desenvolvimento e Qualidade
7. **TDD (Test Driven Development)**
   - A utiliza��o de TDD ser� considerada um diferencial.

8. **Tratamento de Erros**
   - Implementar tratamento de erros espec�fico, evitando o uso de try-catch gen�ricos, especialmente para erros de banco de dados.

### Mensagens e Labels
9. **Mensagens do Sistema**
   - As mensagens emitidas pelo sistema, labels, etc., ficam a crit�rio do desenvolvedor.

### Requisitos Adicionais
10. **Valor do Livro**
    - Incluir uma forma de informar o valor (em R$) do livro dependendo da forma de compra (exemplos: balc�o, self-service, internet, evento, etc).
    - Implementar tanto o modelo quanto a funcionalidade necess�ria para que essa informa��o esteja dispon�vel para o usu�rio final.

### Documenta��o e Scripts
11. **Scripts e Instru��es de Implanta��o**
    - Guardar todos os scripts e instru��es necess�rios para a implanta��o do projeto.
    - 
## BluePrint de arquitetura
Banco de dados utilizado: PostgreSQL/InMemorian
Backend: C# .NET Core 8
Frontend: Angular 16

## DDL
```sql
CREATE TABLE Livro (
    codl UUID PRIMARY KEY,
    titulo VARCHAR(40) NOT NULL,
    Editora VARCHAR(40) NOT NULL,
    Edicao int NOT NULL,
    AnoPublicacao VARCHAR(4) NOT NULL
);

CREATE TABLE assunto (
    codAs UUID PRIMARY KEY,
    descricao VARCHAR(20) NOT NULL
);

CREATE TABLE Autor (
    codAu UUID PRIMARY KEY,
    nome VARCHAR(40) NOT NULL
);

CREATE TABLE Livro_Autor (
    Livro_Codl UUID NOT NULL,
    Autor_CodAu UUID NOT NULL,
    PRIMARY KEY (Livro_Codl, Autor_CodAu),
    FOREIGN KEY (Livro_Codl) REFERENCES Livro(codl),
    FOREIGN KEY (Autor_CodAu) REFERENCES Autor(codAu)
);

CREATE TABLE Livro_Assunto (
    Livro_Codl UUID NOT NULL,
    Assunto_CodAs UUID NOT NULL,
    PRIMARY KEY (Livro_Codl, Assunto_CodAs),
    FOREIGN KEY (Livro_Codl) REFERENCES Livro(codl),
    FOREIGN KEY (Assunto_CodAs) REFERENCES Assunto(codAs)
);

CREATE TABLE Tipo_Compra (
    codc UUID PRIMARY KEY,
    nome VARCHAR(40) NOT NULL
);

CREATE TABLE Preco_Livro (
    Livro_Codl UUID NOT NULL,
    Tipo_codc UUID NOT NULL,
    Preco DECIMAL(18, 2) NOT NULL,
    PRIMARY KEY (Livro_Codl, Tipo_codc),
    FOREIGN KEY (Livro_Codl) REFERENCES Livro(Codl),
    FOREIGN KEY (Tipo_codc) REFERENCES Tipo_Compra(codc)
);

create view vw_livrodetalhes as
SELECT 
    a.nome AS Autores,
    l.codl AS Livro_Id,
    l.titulo AS Livro_Titulo,
    l.Editora,
    l.Edicao,
    l.AnoPublicacao AS Ano_publicacao,
    STRING_AGG(DISTINCT ass.descricao, ', ') AS Assuntos,
    STRING_AGG(DISTINCT CONCAT(tc.nome, ': R$', pl.Preco), ', ') AS Precos
FROM 
    Livro l
LEFT JOIN 
    Livro_Autor la ON l.codl = la.Livro_Codl
LEFT JOIN 
    Autor a ON la.Autor_CodAu = a.codAu
LEFT JOIN 
    Livro_Assunto las ON l.codl = las.Livro_Codl
LEFT JOIN 
    Assunto ass ON las.Assunto_CodAs = ass.codAs
LEFT JOIN 
    Preco_Livro pl ON l.codl = pl.Livro_Codl
LEFT JOIN 
    Tipo_Compra tc ON pl.Tipo_codc = tc.codc
GROUP BY 
    a.codAu, a.nome, l.codl, l.titulo, l.Editora, l.Edicao, l.AnoPublicacao
ORDER BY 
    a.nome, l.titulo;
```

-- Cria o banco de dados se ele não existir
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'SaborGregoDatabase')
BEGIN
    CREATE DATABASE SaborGrego;
END
GO

USE SaborGrego
GO

--------------------------------------------------------------------------------
-- 1. CRIAÇÃO DAS TABELAS (SEM CHAVES ESTRANGEIRAS)
--------------------------------------------------------------------------------

-- Tabela de Usuários (Clientes, Funcionários, Entregadores)
CREATE TABLE [Usuarios] (
    [Id] INT IDENTITY(1, 1) NOT NULL,
    [Nome] NVARCHAR(255) NOT NULL,
    [Telefone] NVARCHAR(20) NOT NULL,
    [Email] NVARCHAR(255) NOT NULL,
    [Senha] NVARCHAR(MAX) NOT NULL, -- SenhaHash, mantendo NVARCHAR(MAX) para o hash
    [Role] INT NOT NULL, -- Mapeamento para o enum UserRole
    CONSTRAINT [PK_Usuarios] PRIMARY KEY ([Id])
);

-- Tabela de Produtos
CREATE TABLE [Produtos] (
    [Id] INT IDENTITY(1, 1) NOT NULL,
    [Nome] NVARCHAR(255) NOT NULL,
    [Preco] DECIMAL(10, 2) NOT NULL,
    [Categoria] NVARCHAR(50) NOT NULL,
    [Descricao] NVARCHAR(MAX) NULL, -- Permitindo texto longo e nulo
    [Imagem] NVARCHAR(MAX) NOT NULL, -- Assumindo URL ou path da imagem
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Produtos] PRIMARY KEY ([Id])
);

-- Tabela de Endereços
CREATE TABLE [Enderecos] (
    [Id] INT IDENTITY(1, 1) NOT NULL,
    [UsuarioId] INT NOT NULL, -- Chave estrangeira para o dono do endereço
    [Apelido] NVARCHAR(100) NOT NULL,
    [Logradouro] NVARCHAR(255) NOT NULL,
    [Numero] NVARCHAR(10) NOT NULL,
    [Complemento] NVARCHAR(100) NOT NULL,
    [Bairro] NVARCHAR(100) NOT NULL,
    [Ativo] BIT NOT NULL,
    CONSTRAINT [PK_Enderecos] PRIMARY KEY ([Id])
);

-- Tabela de Pedidos
CREATE TABLE [Pedidos] (
    [Id] INT IDENTITY(1, 1) NOT NULL,
    [ClienteId] INT NOT NULL, -- Cliente que fez o pedido
    [FuncionarioId] INT NULL, -- Funcionário que processou o pedido (Opcional)
    [EntregadorId] INT NULL, -- Entregador designado (Opcional)
    [DataPedido] DATETIME2 NOT NULL,
    [TotalPedido] DECIMAL(10, 2) NOT NULL,
    [Status] INT NOT NULL, -- Mapeamento para o enum StatusPedido
    [EnderecoId] INT NOT NULL, -- Endereço de entrega (Chave Estrangeira)
    [MetodoPagamento] INT NOT NULL, -- Mapeamento para o enum MetodoPagamento
    -- A coluna 'ProdutoId' foi removida, pois é redundante. A relação é feita via DetalhePedido.
    CONSTRAINT [PK_Pedidos] PRIMARY KEY ([Id])
);

-- Tabela DetalhePedido (Itens do Pedido)
-- Corrigido: Incluí um Id como PK autoincremento para melhor compatibilidade com o EF Core.
CREATE TABLE [DetalhePedido] (
    [Id] INT IDENTITY(1, 1) NOT NULL, -- Chave Primária correta para o EF Core
    [PedidoId] INT NOT NULL, -- Chave Estrangeira para Pedidos
    [ProdutoId] INT NOT NULL, -- Chave Estrangeira para Produtos
    [NomeProduto] NVARCHAR(100) NOT NULL,
    [Imagem] NVARCHAR(MAX) NULL,
    [Quantidade] INT NOT NULL,
    [PrecoUnitario] DECIMAL(18, 2) NOT NULL,
    CONSTRAINT [PK_DetalhePedido] PRIMARY KEY ([Id])
);

-- Tabela Carrinhos (Para o carrinho de compras)
CREATE TABLE [Carrinhos] (
    [Id] INT IDENTITY(1, 1) NOT NULL,
    [SessionId] NVARCHAR(MAX) NOT NULL,
    [Total] DECIMAL(18, 2) NOT NULL,
    CONSTRAINT [PK_Carrinhos] PRIMARY KEY ([Id])
);

-- Tabela CarrinhoItens (Itens no Carrinho)
CREATE TABLE [CarrinhoItens] (
    [Id] INT IDENTITY(1, 1) NOT NULL,
    [Nome] NVARCHAR(255) NOT NULL,
    [ProdutoId] INT NOT NULL, -- FK para Produtos
    [Quantidade] INT NOT NULL,
    [Preco] DECIMAL(18, 2) NOT NULL,
    [Imagem] NVARCHAR(MAX) NOT NULL,
    [CarrinhoId] INT NULL, -- FK para Carrinhos
    CONSTRAINT [PK_CarrinhoItens] PRIMARY KEY ([Id])
);

--------------------------------------------------------------------------------
-- 2. CHAVES ESTRANGEIRAS E ÍNDICES
--------------------------------------------------------------------------------

-- Enderecos: Chave Estrangeira
CREATE INDEX [IX_Enderecos_UsuarioId] ON [Enderecos] ([UsuarioId]);
ALTER TABLE [Enderecos] ADD CONSTRAINT [FK_Enderecos_Usuarios_UsuarioId]
    FOREIGN KEY ([UsuarioId]) REFERENCES [Usuarios] ([Id]) ON DELETE CASCADE;
GO

-- Pedidos: Chaves Estrangeiras (Cliente, Funcionário, Entregador e Endereço)

-- Cliente (UsuarioId)
ALTER TABLE [Pedidos] ADD CONSTRAINT [FK_Pedidos_Usuarios_ClienteId]
    FOREIGN KEY ([ClienteId]) REFERENCES [Usuarios] ([Id]) ON DELETE NO ACTION;

-- Funcionário (FuncionarioId)
CREATE INDEX [IX_Pedidos_FuncionarioId] ON [Pedidos] ([FuncionarioId]);
ALTER TABLE [Pedidos] ADD CONSTRAINT [FK_Pedidos_Usuarios_FuncionarioId]
    FOREIGN KEY ([FuncionarioId]) REFERENCES [Usuarios] ([Id]) ON DELETE NO ACTION;

-- Entregador (EntregadorId)
CREATE INDEX [IX_Pedidos_EntregadorId] ON [Pedidos] ([EntregadorId]);
ALTER TABLE [Pedidos] ADD CONSTRAINT [FK_Pedidos_Usuarios_EntregadorId]
    FOREIGN KEY ([EntregadorId]) REFERENCES [Usuarios] ([Id]) ON DELETE NO ACTION;

-- Endereço (EnderecoId)
CREATE INDEX [IX_Pedidos_EnderecoId] ON [Pedidos] ([EnderecoId]);
ALTER TABLE [Pedidos] ADD CONSTRAINT [FK_Pedidos_Enderecos_EnderecoId]
    FOREIGN KEY ([EnderecoId]) REFERENCES [Enderecos] ([Id]) ON DELETE NO ACTION;
GO


-- DetalhePedido: Chaves Estrangeiras
CREATE INDEX [IX_DetalhePedido_PedidoId] ON [DetalhePedido] ([PedidoId]);
CREATE INDEX [IX_DetalhePedido_ProdutoId] ON [DetalhePedido] ([ProdutoId]);

ALTER TABLE [DetalhePedido] ADD CONSTRAINT [FK_DetalhePedido_Pedidos_PedidoId]
    FOREIGN KEY ([PedidoId]) REFERENCES [Pedidos] ([Id]) ON DELETE CASCADE;

ALTER TABLE [DetalhePedido] ADD CONSTRAINT [FK_DetalhePedido_Produtos_ProdutoId]
    FOREIGN KEY ([ProdutoId]) REFERENCES [Produtos] ([Id]) ON DELETE CASCADE;
GO

-- CarrinhoItens: Chaves Estrangeiras
CREATE INDEX [IX_CarrinhoItens_CarrinhoId] ON [CarrinhoItens] ([CarrinhoId]);
ALTER TABLE [CarrinhoItens] ADD CONSTRAINT [FK_CarrinhoItens_Carrinhos_CarrinhoId]
    FOREIGN KEY ([CarrinhoId]) REFERENCES [Carrinhos] ([Id]) ON DELETE CASCADE;

CREATE INDEX [IX_CarrinhoItens_ProdutoId] ON [CarrinhoItens] ([ProdutoId]);
ALTER TABLE [CarrinhoItens] ADD CONSTRAINT [FK_CarrinhoItens_Produtos_ProdutoId]
    FOREIGN KEY ([ProdutoId]) REFERENCES [Produtos] ([Id]) ON DELETE CASCADE;
GO
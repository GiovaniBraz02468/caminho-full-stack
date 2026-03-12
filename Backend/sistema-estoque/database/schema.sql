CREATE DATABASE sistema_estoque;

USE sistema_estoque;

-- USUARIOS
CREATE TABLE usuarios (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(150) NOT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    senha_hash VARCHAR(255) NOT NULL,
    senha_temporaria BOOLEAN NOT NULL DEFAULT FALSE,
    data_criacao DATETIME NOT NULL
);

-- PRODUTOS
CREATE TABLE produtos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    usuario_id INT NOT NULL,
    nome VARCHAR(150) NOT NULL,
    descricao TEXT NOT NULL,
    quantidade_atual INT NOT NULL DEFAULT 0,
    valor_unitario DECIMAL(10,2) NOT NULL,
    data_criacao DATETIME NOT NULL,

    CONSTRAINT fk_produto_usuario
    FOREIGN KEY (usuario_id)
    REFERENCES usuarios(id)
);

-- MOVIMENTACOES
CREATE TABLE movimentacoes (
    id INT AUTO_INCREMENT PRIMARY KEY,
    produto_id INT NOT NULL,
    quantidade INT NOT NULL,
    valor_unitario DECIMAL(10,2) NOT NULL,
    tipo INT NOT NULL,
    data_movimentacao DATETIME NOT NULL,

    CONSTRAINT fk_mov_produto
    FOREIGN KEY (produto_id)
    REFERENCES produtos(id)
);

-- INDICES (PERFORMANCE)
CREATE INDEX idx_produtos_usuario
ON produtos(usuario_id);

CREATE INDEX idx_movimentacoes_produto
ON movimentacoes(produto_id);

CREATE INDEX idx_movimentacoes_data
ON movimentacoes(data_movimentacao);
CREATE TABLE database.tipo_boleto (
	id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
	descricao  TEXT NOT NULL,
	data_cadastro  INTEGER  NOT NULL,
	data_alteracao INTEGER NULL
);

CREATE TABLE database.boleto (
	id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
	descricao  TEXT NOT NULL,
	tipo_id  INTEGER NOT NULL,
	valor  NUMERIC NOT NULL,
	data_emissao  INTEGER NOT NULL,
	data_vencimento  INTEGER NOT NULL,
	situacao  INTEGER NOT NULL,
	data_cadastro  INTEGER  NOT NULL,
	data_alteracao INTEGER NULL,
	FOREIGN KEY (tipo_id) REFERENCES tipo_boleto (id)
);
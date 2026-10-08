# Controle de Boletos

Aplicação WPF para cadastro de boletos, categorias, filtros e totais por período.

## Executar

No Windows, com o SDK e o runtime Desktop do .NET 6 instalados:

```powershell
dotnet run --project ControleDeBoletos/ControleDeBoletos
```

O projeto continua usando `net6.0-windows` e os pacotes originais. Nesta máquina,
que possui somente o runtime .NET 10, a verificação foi executada com
`dotnet --roll-forward Major` após a compilação. Para executar a aplicação nessa
mesma configuração:

```powershell
dotnet build ControleDeBoletos/ControleDeBoletos.sln
dotnet --roll-forward Major ControleDeBoletos/ControleDeBoletos/bin/Debug/net6.0-windows/ControleDeBoletos.dll
```

## Banco de dados

Na primeira execução, o SQLite cria automaticamente o arquivo e as tabelas pelo
modelo do Entity Framework; não é necessário executar `SCRIPTO.sql`.

Para preservar instalações existentes, a aplicação procura `database.db` primeiro
na pasta de trabalho e depois na pasta do executável. Se não encontrar, usa
`%LOCALAPPDATA%\ControleDeBoletos\database.db`, uma pasta gravável do usuário.
Um banco existente é reutilizado sem apagar seus dados. `EnsureCreated` cria o
esquema inicial; não realiza migrações de esquemas existentes.

Para backup, feche a aplicação antes de copiar o arquivo. Arquivos do banco e
saídas de compilação são ignorados pelo Git.

## Layout adaptável

- Janela redimensionável, com tamanho mínimo de 480 × 420 unidades do WPF.
- Cadastro em duas colunas quando há espaço e uma coluna em janelas estreitas,
  com rolagem vertical para alcançar todos os campos e o botão de salvar.
- Filtros e resumos quebram em linhas; suas áreas têm rolagem própria em janelas
  baixas, mantendo espaço para a tabela.
- Categorias e totais alternam entre seções lado a lado e empilhadas.
- Tabelas preservam larguras legíveis e permitem rolagem horizontal e vertical.

## Verificação

O executável de verificação usa um banco isolado em `artifacts/verificacao`, testa
criação, reabertura, persistência, consulta, edição e exclusão, e mede/renderiza as
quatro abas em seis tamanhos (480 × 420 até 1920 × 1080), sem abrir uma janela.
As imagens PNG ficam nessa mesma pasta para inspeção visual.

```powershell
dotnet run --project tests/ControleDeBoletos.Verificacao
```

Com somente o runtime .NET 10 instalado:

```powershell
dotnet build tests/ControleDeBoletos.Verificacao
dotnet --roll-forward Major tests/ControleDeBoletos.Verificacao/bin/Debug/net6.0-windows/ControleDeBoletos.Verificacao.dll
```

Essa verificação cobre layout WPF e acesso ao SQLite. A interação com a janela
no desktop, como arrastar bordas e abrir o calendário, deve ser conferida ao usar
a aplicação.

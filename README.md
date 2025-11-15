# FelterAPI

API .NET 8 para o Felter Technology Ecosystem (master + clientes).

## Como rodar local

```bash
cd FelterAPI
dotnet restore
dotnet run
```

- Swagger em: http://localhost:5000
- Login do Swagger (Basic Auth):
  - usuário: dev.feltertechnology@gmail.com
  - senha: #MjlEgdc123451958#

## Usuário master padrão

Chame uma vez:

POST /api/auth/register-master

Ele cria:

- Email: lfeltertechnology@gmail.com
- Senha: Egdc1958

Depois faça login em:

POST /api/auth/login

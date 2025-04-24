# 📌 Projeto: Gerenciador de Perfis e Permissões

Este projeto é uma API RESTful desenvolvida com **ASP.NET Core**, que gerencia perfis de usuários e suas permissões (parâmetros).  
Inclui um serviço de atualização automática dos parâmetros a cada 5 minutos e documentação integrada via **Swagger**.

---

## 🚀 Funcionalidades

- Armazenamento em memória de perfis e seus parâmetros.
- Atualização automática periódica dos parâmetros.
- API REST para manipulação completa dos perfis.
- Validação de permissões por perfil.
- Interface de teste via Swagger UI.

---

## 🧱 Estrutura dos Dados

```csharp
class ProfileParameter
{
    string ProfileName;
    Dictionary<string, string> Parameters; // Ex: { "CanEdit": "true", "CanDelete": "false" }
}
```

---

## 🛠 Como Rodar o Projeto

```bash
dotnet restore
dotnet build
dotnet run
```

Depois, acesse o Swagger em:  
👉 `https://localhost:{porta}/swagger`

---

## 📖 Endpoints da API

| Verbo | Rota | Descrição |
|-------|------|-----------|
| GET | `/api/profiles` | Retorna todos os perfis e seus parâmetros |
| GET | `/api/profiles/{profileName}` | Retorna um perfil específico |
| POST | `/api/profiles` | Cria um novo perfil |
| PUT | `/api/profiles/{profileName}` | Atualiza os parâmetros de um perfil existente |
| DELETE | `/api/profiles/{profileName}` | Remove um perfil |
| GET | `/api/profiles/{profileName}/validate?action=CanEdit` | Verifica se o perfil tem permissão para determinada ação |

---

## 🔄 Atualização Automática

A cada **5 minutos**, um serviço em background atualiza o valor de `CanEdit` para simular mudanças de permissão em tempo real.

---

## 📂 Estrutura de Pastas

```
/Models
  - ProfileParameter.cs
/Services
  - IProfileParameterService.cs
  - ProfileParameterService.cs
  - ProfileParameterUpdater.cs
/Controllers
  - ProfilesController.cs
/Filters
 - ProfileParameterSchemaFilter.cs
Program.cs
README.md
```

---

## 📚 Dependências

- ASP.NET Core 8+
- Swashbuckle.AspNetCore (Swagger)
- Microsoft.Extensions.Hosting

---

## ✅ Possíveis Melhorias Futuras

- Persistência com banco de dados (SQL ou NoSQL)
- Autenticação e autorização com JWT
- Interface web para administrar os perfis


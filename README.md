### ToDoAPI

Esse projeto é uma minimal API do projeto ToDoPortifolio que será integrada à outros serviços

---
Para rodar localmente:
- Verifique a cadeia de conexão do seu banco de dados SQL SERVER em appsettings.json
- Verifique o endereço do frontend no Cors em Program.cs
  ```C#
    builder.Services.AddCors(options =>
  {
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins(""http://localhost:{Porta_FrontEnd}") 
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); // necessário se usar autenticação (JWT, cookies, etc)
        });
  });
  ```

## Sobre

Microsserviço com ASP.NET Core 7.0, EF Core 7, SQL Server, AutoMapper, JWT compartilhado entre os serviços para autenticação e autorização, Caching com Redis, documentação com Swagger e Docker.

## Comandos

- Clonar repositório

    ```
    git clone https://github.com/GeoSGon/api-rest-asp.net.git
    ```

- Construir a imagem do Docker

    ```
    docker build -t nome-da-imagem .
    ```

- Executar o container

    ```
    docker run -p 80:80 nome-da-imagem
    ```
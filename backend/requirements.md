# Contexto 

Esse é um backend de um app que irá simular a latência real de trocas de mensagens dentre usuarios (1-1 ou 1-n) utilizando a velocidade real de aves. 

# Stack
- .NET 10
- RabbitMq
- Postgres

# Entidades

User {
    Guid id
    String name 
    String email
    String password 
    DateTime createdAt
    Enum role 
    List<Message> Messages
}


Enum Role {
    Admin = 0
    User = 1
}

Message {
    Guid id
    Guid senderId
    Guid receiverId
    String message
    Anexo (Foto, arquivo etc)
}


## ARQUITETURA

**CONTROLLERS** -> **INTERFACE DE APPLICATION (IService)**  -> **INTERFACE DE REPOSITORY(IRepository)**


API/
    /Controllers
    /Middelaware

DOMAIN/
    /Entities
    /Interfaces(Repository)
    /Exceptions

APPLICATION/
    /Services
        exampleService
        /Interface
        iexampleService
    /Dto
    /Mappers 
    /Exceptions

INFRASTRUCTURE/
    /DATA
        AppDbContext
    /CONFIGURATIONS
        UserConfigurationDb.cs
    /SEEDS

        
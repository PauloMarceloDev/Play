# Documentação do Projeto Play

Esta documentação fornece uma visão geral dos serviços, pacotes e infraestrutura utilizados no projeto Play. O objetivo é oferecer um entendimento claro das responsabilidades de cada componente e como eles interagem entre si.

## Serviços

### Play.Catalog.Services

Este serviço é a espinha dorsal para a gestão do catálogo de itens, permitindo operações CRUD e emitindo eventos relacionados a estas ações.

#### Endpoints:

- **ListAll**: Retorna uma lista completa dos itens disponíveis no catálogo.
- **GetById**: Fornece os detalhes de um item, identificado por seu ID.
- **Create (ItemCreated)**: Adiciona um item ao catálogo, emitindo um evento correspondente.
- **Update (ItemUpdated)**: Atualiza as informações de um item existente, com um evento de notificação.
- **Delete (ItemDeleted)**: Exclui um item do catálogo, desencadeando um evento de exclusão.

#### Documentação:

- **items**: Contém os dados referentes aos itens do catálogo.

### Play.Inventory.Services

Gerencia o inventário dos usuários, incluindo a adição de itens e a sincronização com as atualizações do catálogo.

#### Endpoints:

- **ListItemsByUserId**: Exibe os itens presentes no inventário de um usuário específico.
- **GrantItem**: Adiciona um item específico ao inventário de um usuário.

#### Consumidores:

- **CatalogItemCreated**: Atualiza o inventário em resposta à adição de um novo item ao catálogo.
- **CatalogItemUpdated**: Sincroniza o inventário com as atualizações de itens no catálogo.
- **CatalogItemDeleted**: Ajusta o inventário em resposta à remoção de itens do catálogo.

#### Documentação:

- **catalogItems**: Refletido pelas mudanças feitas pelos consumidores em resposta às alterações do catálogo.
- **inventoryItems**: Mantém os registros dos itens de inventário dos usuários.

## Pacotes

### Play.Common

Fornece funcionalidades comuns e fundamentais necessárias por diversos microserviços.

#### Instrumentação:

- **MassTransit com RabbitMQ**: Facilita a comunicação entre serviços através de mensagens.
- **MongoDB**:
    - **Entidade Básica**: Define a estrutura base para os documentos do banco.
    - **Repositório Genérico**: Oferece uma abordagem padronizada para operações de banco de dados.
- **Configuração de Microserviço**: Inclui aspectos básicos como o nome do serviço.

### Play.Catalog.Contracts

Contém os contratos de comunicação do microserviço Catalog, essenciais para a integração e comunicação eficaz.

#### Modelos de contratos:

- **CatalogItemCreated**
- **CatalogItemUpdated**
- **CatalogItemDeleted**

## Infraestrutura

### Play.Infra

Centraliza as configurações de infraestrutura do projeto, garantindo um ambiente coerente e otimizado.

#### Containers com Docker Compose:

- **MongoDB**: Fornece um banco de dados NoSQL, escalável e flexível.
- **RabbitMQ**: Oferece um sistema de mensageria para comunicação entre serviços.
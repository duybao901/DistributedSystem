# Redis
docker pull redis/redis-stack-server:latest
docker run -p 6379:6379 --name redis -d redis/redis-stack-server
docker ps
docker exec -it redis redis-cli

# RabitMQ
docker compose -f docker-compose.Dev.infrastructure.yaml up

                  ┌─────────────────────────┐
                  │        Client / UI       │
                  └───────────┬─────────────┘
                              │
                              ▼
                   ┌────────────────────┐
                   │   API Gateway      │
                   │     (YARP)         │
                   └───────┬────────────┘
       ┌────────────────────┼──────────────────────┐
       │                    │                      │
       ▼                    ▼                      ▼
┌──────────────┐    ┌──────────────┐      ┌────────────────┐
│  Auth API     │    │ Command API  │      │  Query API      │
│  (6001)       │    │ (3000)       │      │  (4000)         │
│ JWT Generator │    │ Write (CQRS) │      │ Read  (CQRS)    │
└──────┬────────┘    └───────┬──────┘      └────────┬───────┘
       │                     │                      ^
       ▼                     │                      │
   ┌────────┐               │                       │
   │ Redis  │ ← Cache/Token │                       │
   └────────┘               │                       │
                            ▼                       
                  ┌────────────────┐       ┌────────────────┐
                  │   SQL Server   │       │    MongoDB      │
                  │ (Write Model)  │       │ (Read Model)    │
                  └──────┬─────────┘       └────────┬────────┘
                         │                          ^
                         │ (Data Sync via Message   │
                         │  Broker: RabbitMQ/Kafka) │
                         ▼                          
                  ┌─────────────────────────────────────┐
                  │     Event Bus / Message Broker       │
                  └─────────────────────────────────────┘

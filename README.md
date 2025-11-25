# Redis
docker pull redis/redis-stack-server:latest
docker run -p 6379:6379 --name redis -d redis/redis-stack-server
docker ps
docker exec -it redis redis-cli

# RabitMQ
docker compose -f docker-compose.Dev.infrastructure.yaml up

<img width="671" height="601" alt="JK drawio" src="https://github.com/user-attachments/assets/488f3be6-55e3-4667-aa3e-9eb72f70b6ae" />

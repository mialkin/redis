# Redis

Run infrastructure:

```bash
make run-infrastructure
```

- Swagger UI: <http://localhost:4010>
- RedisInsight port: <http://localhost:4030>

In RedisInsight UI:

1. Select **I already have a database** → **Connect to a Redis Database**.
2. Use `host.docker.internal` for host, `4020` for port, and `0` for database name.
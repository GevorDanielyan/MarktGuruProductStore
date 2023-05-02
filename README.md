
# Simple CRUD web api

This is a CRUD web API developed using .NET 7. The application uses Dapper ORM to interact with a Postgres database. Redis is utilized for caching frequently accessed data to improve performance. The application is containerized using Docker and can be orchestrated using Docker Compose. The API provides standard Create, Read, Update and Delete operations for managing data stored in the database.


## Run Locally

Clone the project

```bash
  git clone https://github.com/GevorDanielyan/MarktGuruProductStore.git
```

Go to the project directory

```bash
  cd project
```

Start the server

```bash
  docker-compose up
```


## Contributing

Contributions are always welcome!


## Licensing

This project is licensed under the MIT License. [MIT](https://choosealicense.com/licenses/mit/)


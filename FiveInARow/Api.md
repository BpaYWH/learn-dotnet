# Five In A Row API

This document outlines the API endpoints for managing users and game records. Users can be created, read, and updated (CRU operations), while game records can only be created and read (CR operations).

- [Five In A Row API](#five-in-a-row-api)
  - [Create User](#create-user)
    - [Create User Request](#create-user-request)
    - [Create User Response](#create-user-response)
  - [Read User](#read-user)
    - [Read User Request](#read-user-request)
    - [Read User Response](#read-user-response)
  - [Update User](#update-user)
    - [Update User Request](#update-user-request)
    - [Update User Response](#update-user-response)
  - [Create Game Record](#create-game-record)
    - [Create Game Record Request](#create-game-record-request)
    - [Create Game Record Response](#create-game-record-response)
  - [Read Game Record](#read-game-record)
    - [Read Game Record Request](#read-game-record-request)
    - [Read Game Record Response](#read-game-record-response)

## User API

### Create User

#### Create User Request

```http
POST /users
```

```json
{
  "name": "John Doe",
  "password": "password123",
  "email": "johndoe@example.com"
}
```

#### Create User Response

```http
201 Created
```

```yml
Location: {{host}}/users/{{id}}
```

```json
{
  "id": 1,
  "name": "John Doe",
  "email": "johndoe@example.com",
  "gameRecords": []
}
```

### Read User

#### Read User Request

```http
GET /users/{{id}}
```

#### Read User Response

```http
200 Ok
```

```json
{
  "id": 1,
  "name": "John Doe",
  "email": "johndoe@example.com",
  "gameRecords": [1, 2]
}
```

### Update User

#### Update User Request

```http
PUT /users/{{id}}
```

```json
{
  "name": "Updated Name",
  "password": "updatedpassword456",
  "email": "updatedemail@example.com",
  "gameRecords": [1, 2]
}
```

#### Update User Response

```http
204 No Content
```

## Game Record API

### Create Game Record

#### Create Game Record Request

```http
POST /game-records
```

```json
{
  "player1Id": 1,
  "player2Id": 2,
  "startTime": "2023-09-17T10:00:00",
  "endTime": "2023-09-17T11:30:00",
  "winnerId": 1
}
```

#### Create Game Record Response

```http
201 Created
```

```yml
Location: {{host}}/game-records/{{id}}
```

```json
{
  "id": 1,
  "player1Id": 1,
  "player2Id": 2,
  "startTime": "2023-09-17T10:00:00",
  "endTime": "2023-09-17T11:30:00",
  "winnerId": 1
}
```

### Read Game Record

#### Read Game Record Request

```http
GET /game-records/{{id}}
```

#### Read Game Record Response

```http
200 Ok
```

```json
{
  "id": 1,
  "player1Id": 1,
  "player2Id": 2,
  "startTime": "2023-09-17T10:00:00",
  "endTime": "2023-09-17T11:30:00",
  "winnerId": 1
}
```

# Сервис управления безопасностью

Этот сервис предоставляет REST API для управления безопасностью пользователей. Он поддерживает регистрацию, авторизацию, изменение пароля, а также получение новых токенов доступа и обновления.

## Endpoints

### User Registration

- **Endpoint:** `POST /api/auth/register`
- **Description:** Registers a new user.
- **Request Body:**
  ```json
  {
    "username": "test user",
    "password": "11111",
    "email": "test@mail.ru"
  }
- **Response**:
   - `201 Created` if successful
   - `400 Bad Request` if there are validation errors

### User Authentication

- **Endpoint:** `POST /api/auth/login`
- **Description:** Authenticates a user and returns access and refresh tokens.
- **Request Body:**
  ```json
  {
    "username": "test user",
    "password": "11111"
  }
- **Response**:
   - `200 OK` with tokens if successful
   - `401 Unauthorized` if the credentials are invalid


### Change Password

- **Endpoint:** `PUT /api/auth/changePassword`
- **Description:** Changes the user's password.
- **Request Body:**
  ```json
  {
    "username": "test user",
    "password": "11111",
    "newPassword": "12345"
  }
- **Response**:
   - `200 OK` if successful
   - `400 Bad Request` if there are validation errors
   - `404 Not Found` if the user does not exist
 
### Refresh Tokens

- **Endpoint:** `POST /api/auth/refreshTokens`
- **Description:** Refreshes access tokens using a refresh token.
- **Request Body:**
  ```json
  {
    "refreshToken": "p7j3dnEZVLr4IfGH9f9KXCdAtOenb/eeCL6L3cHAz8c="
  }
- **Response**:
   - `200 OK` with new tokens if successful
   - `400 Bad` Request if the refresh token is invalid
     
## Configuration
### Example Configuration
  ```json
  {
    "ConsulConfig": {
      "Address": "http://host.docker.internal:8500",
      "ServiceName": "Security Service",
      "ServiceAddress": "localhost",
      "ServicePort": 8082,
      "Tags": [ "api", "auth" ]
    },

    "ConsulKey": "JwtTokenSettings",

    "JwtTokenSettings": {
      "SecretKey": "K17T6p+mYlBuIll6EOQDUmAdM6xmzeHOpE+O35zsAvw=",
      "Issuer": "TMS.Serurity.Service",
      "Audience": "TMS.Notes.Service",
      "AccessTokenLifetimeInMinutes": 15,
      "RefreshTokenLifetimeInMinutes": 7
    },

    "DatabaseConnection": "Server=postgres;Port=5432;Database=db;User Id=postgres;Password=admin;"
  }

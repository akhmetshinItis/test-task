# Vitacore.Test

Онлайн-аукцион картинок мандаринок на ASP.NET Core + PostgreSQL.

## Задание

Нужно реализовать сервис со следующими правилами:

- Мандаринка генерируется через равный промежуток времени или в заданное время.
- Пользователь, зайдя на сайт, видит мандаринки, но не может ни сделать ставку, ни выкупить.
- Чтобы взаимодействовать с мандаринкой, нужно зарегистрироваться или авторизоваться.
- После того как ставка пользователя перебивается, ему на почту приходит уведомление.
- После выкупа или победы в аукционе лот также приходит на почту с "чеком".
- Раз в день платформа очищается от испорченных мандаринок.

## Что реализовано

- JWT-аутентификация и регистрация пользователей.
- Роли `Admin` и `User`.
- Просмотр списка лотов и отдельного лота без авторизации.
- Ставки и выкуп только для авторизованных пользователей.
- Генерация лотов вручную через API и автоматически через Hangfire.
- Отправка писем о перебитой ставке и покупке через SMTP.
- Outbox-механизм для фоновой отправки email.
- Фоновые задачи:
  - обработка outbox;
  - завершение аукционов;
  - очистка "испорченных" лотов;
  - генерация новых мандаринок.

## Структура проекта

- `src/Vitacore.Test.Web` — ASP.NET Core API, middleware, контроллеры, Hangfire.
- `src/Vitacore.Test.Core` — доменная логика, сущности, команды и запросы.
- `src/Vitacore.Test.Infrastructure` — identity, JWT, email, обработчики инфраструктурного уровня.
- `src/Vitacore.Test.Data.Postgres` — EF Core, `DbContext`, конфигурации и миграции.
- `src/Vitacore.Test.Migrator` — отдельное консольное приложение для применения миграций.
- `src/Vitacore.Test.Contracts` — DTO и контракты запросов/ответов.

## Основные API-эндпоинты

### Аутентификация

- `POST /Authentication/register`
- `POST /Authentication/login`
- `GET /Authentication/me`

### Лоты

- `GET /Lots`
- `GET /Lots/{id}`
- `GET /Lots/{id}/bids`
- `POST /Lots/{id}/bids`
- `POST /Lots/{id}/buyout`
- `POST /Lots/generate` — только для администратора

### Фоновые задачи (Для тестов)

- `POST /Jobs/process-outbox`
- `POST /Jobs/complete-ended-auctions`
- `POST /Jobs/cleanup-expired-lots`

## Локальный запуск

### Требования

- .NET SDK 10
- PostgreSQL
- SMTP-сервер для писем или MailHog

### Запуск API

Из корня проекта:

```bash
dotnet run --project src/Vitacore.Test.Web
```

По умолчанию локальный профиль использует:

- `http://localhost:5109`
- `https://localhost:7288`

### Применение миграций

```bash
dotnet run --project src/Vitacore.Test.Migrator
```

## Docker

### Сборка web-образа

Из корня проекта:

```bash
docker buildx build --platform linux/amd64 -f Dockerfile --target web -t <dockerhub-user>/test-task-web:latest .
```

### Сборка migrator-образа

Из корня проекта:

```bash
docker buildx build --platform linux/amd64 -f Dockerfile.migrator -t <dockerhub-user>/test-task-migrator:latest .
```

### Запуск через docker compose

```bash
docker compose up -d
```

Сервисы:

- `postgres`
- `mailhog`
- `migrator`
- `web`

### Готовые Docker Hub образы

Образы уже опубликованы в Docker Hub:

- `bahlish/test-task-web:latest`
- `bahlish/test-task-migrator:latest`

Поэтому `docker compose` можно запускать без локальной сборки образов

Для сервера достаточно:

```bash
docker compose pull
docker compose up -d
```

## Демо-стенд

Приложение задеплоено и доступно по адресу:

- [bahlish.ru](https://bahlish.ru/)

Полезные страницы:

- [Swagger](https://bahlish.ru/swagger)
- [MailHog](https://bahlish.ru/mailhog)

## Конфигурация

Ключевые настройки находятся в:

- `src/Vitacore.Test.Web/appsettings.json`
- `src/Vitacore.Test.Web/appsettings.Development.json`

Основные секции:

- `Application:DbConnectionString`
- `Hangfire`
- `TangerineLotGeneration`
- `Jwt`
- `AdminUser`
- `Email`

## Поведение системы

- Анонимный пользователь может только смотреть список лотов и ставки.
- Авторизованный пользователь может делать ставки и выкупать лоты.
- Администратор может вручную генерировать лоты и запускать фоновые задачи через API.
- При перебитии ставки создаётся outbox-сообщение на отправку email.
- При выкупе или победе в аукционе пользователю отправляется письмо с чеком.

## Важные замечания

- Для "испорченных" мандаринок реализован `SoftDelete`, а не физическое удаление.
- Причина: логика обработки выкупленных и завершённых лотов после наступления `ExpirationAt` остаётся неоднозначной, а soft delete сохраняет историю торгов и упрощает дальнейшее развитие бизнес-правил.

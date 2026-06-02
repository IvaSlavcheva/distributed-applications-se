# FashionSystem — Система за наемане на дизайнерски облекла и аксесоари

**Автор:** Ива Славчева
**Факултетен номер:** 2401321046

---

## Кратко описание на проекта

FashionSystem е уеб приложение, чрез което потребителите могат да разглеждат, резервират и наемат луксозни модни артикули — рокли, обувки, чанти, часовници и аксесоари от дизайнерски марки като Gucci, Prada, Dior, Hermes, Balenciaga, Versace, Saint Laurent, Bottega Veneta, Loro Piana и Loewe.

Проектът е разделен на две отделни приложения, които работят заедно:

- **FashionSystem** — това е **Web API-то** (back-end). Тук се намира цялата бизнес логика, връзката с базата данни, JWT автентикацията и REST endpoint-ите. Изградено е на **ASP.NET Core 8 Web API**, използва **Entity Framework Core** за работа с базата и **Swagger** за документация и тестване на endpoint-ите.
- **FashionSystem.Web** — това е **MVC клиентското приложение** (front-end). То е изградено на **ASP.NET Core 9 MVC** с Razor Views и комуникира с API-то през HTTP. Това е страницата, която реалният потребител отваря в браузъра си.

Базата данни се казва `FashionSystemDb` и се създава автоматично при първото стартиране на API-то благодарение на EF Core миграциите. При първото стартиране се вкарват и начални данни (seed) — 11 модни артикула с реални снимки.

### Основни функционалности

- Регистрация и вход на потребители с **JWT токени** и хеширани пароли (BCrypt)
- Каталог с модни артикули — филтриране по дизайнер, категория, стил и размер
- Създаване на резервации (rentals) с период на наем и автоматично изчисляване на цената
- Управление на потребителския профил
- Защитени endpoint-и с роли и authorization
- Глобален exception middleware за обработка на грешки

---

## Какви приложения трябва да изтеглиш — стъпка по стъпка

За да можеш да отвориш и пуснеш проекта на твоя компютър, ще ти трябват няколко безплатни програми. Не се притеснявай, всичко е лесно — просто следвай стъпките по ред.

### 1. Visual Studio 2022 (Community Edition) — **задължително**

Това е програмата, в която ще отвориш и редактираш кода. Community версията е напълно безплатна за студенти и лично ползване.

- **Линк за изтегляне:** https://visualstudio.microsoft.com/downloads/
- Избери **Community 2022** и натисни **Free download**
- Когато инсталаторът се пусне, той ще те попита какви "workload-и" (пакети) искаш да инсталираш. Сложи отметка на следните два:
  - **ASP.NET and web development** — за да можеш да работиш с уеб приложения
  - **.NET desktop development** — полезно за общата работа с .NET
- Натисни **Install** и изчакай (инсталацията е голяма, около 5–10 GB, може да отнеме 20–40 минути в зависимост от интернета ти)

> Ако предпочиташ по-лек вариант, може да ползваш **Visual Studio Code** + .NET CLI, но Visual Studio 2022 е препоръчителен, защото проектът е създаден точно за него (има `.sln` файл).

### 2. .NET SDK 8.0 и .NET SDK 9.0 — **задължително**

Проектът се състои от две части — едната е написана на .NET 8 (API-то), а другата на .NET 9 (Web-частта). Затова трябва да са инсталирани и двете версии.

- **Линк за изтегляне:** https://dotnet.microsoft.com/en-us/download
- Изтегли **.NET 8.0 SDK** (за API-то) — натисни големия лилав бутон за версия 8
- Изтегли също и **.NET 9.0 SDK** (за Web частта) — от секцията по-долу на същата страница
- И двата SDK са обикновени `.exe` инсталатори — пускаш ги, натискаш **Install** и това е.

> Често Visual Studio 2022 идва с .NET 8 автоматично, но е добре да провериш ръчно с командата `dotnet --list-sdks` в PowerShell.

### 3. SQL Server (Express или Developer Edition) — **задължително**

Тук ще се пази базата данни на проекта. Express е безплатна и напълно достатъчна за разработка.

- **Линк за изтегляне:** https://www.microsoft.com/en-us/sql-server/sql-server-downloads
- Избери **Developer** или **Express** (и двете са безплатни)
- При инсталацията избери опцията **Basic** — тя е най-лесна и инсталира всичко по подразбиране
- В края на инсталацията ще видиш екран с името на сървъра (нещо като `localhost` или `.\SQLEXPRESS`). **Запомни го** — може да ти трябва, ако трябва да промениш connection string-а.

> В проекта connection string-ът е настроен на `Server=.;Database=FashionSystemDb;Trusted_Connection=True;` което означава локален SQL Server с Windows автентикация. Ако твоят SQL Server е инсталиран като SQLEXPRESS, ще трябва да промениш `Server=.` на `Server=.\SQLEXPRESS` в [FashionSystem/Program.cs](FashionSystem/Program.cs).

### 4. SQL Server Management Studio (SSMS) — **препоръчително**

Това не е задължително, но силно ще ти помогне да виждаш визуално какво има в базата ти, да правиш заявки, да гледаш таблици и т.н.

- **Линк за изтегляне:** https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms
- Изтегли и инсталирай — инсталацията е стандартна, без нужда от особени настройки.

### 5. Браузър (Chrome, Edge или Firefox) — **задължително**

Сигурно вече имаш такъв. Препоръчвам **Google Chrome** или **Microsoft Edge**, защото с тях Swagger UI-ят и MVC сайтът работят най-плавно.

---

## Инсталация и стартиране на проекта

След като всичко по-горе е инсталирано, следвай тези стъпки:

### Стъпка 1: Отваряне на проекта

1. Намери папката, в която е разархивиран проектът (там, където е този `README.md`).
2. Кликни два пъти върху файла **`FashionSystem.sln`** — той ще се отвори автоматично с Visual Studio 2022.
3. Изчакай Visual Studio да зареди проекта и да възстанови NuGet пакетите (виж долу вдясно дали пише *Ready*).

### Стъпка 2: Възстановяване на NuGet пакетите (ако не стане автоматично)

Понякога Visual Studio не сваля автоматично всички пакети. За да го направиш ръчно:

- Десен бутон върху **Solution 'FashionSystem'** в Solution Explorer (горе вдясно)
- Избери **Restore NuGet Packages**

Или от менюто: **Tools → NuGet Package Manager → Package Manager Console** и пиши:

```powershell
dotnet restore
```

### Стъпка 3: Настройка на базата данни

База данни не е нужно да създаваш ръчно — API-то ще го направи само при първото стартиране (`context.Database.Migrate()` в [Program.cs](FashionSystem/Program.cs)).

Но първо провери дали connection string-ът отговаря на твоя SQL Server:

- Отвори [FashionSystem/Program.cs](FashionSystem/Program.cs)
- Намери реда:
  ```csharp
  var connectionString = "Server=.;Database=FashionSystemDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;";
  ```
- Ако ползваш **SQL Server Express**, промени `Server=.` на `Server=.\SQLEXPRESS`
- Ако ползваш **LocalDB**, промени на `Server=(localdb)\MSSQLLocalDB`
- Запази файла (Ctrl+S)

### Стъпка 4: Конфигуриране на стартирането на двата проекта едновременно

Това е важно! Проектът има две приложения, които трябва да тръгнат **по едно и също време** — API-то и Web-частта.

1. Десен бутон върху **Solution 'FashionSystem'** в Solution Explorer
2. Избери **Configure Startup Projects...**
3. Маркирай **Multiple startup projects**
4. И за двата проекта — `FashionSystem` и `FashionSystem.Web` — избери **Start** в колоната Action
5. Натисни **OK**

### Стъпка 5: Стартиране

- Натисни клавиша **F5** (или зеления бутон ▶ Start горе в лентата)
- Visual Studio ще компилира и стартира двата проекта едновременно
- Ще се отворят **два прозореца в браузъра**:
  - **https://localhost:7182/swagger** — това е Swagger UI на API-то, тук виждаш всички endpoint-и
  - **https://localhost:7268** — това е сайта на FashionSystem, тук влизаш като нормален потребител

Ако браузърът ти покаже предупреждение за невалиден SSL сертификат, кликни **Advanced → Continue to localhost (unsafe)** — това е нормално за development среда.

### Стъпка 6: Тестване

- В сайта `https://localhost:7268` можеш да се регистрираш с нов потребител, да влезеш и да разгледаш каталога с дрехите
- В Swagger UI `https://localhost:7182/swagger` можеш да тестваш endpoint-ите директно — натискаш **Try it out → Execute**

---

## Често срещани проблеми

**Проблем:** Грешка `Cannot connect to server`
**Решение:** SQL Server не работи. Отвори **Services** (Win+R → `services.msc`) и провери дали `SQL Server (MSSQLSERVER)` или `SQL Server (SQLEXPRESS)` е стартиран.

**Проблем:** Грешка при миграцията `Login failed for user`
**Решение:** Connection string-ът не съвпада с твоя SQL Server. Виж Стъпка 3 по-горе.

**Проблем:** Port 7182 или 7268 е зает
**Решение:** Затвори всички Visual Studio инстанции и пробвай отново, или промени портовете в [launchSettings.json](FashionSystem/Properties/launchSettings.json) и [launchSettings.json](FashionSystem.Web/Properties/launchSettings.json).

**Проблем:** Web проектът не може да достигне API-то
**Решение:** Увери се, че и двата проекта работят едновременно (Стъпка 4). API-то трябва да е на `https://localhost:7182`.

---

## Технологии използвани в проекта

| Компонент | Технология |
|---|---|
| Back-end (API) | ASP.NET Core 8 Web API |
| Front-end (Web) | ASP.NET Core 9 MVC + Razor Views |
| База данни | SQL Server + Entity Framework Core 8 |
| Автентикация | JWT Bearer tokens + BCrypt хеширане на пароли |
| Документация на API | Swagger / Swashbuckle |
| Архитектура | Repository pattern, Dependency Injection, Middleware |

---

## Структура на проекта

```
project/
├── FashionSystem.sln              # Solution файл — отвори го с Visual Studio
├── FashionSystem/                 # Web API проект (.NET 8)
│   ├── Controllers/               # AuthController, FashionItemsController, RentalsController, UsersController
│   ├── Data/                      # AppDbContext
│   ├── Entities/                  # Модели на таблиците в базата
│   ├── Migrations/                # EF Core миграции
│   ├── Middleware/                # GlobalExceptionMiddleware
│   ├── Repository/                # Generic repository pattern
│   ├── Services/                  # Бизнес логика
│   └── Program.cs                 # Точка на влизане
└── FashionSystem.Web/             # MVC клиент (.NET 9)
    ├── Controllers/               # MVC контролери
    ├── Views/                     # Razor шаблони
    ├── Services/                  # HTTP клиенти към API-то
    └── Program.cs                 # Точка на влизане
```

---

Ако нещо не тръгне или имаш въпроси — провери първо секцията **Често срещани проблеми**. Успех!

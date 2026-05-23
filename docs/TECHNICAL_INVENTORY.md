# MultiShop Technical Inventory

Date: 2026-05-07  
Scope: `MultiShop.sln`, backend services, API gateway, IdentityServer, customer WebUI, admin/vendor/user areas, image/RapidApi/SignalR/RabbitMQ auxiliary projects.

This report is based on the project files, package references, configuration files, controllers, services, contexts, and Razor components currently present in the repository. Items that are present as packages or projects but not clearly integrated into the main marketplace flow are marked as **to be confirmed**.

## 1. Backend Technologies

| Technology | Where It Is Used | Purpose | Evidence / Modules |
|---|---|---|---|
| .NET 8 / ASP.NET Core | All main projects target `net8.0`. | Runtime and application framework for APIs, MVC UI, gateway, and auxiliary services. | `*.csproj` files across `Services`, `Frontends`, `ApiGateway`, `IdentityServer`. |
| ASP.NET Core Web API | Catalog, Basket, Discount, Order, Cargo, Comment, Message, Payment, Images, RabbitMQMessage. | Exposes REST-style endpoints for microservice operations. | `Services/*/Controllers/*Controller.cs`, `RabbitMQMessage/MultiShop.RabbitMQMessageApi/Controllers/MessageController.cs`. |
| ASP.NET Core MVC | `Frontends/MultiShop.WebUI`, `RapidApi/MultiShop.RapidApi`, `Services/Images/MultiShop.Images.WebUI`, IdentityServer UI. | Server-rendered Razor pages and panels. | `Controllers`, `Views`, Razor layouts, ViewComponents. |
| Ocelot API Gateway | `ApiGateway/MultiShop.OcelotGateway`. | Routes `/services/{service}` requests to downstream APIs and applies gateway-level JWT scope checks. | `ApiGateway/MultiShop.OcelotGateway/Ocelot.json`, `Program.cs`. |
| IdentityServer4 + ASP.NET Core Identity | `IdentityServer/MultiShop.IdentityServer`. | Central authentication, token issuing, API scopes/resources, users, roles, vendor profiles. | `Startup.cs`, `Config.cs`, `ApplicationDbContext.cs`, `RegistersController.cs`, `CustomProfileService.cs`. |
| JWT Bearer Authentication | Gateway and protected APIs. | Validates access tokens issued by IdentityServer. | `AddJwtBearer` in Catalog, Basket, Discount, Order, Cargo, Comment, Message, Ocelot. |
| Cookie Authentication | `Frontends/MultiShop.WebUI`. | Maintains browser session after login while storing access/refresh tokens in auth properties. | `Frontends/MultiShop.WebUI/Program.cs`, `IdentityService.cs`. |
| IdentityModel / AccessTokenManagement | `Frontends/MultiShop.WebUI`. | Gets client credentials tokens, password tokens, refresh tokens, and discovery metadata. | `ClientCredentialTokenService.cs`, `IdentityService.cs`, package `IdentityModel.AspNetCore`. |
| Entity Framework Core | Identity, Comment, Message, Order, Cargo, Discount migrations/context. | ORM for relational databases and migrations. | `ApplicationDbContext`, `CommentContext`, `MessageContext`, `OrderContext`, `CargoContext`, `DapperContext`. |
| Dapper | `Services/Discount/MultiShop.Discount`. | Lightweight SQL querying for coupon operations. | `DiscountService.cs`, package `Dapper`, `DapperContext.CreateConnection()`. |
| MongoDB.Driver | `Services/Catalog/MultiShop.Catalog`. | Document database access for catalog objects. | `DatabaseSettings`, catalog service classes using `IMongoCollection<T>`. |
| StackExchange.Redis | `Services/Basket/MultiShop.Basket`. | Stores user basket state. | `RedisService.cs`, `BasketService.cs`, Redis config in `appsettings.json`. |
| Npgsql / EF Core PostgreSQL | `Services/Message/MultiShop.Message`. | PostgreSQL-backed messaging data. | `MessageContext`, `UseNpgsql`, connection string `MultiShopMessageDb`. |
| SQL Server Provider | Identity, Discount, Comment, Order, Cargo. | Relational persistence. | `UseSqlServer` contexts and connection strings. |
| AutoMapper | Catalog, Comment, Message. | Maps entities to DTOs and DTOs to entities. | `Mapping/GeneralMapping.cs`, `AddAutoMapper(...)`, package refs. |
| MediatR | Order Application layer. | Mediator pattern for `Ordering` commands/queries. | `Features/Mediator`, `ServiceRegistration.cs`, `OrderingsController.cs`. |
| CQRS | Order Application layer. | Separates command and query models/handlers for Address and OrderDetail, plus MediatR-based Ordering flow. | `Services/Order/Core/MultiShop.Order.Application/Features/CQRS` and `Features/Mediator`. |
| Swagger / Swashbuckle | Most Web API projects. | API documentation and local testing. | `Swashbuckle.AspNetCore` refs and `AddSwaggerGen()`. |
| RabbitMQ.Client | `RabbitMQMessage/MultiShop.RabbitMQMessageApi`. | Demonstrates direct publish/consume with RabbitMQ. Main marketplace integration is **to be confirmed**. | `MessageController.cs`, package `RabbitMQ.Client`. |
| SignalR | `SignalRRealTime/MultiShop.SignalRRealTime`, client library present in WebUI static assets. | Real-time message/comment statistics; integration with main UI is **to be confirmed**. | `SignalRHub.cs`, SignalR services, `wwwroot/lib/microsoft/signalr`. |
| Google Cloud Storage | `Services/Images/MultiShop.Images.WebUI`. | Cloud image upload/storage UI/service. | `Google.Cloud.Storage.V1`, `CloudStorageService.cs`, `GCSConfigOptions`. |
| Newtonsoft.Json | SignalR project and some WebUI controllers. | JSON serialization/deserialization. | `SignalRRealTime` package, WebUI `RegisterController.cs`. |
| Localization | `Frontends/MultiShop.WebUI`. | View/data annotation localization for multiple cultures. | `AddLocalization`, `AddViewLocalization`, cultures `en`, `fr`, `de`, `it`, `tr`. |

## 2. Frontend Technologies

| Technology | Where It Is Used | Purpose | Examples |
|---|---|---|---|
| Razor Views | Customer WebUI, Admin, Vendor, User areas, Login/Register pages. | Server-rendered HTML templates. | `Views/Default/Index.cshtml`, `Views/LogIn/Index.cshtml`, `Areas/Vendor/Views/Product/Index.cshtml`. |
| ASP.NET Core MVC ViewComponents | Customer layout and page composition, admin/vendor/user layouts. | Reusable UI sections that can fetch data independently. | `_NavbarUILayoutComponentPartial`, `_CarouselDefaultComponentPartial`, `_ProductListProductsFilterComponentPartial`, `_VendorLayoutHeaderComponentPartial`. |
| Bootstrap | Main storefront and admin/vendor templates. | Grid, navbar, dropdowns, carousel, responsive utilities. | `multishopTemp/css/style.css`, Bootstrap 4 bundle in `_ScriptUILayoutVComponentPartial`, Ovio admin Bootstrap assets. |
| HTML/CSS | All Razor components and static themes. | Custom marketplace styling and responsive layout. | Component-level `<style>` blocks in navbar/topbar/carousel/login/sidebar; `wwwroot/multishopTemp/css/style.css`. |
| JavaScript | Bootstrap interactions, template scripts, form validation, plugins. | Dropdowns, carousel, UI controls, validation, vendor/admin template behavior. | `_ScriptUILayoutVComponentPartial`, Ovio `ovio.js`, validation scripts. |
| jQuery | Main template, admin/vendor templates, validation. | Bootstrap 4 dependency and client-side validation. | CDN jQuery in scripts, local `wwwroot/lib/jquery`, `jquery-validation`. |
| Font Awesome | Storefront, login, navbar, admin/vendor sidebar. | Icons for account, cart, favorites, menu items. | `_HeadUILayoutComponentPartial`, login page, navbar action links. |
| Owl Carousel | Storefront product/vendor carousels. | Slider/carousel interactions outside Bootstrap carousel. | `wwwroot/multishopTemp/lib/owlcarousel`, `_VendorDefaultComponentPartial`, `_ProductYouLikeComponentPartial`. |
| Animate.css | Storefront theme. | Animation helper classes. | `wwwroot/multishopTemp/lib/animate/animate.min.css`. |
| Responsive design | Customer navbar/topbar/carousel, product pages, login, admin/vendor layouts. | Mobile/tablet/desktop adaptation using Bootstrap and custom media queries. | `_TopbarUILayoutComponentPartial`, `_NavbarUILayoutComponentPartial`, `_CarouselDefaultComponentPartial`, `Views/LogIn/Index.cshtml`. |
| Admin templates | OvioAdmin and Spica assets. | Dashboard/panel styling for admin/vendor/user panels. | `wwwroot/OvioAdmin`, `wwwroot/spica`, area layouts. |

## 3. Databases and Data Storage

| Database / Storage | Service | Database / Store Name | Data Stored | Reason / Fit |
|---|---|---|---|---|
| MongoDB | Catalog Service | `MultiShopCatalogDb` | Products, categories, product images, product details, feature sliders, special offers, features, offer discounts, brands, about, contacts. | Flexible document model suits catalog entities and content modules. |
| Redis | Basket Service | Redis DB 0 at `localhost:6379` | Basket totals and basket items per authenticated user. | Fast key-value/session-like storage for volatile cart data. |
| SQL Server | IdentityServer | `MultiShopIdentityDb` | Users, roles, claims, Identity tables, vendor profiles. | Strong relational model for identities and role/profile relationships. |
| SQL Server | Discount Service | `MultiShopDiscountDb` | Coupon/discount records. | Relational coupon storage; Dapper provides lightweight queries. |
| SQL Server | Order Service | `MultiShopOrderDb` | Addresses, order details, orderings. | Transactional relational order data. |
| SQL Server | Cargo Service | `MultiShopCargoDb` | Cargo customers, companies, details, operations, vendor cargo companies. | Relational shipping and tracking data. |
| SQL Server | Comment Service | `MultiShopCommentDb` | User comments and comment status. | Relational review/comment management. |
| PostgreSQL | Message Service | `MultiShopMessageDb` | User inbox/sendbox messages. | Relational message storage; project explicitly uses Npgsql. |
| Google Cloud Storage | Images WebUI | Bucket `image_shop_bucket` | Uploaded images. | External object storage for media. |
| RabbitMQ | RabbitMQMessage API | Queue name in code; full deployment details **to be confirmed**. | Demo message content publish/consume. | Message broker pattern is present as a standalone sample/API. |

## 4. Microservices and Modules

| Module | Responsibility | Database / Storage | Communication Style | Main Controllers / Endpoints | DTOs / Services |
|---|---|---|---|---|---|
| Catalog Service | Product catalog, categories, product images/details, brand/content modules, statistics. | MongoDB `MultiShopCatalogDb`. | REST API behind Ocelot; JWT audience `ResourceCatalog`. | `ProductsController`, `CategoriesController`, `ProductImagesController`, `ProductDetailsController`, `FeatureSlidersController`, `SpecialOffersController`, `FeaturesController`, `OfferDiscountsController`, `BrandsController`, `AboutsController`, `ContactsController`, `StatisticsController`. | Catalog DTO folders; services such as `ProductService`, `CategoryService`, `StatisticService`. |
| Basket Service | Authenticated user basket read/write/delete. | Redis. | REST API behind Ocelot; all controllers require authenticated user policy. | `BasketsController`: `GET`, `POST`, `DELETE`. | `BasketTotalDto`, `BasketItemDto`, `IBasketService`. |
| Discount Service | Coupon CRUD and lookup by code. | SQL Server `MultiShopDiscountDb`. | REST API behind Ocelot; JWT audience `ResourceDiscount`. | `DiscountsController`: CRUD, `GetCodeDetailByCode`, `GetDiscountCouponCount`. | `CreateDiscountCouponDto`, `UpdateDiscountCouponDto`, `ResultDiscountCouponDto`; Dapper `IDiscountService`. |
| Order Service | Orderings, addresses, order details, vendor/order lookup. | SQL Server `MultiShopOrderDb`. | REST API behind Ocelot; protected by `[Authorize]`; uses direct CQRS handlers and MediatR. | `OrderingsController`, `AddressesController`, `OrderDetailsController`. | Commands/queries/results in `Features/CQRS` and `Features/Mediator`; repositories in `Persistance`. |
| Cargo Service | Cargo companies, customers, shipment details, operations/tracking, vendor cargo companies. | SQL Server `MultiShopCargoDb`. | REST API behind Ocelot; JWT audience `ResourceCargo`; layered Business/DataAccess/Entity/Dto projects. | `CargoCompanyController`, `CargoCustomerController`, `CargoDetailController`, `CargoOperationController`, `VendorCargoCompanyController`. | Cargo DTO layer, managers, EF DAL classes. |
| Comment Service | Product comments/reviews and comment statistics. | SQL Server `MultiShopCommentDb`. | REST API behind Ocelot; JWT audience `ResourceComment`. | `CommentsController`, `CommentStatisticsController`. | `CreateUserCommentDto`, `ResultUserCommentDto`; AutoMapper profile. |
| Message Service | User messages, inbox/sendbox, message counts. | PostgreSQL `MultiShopMessageDb`. | REST API behind Ocelot; JWT audience `ResourceMessage`. | `UserMessagesController`, duplicate `UserMessageController`. | `IUserMessageService`, message DTOs, AutoMapper. |
| IdentityServer | Authentication, users, roles, claims, token issuing, user stats. | SQL Server `MultiShopIdentityDb`. | Direct calls from WebUI for login/register/user info; token authority for APIs/gateway. | `RegistersController`, `LogInsController`, `UsersController`, `StatisticsController`; IdentityServer endpoints `/connect/token`, `/connect/userinfo`. | `ApplicationUser`, `VendorProfile`, `CustomProfileService`, client/API scope config. |
| Ocelot Gateway | Single gateway endpoint for service calls and scope validation. | No database. | Reverse proxy from WebUI to backend APIs. | Routes `/services/catalog`, `/services/discount`, `/services/order`, `/services/cargo`, `/services/basket`, `/services/comment`, `/services/message`, `/services/payment`, `/services/images`. | `Ocelot.json`, JWT scheme `OcelotAuthenticationScheme`. |
| Payment Service | Payment completion endpoint. | No database visible. | REST API; WebUI calls direct `http://localhost:7076/api/` for payment. Ocelot route exists. | `PaymentController`. | Payment DTOs in WebUI DTO layer and service DTOs. |
| Images Service | Image upload API; separate Google Cloud Storage MVC UI. | Google Cloud Storage for WebUI; API storage details **to be confirmed**. | REST API and MVC upload UI. | `GoogleDriveImageUploadController`, Images WebUI `DefaultController`. | `ICloudStorageService`, `CloudStorageService`. |
| RabbitMQMessage API | RabbitMQ publish/consume sample. | RabbitMQ broker. | Direct RabbitMQ client operations through Web API endpoints. | `MessageController`: `POST`, `GET`. | `RabbitMQ.Client`; no main integration found. |
| SignalRRealTime | Real-time hub and services for message/comment statistics. | Calls external/internal services; persistence **to be confirmed**. | SignalR hub and REST client services. | `SignalRHub`. | `SignalRMessageService`, `SignalRCommentService`. |
| RapidApi MVC | External API demo for weather, exchange, ecommerce list. | No database visible. | MVC controllers call RapidAPI endpoints. | `DefaultController`, `ECommerceController`. | View models under `RapidApi/MultiShop.RapidApi/Models`. |
| WebUI | Customer storefront, login/register, cart, order, payment, admin/vendor/user areas. | No local DB; uses services through HttpClient. | MVC controllers/ViewComponents call typed HttpClient services via gateway or IdentityServer. | Storefront controllers plus `Areas/Admin`, `Areas/Vendor`, `Areas/User`. | `Frontends/MultiShop.DtoLayer`, service interfaces/implementations, token handlers. |

## 5. Architectural Patterns

| Pattern | What It Is | Where It Appears | Why It Is Useful |
|---|---|---|---|
| Microservices Architecture | Splitting business capabilities into independent deployable services. | `Services/Catalog`, `Basket`, `Discount`, `Order`, `Cargo`, `Comment`, `Message`, `Payment`, `Images`, plus IdentityServer and gateway. | Keeps catalog, basket, order, cargo, etc. independently owned and persisted. |
| API Gateway Pattern | A gateway fronting multiple downstream APIs. | Ocelot routes in `ApiGateway/MultiShop.OcelotGateway/Ocelot.json`. | Centralizes service routing and scope-based access checks for WebUI. |
| MVC Pattern | Controller actions render views and handle form/navigation flows. | `Frontends/MultiShop.WebUI`, IdentityServer UI, RapidApi, Images WebUI. | Separates controllers, Razor views, and models/DTOs for server-rendered UI. |
| Layered Architecture | Separating API, business, data access, entity, DTO layers. | Strongly visible in Cargo; also WebUI service abstraction and Order Application/Domain/Infrastructure/Presentation layers. | Improves maintainability and keeps persistence/business logic separate from controllers. |
| Clean / Onion Architecture | Domain/Application independent of infrastructure/presentation. | Order service has `Core/Domain`, `Core/Application`, `Infrastructure/Persistance`, `Presentation/WebApi`. | Keeps order use cases and domain entities separated from EF and API controllers. |
| CQRS | Separate commands and queries with handlers/results. | Order `Features/CQRS` for Address and OrderDetail; `Features/Mediator` for Ordering. | Makes read/write operations explicit and easier to extend independently. |
| Repository Pattern | Abstracting persistence operations behind interfaces. | Order `IRepository<T>`, `Repository<T>`, `IOrderingRepository`; Cargo `IGenericDal<T>`, `GenericRepository<T>`, EF DAL classes. | Reduces controller/business dependency on EF details. |
| Dependency Injection | Container-managed services and abstractions. | All ASP.NET Core projects; service registrations in `Program.cs`. | Enables constructor injection, testing, and clear dependency ownership. |
| DTO Pattern | Separate transport objects from persistence entities. | DTO layers in Catalog, Cargo, WebUI DTO layer, Identity DTOs, Order commands/results. | Avoids exposing entities directly and shapes API/view payloads. |
| Service Abstraction Layer | Interfaces wrapping external APIs or persistence. | WebUI `IProductService`, `IBasketService`, `IOrderDetailService`; Catalog service interfaces; Cargo managers. | Keeps controllers and ViewComponents thin. |
| Area-based MVC Structure | MVC areas split by application role/domain. | `Areas/Admin`, `Areas/Vendor`, `Areas/User`. | Separates admin, vendor, and user dashboards and routes. |
| ViewComponent UI Composition | Reusable Razor component classes with views. | Layout sections, navbar, carousel, filters, product detail tabs, admin/vendor headers/sidebars. | Lets each UI section fetch data and render independently. |
| Options Pattern | Binding config sections to typed settings classes. | `DatabaseSettings`, `RedisSettings`, `ClientSettings`, `ServiceApiSettings`, `GCSConfigOptions`. | Type-safe configuration. |

## 6. Design Patterns

| Design Pattern | Where It Appears | Example Files / Classes | Purpose |
|---|---|---|---|
| Dependency Injection | Throughout APIs and WebUI. | `builder.Services.AddScoped<...>`, constructors in controllers/services. | Decouples concrete implementations from consumers. |
| Repository Pattern | Order and Cargo. | `Repository<T>`, `OrderingRepository`, `GenericRepository<T>`, `EfCargoCompanyDal`. | Encapsulates database CRUD and query logic. |
| Generic Repository | Cargo and Order. | `IGenericDal<T>`, `GenericRepository<T>`, `IRepository<T>`, `Repository<T>`. | Reuses common CRUD operations for multiple entities. |
| Mediator Pattern | Order Ordering feature. | `IMediator`, `IRequest<T>`, `IRequestHandler<T>`, `OrderingsController`. | Dispatches requests to handlers without direct controller-handler coupling. |
| CQRS Handler Pattern | Order Address and OrderDetail features. | `CreateAddressCommandHandler`, `GetOrderDetailQueryHandler`. | Separates command/query handlers and result models. |
| Adapter / Service Wrapper | WebUI typed HttpClient services. | `ProductService`, `BasketService`, `OrderDetailService`, `CargoDetailService`, `PaymentService`. | Wraps remote API calls in application-friendly methods. |
| Delegating Handler | WebUI outbound HTTP pipeline. | `ClientCredentialTokenHandler`, `ResourceOwnerPasswordTokenHandler`. | Adds/refreshes Bearer tokens automatically on API calls. |
| DTO Mapping | Catalog, Comment, Message. | `GeneralMapping.cs`, AutoMapper registrations. | Converts between entities and API DTOs. |
| Component-based UI | WebUI, Admin, Vendor, User layouts. | `_NavbarUILayoutComponentPartial`, `_CarouselDefaultComponentPartial`, `_VendorLayoutSideBarComponentPartial`. | Reusable, composable UI building blocks. |
| Options Pattern | Config-bound settings. | `ServiceApiSettings`, `ClientSettings`, `DatabaseSettings`, `RedisSettings`. | Avoids hard-coded config in consumers. |
| Unit of Work | Not clearly found. | **To be confirmed**. | EF `DbContext` provides implicit unit-of-work behavior, but no explicit `IUnitOfWork` was found. |
| Factory Pattern | Not clearly found as a custom pattern. | `IHttpClientFactory` used in WebUI register flow. | ASP.NET Core factory creates HttpClient instances; no custom factory layer found. |

## 7. Backend-Frontend Communication

The WebUI communicates with backend services primarily through typed `HttpClient` services registered in `Frontends/MultiShop.WebUI/Program.cs`.

### Flow

1. MVC controller or ViewComponent receives a request.
2. Controller/ViewComponent calls an injected service interface, for example `IProductService`.
3. The service implementation uses `HttpClient` and DTOs from `Frontends/MultiShop.DtoLayer`.
4. `HttpClient` base address points either to Ocelot (`http://localhost:5000/services/...`) or directly to a service/IdentityServer where configured.
5. A `DelegatingHandler` adds the correct Bearer token:
   - `ClientCredentialTokenHandler` for visitor/client credentials calls.
   - `ResourceOwnerPasswordTokenHandler` for authenticated user calls and token refresh.
6. Ocelot routes the request to the downstream microservice and checks scopes.
7. The service returns JSON DTOs that the WebUI renders in Razor views.

### Examples

| WebUI Service | Backend Target | Purpose |
|---|---|---|
| `ProductService` | `services/catalog/products` | Product list, detail, vendor products, products with category. |
| `CategoryService` / `OfferService` | `services/catalog/categories` | Navbar/category lists and admin category management. |
| `FeatureSliderService` | `services/catalog/featuresliders` | Homepage hero carousel. |
| `BasketService` | `services/basket/baskets` | Basket CRUD and item manipulation. |
| `DiscountService` | `services/discount/discounts` | Coupon confirmation and discount details. |
| `OrderAddressService`, `OrderOrderingService`, `OrderDetailService` | `services/order/...` | Checkout address, ordering creation, order detail/vendor order views. |
| `CargoCompanyService`, `CargoDetailService`, `CargoOperationService` | Ocelot Cargo route and direct `http://localhost:7073/api/` in some registrations. | Vendor shipment creation, status updates, cargo tracking. |
| `MessageService` | `services/message/usermessages` | User/admin message inbox/sendbox and counts. |
| `CommentService` | `services/comment/comments` | Product reviews and comment statistics. |
| `IdentityService` | `http://localhost:5001` IdentityServer discovery/token/userinfo endpoints. | Login, access token storage, refresh token flow. |
| `UserService` / `UserIdentityService` | IdentityServer local API. | Current user information and user statistics. |

## 8. Authentication and Authorization

| Topic | Implementation |
|---|---|
| Identity provider | IdentityServer4 in `IdentityServer/MultiShop.IdentityServer`, backed by ASP.NET Core Identity and SQL Server. |
| User model | `ApplicationUser : IdentityUser` with `Name` and `Surname`; `VendorProfile` extends vendor account data. |
| Roles | Register flow creates and assigns `Customer` or `Vendor`; WebUI login logic also checks `Admin` if present. |
| Claims | Register flow adds name, given name, family name, email, and role claims. `CustomProfileService` also emits role claims into tokens. |
| Clients | `MultiShopVisitorId` uses client credentials; `MultiShopManagerId` and `MultiShopAdminId` use resource owner password flow. |
| API resources/scopes | `ResourceCatalog`, `ResourceBasket`, `ResourceDiscount`, `ResourceOrder`, `ResourceCargo`, `ResourceComment`, `ResourcePayment`, `ResourceImage`, `ResourceMessage`, `ResourceOcelot`. |
| WebUI session | Cookie auth named `MultiShopCookie`; tokens stored in authentication properties. |
| Token refresh | `ResourceOwnerPasswordTokenHandler` refreshes access tokens when a service responds 401. |
| Gateway authorization | Ocelot validates JWT with `OcelotAuthenticationScheme` and per-route allowed scopes. |
| API authorization | Basket applies a global authenticated-user policy; Order controllers use `[Authorize]`; other APIs configure JWT Bearer and `UseAuthorization`. |
| Role-based UI | Navbar uses `User.Identity.IsAuthenticated` and `User.IsInRole("Customer")` / `User.IsInRole("Vendor")` for account dropdown behavior. |
| Customer/vendor separation | Vendor MVC area exists under `Areas/Vendor`; customer/user area exists under `Areas/User`; role registration and navigation distinguish users. Controller-level `[Authorize(Roles=...)]` is not consistently visible and should be reviewed for production hardening. |

## 9. UI Structure

| Area / Page Group | Structure and Purpose |
|---|---|
| Main customer layout | `Views/UILayout/_UILayout.cshtml` composes head, topbar, navbar, body, footer, scripts using ViewComponents. |
| Topbar | `_TopbarUILayoutComponentPartial` renders compact black announcement and utility links. |
| Navbar | `_NavbarUILayoutComponentPartial` renders category navigation, integrated search, auth-aware account dropdown, favorites and basket actions. |
| Homepage | `Views/Default/Index.cshtml` uses components such as carousel, categories, features, offers, featured products, vendors. |
| Hero carousel | `_CarouselDefaultComponentPartial` uses Bootstrap carousel with data from FeatureSlider service. |
| Product list | `Views/ProductList/Index.cshtml`, `IndexAll.cshtml` combine filter ViewComponents and product list/pagination components. |
| Product detail | `Views/ProductList/ProductDetail.cshtml` composes image slider, feature, description, information, review, recommendations. |
| Shopping cart | `Views/ShoppingCart/Index.cshtml` uses cart product list, checkout summary, optional discount coupon component. |
| Order/checkout | `Views/Order/Index.cshtml` uses address/detail/payment components. |
| Payment | `Views/Payment/Index.cshtml`, `Success.cshtml`, `Fail.cshtml`. |
| Login/register | `Views/LogIn/Index.cshtml`, `Views/Register/CustomerRegister.cshtml`, `VendorRegister.cshtml`. |
| My profile/orders | `Views/MyProfile/MyOrders.cshtml`, `OrderDetail.cshtml`; `Areas/User/Views/MyOrder`. |
| User area | `Areas/User/Views/UserLayout/Index.cshtml` with sidebar/navbar/footer ViewComponents, message pages, order pages. |
| Vendor area | `Areas/Vendor/Views/VendorLayout/Index.cshtml` with vendor header/sidebar/main-section ViewComponents; product, offer, comment, cargo, order pages. |
| Admin area | `Areas/Admin/Views/AdminLayout/Index.cshtml` with admin header/sidebar/main-section ViewComponents; catalog/content/cargo/comment/statistic/user management pages. |

## 10. Summary Tables

### Backend Technologies

| Category | Technologies |
|---|---|
| Framework | .NET 8, ASP.NET Core Web API, ASP.NET Core MVC |
| Auth | IdentityServer4, ASP.NET Core Identity, JWT Bearer, Cookie Authentication, IdentityModel |
| Gateway | Ocelot |
| Persistence | EF Core, Dapper, MongoDB.Driver, StackExchange.Redis, Npgsql |
| Patterns/mediators | CQRS, MediatR, Repository, DI |
| API tooling | Swagger / Swashbuckle |
| Messaging / real-time | RabbitMQ.Client, SignalR (**main integration to be confirmed**) |
| Cloud | Google Cloud Storage |

### Frontend Technologies

| Category | Technologies |
|---|---|
| Rendering | Razor Views, MVC Areas, ViewComponents |
| UI framework | Bootstrap, custom CSS |
| JS | JavaScript, jQuery, Bootstrap JS |
| Icons/fonts | Font Awesome, Google Fonts |
| Plugins/templates | Owl Carousel, Animate.css, OvioAdmin, Spica |
| Validation | jQuery Validation, unobtrusive validation |
| Responsive design | Bootstrap grid/utilities and component media queries |

### Databases

| Service | Database Technology | Name |
|---|---|---|
| Catalog | MongoDB | `MultiShopCatalogDb` |
| Basket | Redis | DB 0 at `localhost:6379` |
| Identity | SQL Server | `MultiShopIdentityDb` |
| Discount | SQL Server | `MultiShopDiscountDb` |
| Order | SQL Server | `MultiShopOrderDb` |
| Cargo | SQL Server | `MultiShopCargoDb` |
| Comment | SQL Server | `MultiShopCommentDb` |
| Message | PostgreSQL | `MultiShopMessageDb` |
| Images WebUI | Google Cloud Storage | `image_shop_bucket` |

### Microservices

| Service | Port / Route Evidence | Notes |
|---|---|---|
| Catalog | Ocelot downstream `localhost:7070`, upstream `/services/catalog/{everything}` | MongoDB catalog/content service. |
| Discount | `localhost:7071`, `/services/discount/{everything}` | SQL Server + Dapper coupon service. |
| Order | `localhost:7072`, `/services/order/{everything}` | SQL Server + CQRS/MediatR. |
| Cargo | `localhost:7073`, `/services/cargo/{everything}` | Layered SQL Server cargo service. |
| Basket | `localhost:7074`, `/services/basket/{everything}` | Redis basket service. |
| Comment | `localhost:7075`, `/services/comment/{everything}` | SQL Server comment service. |
| Payment | `localhost:7076`, `/services/payment/{everything}` | Payment endpoint, persistence not visible. |
| Images | `localhost:7077`, `/services/images/{everything}` | API present; image storage details to be confirmed. |
| Message | `localhost:7078`, `/services/message/{everything}` | PostgreSQL message service. |
| IdentityServer | `localhost:5001` | Authority/token/user registration service. |
| Ocelot Gateway | `localhost:5000` | Front door for WebUI service calls. |

### Architecture Patterns

| Pattern | Status |
|---|---|
| Microservices | Confirmed |
| API Gateway | Confirmed |
| MVC | Confirmed |
| Layered Architecture | Confirmed |
| Clean/Onion Architecture | Confirmed in Order service |
| CQRS | Confirmed in Order service |
| Repository | Confirmed in Order and Cargo |
| DTO Pattern | Confirmed |
| ViewComponent Composition | Confirmed |
| Options Pattern | Confirmed |

### Design Patterns

| Pattern | Status / Examples |
|---|---|
| Dependency Injection | Confirmed across services |
| Repository / Generic Repository | Confirmed: Order, Cargo |
| Mediator | Confirmed: Order Ordering |
| CQRS Handlers | Confirmed: Order Address/OrderDetail/Ordering |
| Adapter/Service Wrapper | Confirmed: WebUI typed HttpClient services |
| Delegating Handler | Confirmed: token handlers |
| DTO Mapping | Confirmed: AutoMapper and DTO layers |
| Unit of Work | To be confirmed; no explicit implementation found |
| Custom Factory | To be confirmed; `IHttpClientFactory` exists, no custom factory layer found |

### External Libraries / Packages

| Package / Library | Used By |
|---|---|
| Ocelot | API Gateway |
| Microsoft.AspNetCore.Authentication.JwtBearer | APIs and gateway |
| IdentityServer4.AspNetIdentity | IdentityServer |
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | IdentityServer |
| IdentityModel.AspNetCore | WebUI token management |
| EntityFrameworkCore.SqlServer | Identity, Discount, Order, Cargo, Comment |
| Npgsql.EntityFrameworkCore.PostgreSQL | Message |
| Dapper | Discount |
| MongoDB.Driver / MongoDB.Bson | Catalog |
| StackExchange.Redis | Basket |
| AutoMapper | Catalog, Comment, Message |
| MediatR | Order Application |
| Swashbuckle.AspNetCore | APIs |
| RabbitMQ.Client | RabbitMQMessage API |
| Newtonsoft.Json | SignalR and selected WebUI flows |
| Google.Cloud.Storage.V1 | Images WebUI |
| Bootstrap | WebUI/admin/vendor templates |
| jQuery | WebUI/admin/vendor templates and validation |
| Font Awesome | WebUI/admin/vendor icons |
| Owl Carousel | Storefront carousel components |
| Animate.css | Storefront template animations |

## Findings and Confirmation Notes

- RabbitMQ is present as a separate API project and package, but no clear main e-commerce workflow integration was found.
- SignalR is present with a hub and services, but the active WebUI integration points should be confirmed.
- Payment service exposes a controller and is called from WebUI; no database or real payment provider integration is visible.
- Images API exists, and a separate Images WebUI integrates Google Cloud Storage. The exact relationship between the Images API and GCS WebUI should be confirmed.
- Some APIs configure JWT authentication but do not consistently apply `[Authorize]` or global authorization filters. Basket and Order are visibly protected; other services should be reviewed for production authorization consistency.
- Some connection strings are hard-coded in `DbContext.OnConfiguring`; production should move these fully into configuration/secrets.

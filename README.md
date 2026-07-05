# SalesFlow CRM

## Proje Hakkında

SalesFlow, müşteri ilişkileri ve satış süreçlerini yönetmek amacıyla geliştirilen modern bir CRM (Customer Relationship Management) API projesidir.

Proje; potansiyel müşterilerin sisteme kazandırılması, satış fırsatlarının yönetilmesi, toplantıların planlanması, görevlerin takip edilmesi ve müşteriyle ilgili tüm süreçlerin tek bir sistem üzerinden yönetilebilmesini hedeflemektedir.

SalesFlow geliştirilirken yalnızca temel CRUD işlemlerine odaklanılmamış, gerçek bir CRM uygulamasında karşılaşılabilecek iş akışları, iş kuralları ve sürdürülebilir yazılım mimarisi esas alınmıştır.

Proje aktif olarak geliştirilmeye devam etmekte olup yeni iş akışları, raporlama özellikleri ve React tabanlı yönetim paneli ile genişletilecektir.

---

# Amaç

SalesFlow'u geliştirirken temel hedefim;

* Gerçek bir iş senaryosunu temel alan bir CRM altyapısı oluşturmak,
* Modern .NET teknolojilerini tek bir projede bir araya getirmek,
* Genişletilebilir, sürdürülebilir ve okunabilir bir backend mimarisi geliştirmek,
* Gerçek hayatta kullanılabilecek bir proje ortaya koymaktır.

---

# Kullanılan Teknolojiler

* ASP.NET Core 10
* Entity Framework Core
* SQL Server
* ASP.NET Core Identity
* JWT Authentication
* Refresh Token
* FluentValidation
* Mapster
* Scalar OpenAPI

---

# Kullanılan Mimari ve Tasarım Yaklaşımları

* N Katmanlı Mimari
* Repository Pattern
* Generic Repository
* Unit of Work
* Result Pattern
* Dependency Injection
* FluentValidation
* Global Exception Middleware
* Fluent API
* Soft Delete
* Audit Interceptor
* Global Query Filter
* Pagination

---

# Proje Yapısı

```
SalesFlow.API
│
├── Controllers
├── Middlewares
└── Program.cs

SalesFlow.Business
│
├── DTOs
├── Services
├── BusinessRules
├── Validations
├── Extensions
└── Jwt

SalesFlow.DataAccess
│
├── Configurations
├── Context
├── Interceptors
├── Repositories
├── UnitOfWork
└── Extensions

SalesFlow.Entity
│
├── Common
├── Entities
└── Enums

SalesFlow.Core
│
├── Exceptions
├── Pagination
└── Results
```

---

# Tamamlanan Modüller

### Customer Management

* Müşteri yönetimi
* Soft Delete
* Pagination
* Validation
* Business Rules

### Lead Management

* Potansiyel müşteri yönetimi
* Lead Status
* Lead Source
* Validation
* Business Rules

### Deal Management

* Satış fırsatı yönetimi
* Deal Stage yönetimi
* Durum geçiş kuralları

### Meeting Management

* Toplantı planlama
* Toplantı durumu yönetimi
* Çakışma kontrolü

### Task Management

* Görev yönetimi
* Öncelik yönetimi
* Durum yönetimi

### Note Management

* Müşteri notları

### Attachment Management

* Dosya kayıt yönetimi

### Tag Management

* Etiket oluşturma
* Müşterilere etiket atama

### Authentication & Authorization

* ASP.NET Core Identity
* JWT Authentication
* Refresh Token
* Role Based Authorization

---

# Altyapı Özellikleri

## Result Pattern

Tüm servisler standart Result yapısı kullanmaktadır.

```json
{
  "isSuccess": true,
  "message": "Operation completed successfully.",
  "data": {}
}
```

---

## Global Exception Middleware

Merkezi hata yönetimi uygulanmaktadır.

Yönetilen hata tipleri;

* ValidationException
* BusinessException
* NotFoundException
* Internal Server Error

---

## FluentValidation

DTO doğrulamaları FluentValidation ile gerçekleştirilmektedir.

---

## Soft Delete

Silinen kayıtlar fiziksel olarak silinmez.

Global Query Filter sayesinde normal sorgulara dahil edilmez.

---

## Audit Interceptor

Aşağıdaki alanlar otomatik olarak yönetilmektedir.

* CreatedDate
* UpdatedDate
* IsDeleted

---

# Entity Yapısı

* Customer
* Lead
* Deal
* Meeting
* TaskItem
* Note
* Attachment
* Tag
* CustomerTag
* AppUser
* AppRole

---

# Geliştirme Yol Haritası

## Tamamlanan

* N Katmanlı Mimari
* Repository & Unit of Work
* Result Pattern
* FluentValidation
* Global Exception Middleware
* Soft Delete
* Audit Interceptor
* Global Query Filter
* Pagination
* Customer Management
* Lead Management
* Deal Management
* Meeting Management
* Task Management
* Note Management
* Attachment Management
* Tag Management
* ASP.NET Core Identity
* JWT Authentication
* Refresh Token
* Role Based Authorization

## Planlanan

* Lead → Customer Conversion
* Dashboard API
* Dashboard Raporları
* React Admin Panel
* Dosya Yükleme (IFormFile)
* E-posta Bildirimleri
* SignalR Bildirimleri
* Logging
* Unit Test
* Integration Test
* Docker
* CI/CD

---

# Durum

SalesFlow aktif olarak geliştirilmeye devam etmektedir.

Mevcut aşamada CRM'in temel modülleri ve kimlik doğrulama altyapısı tamamlanmıştır.

Bir sonraki geliştirme aşamasında satış süreçlerinin gerçek iş akışları (Lead → Customer Conversion), dashboard servisleri ve React tabanlı yönetim paneli geliştirilecektir.

---

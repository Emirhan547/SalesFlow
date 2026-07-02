# SalesFlow CRM

SalesFlow, müşteri ilişkileri ve satış süreçlerini yönetmek amacıyla geliştirilen bir CRM (Customer Relationship Management) API projesidir.

Projede potansiyel müşterilerin (Lead) sisteme eklenmesi, yönetilmesi ve müşteriye (Customer) dönüştürülmesiyle başlayan satış sürecinin tek bir sistem üzerinden takip edilmesi hedeflenmektedir.

Bu proje yalnızca CRUD işlemlerini gerçekleştirmek amacıyla geliştirilmemektedir. Gerçek bir CRM uygulamasında ihtiyaç duyulabilecek modüller ve iş kuralları dikkate alınarak katmanlı bir backend altyapısı oluşturulmaktadır.

---

# Neden Bu Proje?

Daha önce geliştirdiğim projelerde farklı teknolojileri ve mimari yaklaşımları ayrı ayrı kullanma fırsatı buldum. SalesFlow'u ise bunları gerçek bir iş senaryosu içerisinde bir araya getirmek amacıyla geliştirmeye başladım.

Amacım yalnızca çalışan bir API geliştirmek değil; geliştirilebilir, sürdürülebilir ve yeni özelliklerin mevcut yapıyı bozmadan eklenebileceği bir CRM altyapısı oluşturmak.

Proje aktif olarak geliştirilmeye devam etmekte olup yeni modüller ve iş kuralları eklendikçe genişletilecektir.

---

# Kullanılan Teknolojiler

- ASP.NET Core 10
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- FluentValidation
- Mapster
- Scalar OpenAPI

---

# Mimari

Projede aşağıdaki mimari yaklaşımlar kullanılmaktadır.

- N Katmanlı Mimari
- Repository Pattern
- Generic Repository
- Unit of Work Pattern
- Result Pattern
- Global Exception Middleware
- Fluent API
- Soft Delete
- Audit Interceptor
- Global Query Filter
- Dependency Injection

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
└── Extensions

SalesFlow.DataAccess
│
├── Configurations
├── Context
├── Interceptors
├── Repositories
└── UnitOfWork

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

## Customer Management

- Customer CRUD işlemleri
- Sayfalama (Pagination)
- FluentValidation
- Business Rules
- Email benzersizlik kontrolü
- Soft Delete desteği

---

## Lead Management

- Lead CRUD işlemleri
- Lead Status yönetimi
- Lead Source yönetimi
- Sayfalama (Pagination)
- FluentValidation
- Business Rules
- Email benzersizlik kontrolü

---

# Altyapı Özellikleri

### Result Pattern

Tüm servisler standart bir Result yapısı kullanmaktadır.

```json
{
  "isSuccess": true,
  "message": "Operation completed successfully.",
  "data": { }
}
```

---

### Global Exception Middleware

Uygulama genelindeki exception yönetimi tek noktadan gerçekleştirilmektedir.

Yönetilen hata tipleri:

- ValidationException
- BusinessException
- NotFoundException
- Internal Server Error

---

### FluentValidation

Tüm DTO doğrulamaları FluentValidation ile gerçekleştirilmektedir.

---

### Soft Delete

Silinen kayıtlar fiziksel olarak veritabanından kaldırılmaz.

Global Query Filter sayesinde silinen kayıtlar normal sorgulara dahil edilmez.

---

### Audit Interceptor

Aşağıdaki alanlar Entity Framework Core Interceptor yapısı ile otomatik olarak yönetilmektedir.

- CreatedDate
- UpdatedDate
- IsDeleted

---

# Mevcut Entity'ler

- Customer
- Lead
- Deal
- Meeting
- Note
- TaskItem
- Attachment
- Tag
- CustomerTag
- AppUser
- AppRole

---

# Yol Haritası

## ✅ Faz 1 — Temel Altyapı

- [x] N Katmanlı Mimari
- [x] Entity Framework Core
- [x] SQL Server
- [x] Generic Repository
- [x] Unit of Work
- [x] Result Pattern
- [x] Global Exception Middleware
- [x] FluentValidation
- [x] Fluent API
- [x] Soft Delete
- [x] Audit Interceptor
- [x] Global Query Filter
- [x] Pagination

---

## ✅ Faz 2 — Customer Management

- [x] Customer CRUD
- [x] DTO yapıları
- [x] Business Rules
- [x] Validation
- [x] Pagination

---

## ✅ Faz 3 — Lead Management

- [x] Lead CRUD
- [x] Lead Status
- [x] Lead Source
- [x] DTO yapıları
- [x] Business Rules
- [x] Validation
- [x] Pagination

---

##  Faz 4 — Deal Management

- [ ] Deal CRUD
- [ ] Deal Stage yönetimi
- [ ] Lead → Customer dönüşümü
- [ ] Satış süreci yönetimi
- [ ] Satış geçmişi

---

##  Faz 5 — Meeting & Task Management

- [ ] Meeting yönetimi
- [ ] Task yönetimi
- [ ] Note yönetimi
- [ ] Attachment yönetimi
- [ ] Tag sistemi

---

##  Faz 6 — Authentication & Authorization

- [ ] ASP.NET Core Identity
- [ ] JWT Authentication
- [ ] Refresh Token
- [ ] Role Management
- [ ] Role Based Authorization

---

##  Faz 7 — Dashboard

- [ ] Dashboard istatistikleri
- [ ] Lead raporları
- [ ] Customer raporları
- [ ] Deal raporları
- [ ] Grafikler

---

##  Faz 8 — React Admin Panel

- [ ] Authentication
- [ ] Dashboard
- [ ] Customer Management
- [ ] Lead Management
- [ ] Deal Management
- [ ] Meeting Management
- [ ] Task Management

---

##  Faz 9 — Son Dokunuşlar

- [ ] Unit Test
- [ ] Integration Test
- [ ] Logging
- [ ] Docker
- [ ] CI/CD
- [ ] API Dokümantasyonu

---

# Durum

 Proje aktif olarak geliştirilmektedir.

Customer ve Lead modülleri tamamlanmıştır.

Bir sonraki geliştirme aşaması Deal Management modülü ve kimlik doğrulama (Identity/JWT) altyapısı olacaktır.

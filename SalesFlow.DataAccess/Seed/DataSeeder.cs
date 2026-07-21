using Microsoft.AspNetCore.Identity;
using SalesFlow.DataAccess.Context;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums;
using Microsoft.EntityFrameworkCore;
using TaskStatus = SalesFlow.Entity.Enums.TaskStatus;

namespace SalesFlow.DataAccess.Seed
{
    public class DataSeeder
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly Random _random = new Random();
        private List<AppUser> _users = new();
        private List<Customer> _customers = new();
        private List<Lead> _leads = new();
        private List<Tag> _tags = new();
        private List<Deal> _deals = new();
        private List<Meeting> _meetings = new();
        private List<TaskItem> _tasks = new();
        private List<Note> _notes = new();
        private List<ActivityLog> _activityLogs = new();
        private List<Attachment> _attachments = new();
        private List<Notification> _notifications = new();

        public DataSeeder(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Check if data already exists
            //if (await _context.Users.AnyAsync())
            //    return;

            await CreateRolesAsync();
            await _context.SaveChangesAsync();
            await CreateUsersAsync();
            await _context.SaveChangesAsync();
            await CreateTagsAsync();
            await _context.SaveChangesAsync();
            await CreateLeadsAsync();
            await _context.SaveChangesAsync();
            await CreateCustomersAsync();
            await _context.SaveChangesAsync();
            await CreateCustomerTagsAsync();
            await _context.SaveChangesAsync();
            await CreateDealsAsync();
            await _context.SaveChangesAsync();
            await CreateMeetingsAsync();
            await _context.SaveChangesAsync();
            await CreateTasksAsync();
            await _context.SaveChangesAsync();
            await CreateNotesAsync();
            await _context.SaveChangesAsync();
            await CreateActivityLogsAsync();
            await _context.SaveChangesAsync();
            await CreateAttachmentsAsync();
            await _context.SaveChangesAsync();
            await CreateNotificationsAsync();

            await _context.SaveChangesAsync();
        }

        #region Roles
        private async Task CreateRolesAsync()
        {
            var roles = new[] { "Admin", "Sales Manager", "Sales Representative" };
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new AppRole { Name = role });
                }
            }
        }
        #endregion

        #region Users
        private async Task CreateUsersAsync()
        {
            var users = new[]
            {
        new { FirstName = "Mehmet", LastName = "Demir", Email = "mehmet.demir@salesflow.com", Role = "Admin", Phone = "+90 532 111 11 11" },
        new { FirstName = "Ayşe", LastName = "Yılmaz", Email = "ayse.yilmaz@salesflow.com", Role = "Sales Manager", Phone = "+90 533 222 22 22" },
        new { FirstName = "Mustafa", LastName = "Kaya", Email = "mustafa.kaya@salesflow.com", Role = "Sales Representative", Phone = "+90 535 333 33 33" },
        new { FirstName = "Zeynep", LastName = "Çelik", Email = "zeynep.celik@salesflow.com", Role = "Sales Representative", Phone = "+90 536 444 44 44" },
        new { FirstName = "Ali", LastName = "Şahin", Email = "ali.sahin@salesflow.com", Role = "Sales Representative", Phone = "+90 537 555 55 55" }
    };

            foreach (var userData in users)
            {
                // Kullanıcı zaten var mı?
                var existingUser = await _userManager.FindByEmailAsync(userData.Email);

                if (existingUser != null)
                {
                    // Listeye ekle ki diğer seed işlemleri kullanabilsin
                    _users.Add(existingUser);

                    // Rolü yoksa ekle
                    if (!await _userManager.IsInRoleAsync(existingUser, userData.Role))
                    {
                        await _userManager.AddToRoleAsync(existingUser, userData.Role);
                    }

                    continue;
                }

                var user = new AppUser
                {
                    UserName = userData.Email,
                    Email = userData.Email,
                    FirstName = userData.FirstName,
                    LastName = userData.LastName,
                    PhoneNumber = userData.Phone,
                    IsActive = true,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, "SalesFlow.2026!");

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"{error.Code} - {error.Description}");
                    }

                    continue;
                }

                await _userManager.AddToRoleAsync(user, userData.Role);

                _users.Add(user);
            }
        }
        #endregion

        #region Tags
        private async Task CreateTagsAsync()
        {
            if (await _context.Tags.AnyAsync())
                return;

            var tagData = new[]
            {
        new { Name = "VIP", Color = "#FF6B6B" },
        new { Name = "Enterprise", Color = "#4ECDC4" },
        new { Name = "Manufacturing", Color = "#45B7D1" },
        new { Name = "Logistics", Color = "#96CEB4" },
        new { Name = "Renewable Energy", Color = "#FFEAA7" },
        new { Name = "Healthcare", Color = "#DDA0DD" },
        new { Name = "High Value", Color = "#FF8A5C" },
        new { Name = "Existing Customer", Color = "#A8E6CF" },
        new { Name = "Needs Follow-up", Color = "#FFD93D" },
        new { Name = "Decision Maker", Color = "#6C5B7B" },
        new { Name = "Hot Lead", Color = "#FF6B6B" },
        new { Name = "SMB", Color = "#88D8B0" }
    };

            foreach (var tag in tagData)
            {
                _tags.Add(new Tag
                {
                    Name = tag.Name,
                    Color = tag.Color
                });
            }

            await _context.Tags.AddRangeAsync(_tags);
        }
        #endregion

        #region Leads
        private async Task CreateLeadsAsync()
        {
            var leadData = new[]
            {
                new { FirstName = "Ahmet", LastName = "Yıldız", Company = "İstanbul Teknoloji A.Ş.", Email = "ahmet.yildiz@istanbultech.com", Phone = "+90 212 555 11 11", Website = "www.istanbultech.com", Address = "Maslak, İstanbul", Status = LeadStatus.Qualified, Source = LeadSource.Website, Description = "ERP sistemlerine ihtiyaç duyan orta ölçekli teknoloji şirketi. Mevcut altyapıları manuel süreçlerle yönetiliyor." },
                new { FirstName = "Elif", LastName = "Demirtaş", Company = "Ankara Sağlık Çözümleri", Email = "elif.demirtas@ankarasaglik.com", Phone = "+90 312 555 22 22", Website = "www.ankarasaglik.com", Address = "Çankaya, Ankara", Status = LeadStatus.Converted, Source = LeadSource.Referral, Description = "Hastane yönetim sistemleri konusunda danışmanlık almak isteyen özel hastane zinciri. Dijital dönüşüm sürecinde." },
                new { FirstName = "Murat", LastName = "Kurt", Company = "Ege Lojistik", Email = "murat.kurt@egelojistik.com", Phone = "+90 232 555 33 33", Website = "www.egelojistik.com", Address = "Bornova, İzmir", Status = LeadStatus.Contacted, Source = LeadSource.Phone, Description = "Filo yönetimi ve rota optimizasyonu konularında çözüm arayan lojistik şirketi." },
                new { FirstName = "Selin", LastName = "Özdemir", Company = "Güneş Enerji Sistemleri", Email = "selin.ozdemir@guneSenerji.com", Phone = "+90 242 555 44 44", Website = "www.guneSenerji.com", Address = "Konyaaltı, Antalya", Status = LeadStatus.New, Source = LeadSource.SocialMedia, Description = "Güneş enerjisi santrali kurulumu için finansman ve teknoloji danışmanlığı arayan yatırımcı." },
                new { FirstName = "Kemal", LastName = "Aydın", Company = "Marmara Gıda A.Ş.", Email = "kemal.aydin@marmaragida.com", Phone = "+90 216 555 55 55", Website = "www.marmaragida.com", Address = "Ümraniye, İstanbul", Status = LeadStatus.Qualified, Source = LeadSource.Email, Description = "Üretim süreçlerini optimize etmek ve tedarik zinciri yönetimini dijitalleştirmek isteyen gıda şirketi." },
                new { FirstName = "Deniz", LastName = "Erdoğan", Company = "Karadeniz Madencilik", Email = "deniz.erdogan@karadenizmaden.com", Phone = "+90 362 555 66 66", Website = "www.karadenizmaden.com", Address = "Samsun, Merkez", Status = LeadStatus.Lost, Source = LeadSource.Other, Description = "Maden sahalarında iş güvenliği ve varlık takibi için IoT çözümleri araştırıyor. Bütçe uyuşmazlığı nedeniyle görüşmeler durduruldu." },
                new { FirstName = "Zeynep", LastName = "Kara", Company = "Akdeniz Tekstil", Email = "zeynep.kara@akdeniztekstil.com", Phone = "+90 326 555 77 77", Website = "www.akdeniztekstil.com", Address = "Antakya, Hatay", Status = LeadStatus.Converted, Source = LeadSource.Website, Description = "Tekstil üretiminde verimliliği artırmak ve stok yönetimini iyileştirmek isteyen orta ölçekli firma." },
                new { FirstName = "Yusuf", LastName = "Şimşek", Company = "Doğu Otomotiv", Email = "yusuf.simsek@doguotomotiv.com", Phone = "+90 412 555 88 88", Website = "www.doguotomotiv.com", Address = "Bağcılar, Diyarbakır", Status = LeadStatus.Contacted, Source = LeadSource.Referral, Description = "Araç satış ve servis süreçlerini dijitalleştirmek isteyen otomotiv firması. CRM sistemi arıyor." },
                new { FirstName = "Fatma", LastName = "Yılmaz", Company = "Ege Tarım", Email = "fatma.yilmaz@egetarim.com", Phone = "+90 252 555 99 99", Website = "www.egetarim.com", Address = "Aydın, Merkez", Status = LeadStatus.New, Source = LeadSource.Phone, Description = "Tarımsal üretimde veri analizi ve saha yönetimi için dijital çözüm arayan çiftçi kooperatifi." },
                new { FirstName = "Hakan", LastName = "Öztürk", Company = "Kuzey İnşaat", Email = "hakan.ozturk@kuzeyinsaat.com", Phone = "+90 322 555 00 00", Website = "www.kuzeyinsaat.com", Address = "Seyhan, Adana", Status = LeadStatus.Qualified, Source = LeadSource.SocialMedia, Description = "Proje yönetimi ve kaynak planlaması için entegre çözüm isteyen inşaat firması." },
                new { FirstName = "Büşra", LastName = "Kaya", Company = "Batı Kimya", Email = "busra.kaya@batikimya.com", Phone = "+90 258 555 11 11", Website = "www.batikimya.com", Address = "Denizli, Merkez", Status = LeadStatus.Converted, Source = LeadSource.Website, Description = "Kimya üretiminde kalite kontrol ve süreç yönetimini iyileştirmek isteyen firma." },
                new { FirstName = "Emre", LastName = "Kılıç", Company = "Güney Finans", Email = "emre.kilic@guneyfinans.com", Phone = "+90 216 555 22 22", Website = "www.guneyfinans.com", Address = "Kadıköy, İstanbul", Status = LeadStatus.Contacted, Source = LeadSource.Email, Description = "Müşteri ilişkileri yönetimi ve kredi süreçlerini dijitalleştirmek isteyen finans kurumu." },
                new { FirstName = "Aslı", LastName = "Aksoy", Company = "Kaya Hukuk", Email = "asli.aksoy@kayahukuk.com", Phone = "+90 212 555 33 33", Website = "www.kayahukuk.com", Address = "Beşiktaş, İstanbul", Status = LeadStatus.Qualified, Source = LeadSource.Referral, Description = "Müvekkil yönetimi ve dava takibi için özel yazılım çözümü arayan hukuk bürosu." },
                new { FirstName = "Can", LastName = "Toprak", Company = "Yıldız Eğitim", Email = "can.toprak@yildizegitim.com", Phone = "+90 312 555 44 44", Website = "www.yildizegitim.com", Address = "Ankara, Çankaya", Status = LeadStatus.Lost, Source = LeadSource.Other, Description = "Öğrenci kayıt ve akademik takip sistemi arayan eğitim kurumu. Alternatif bir çözümle devam ediyor." },
                new { FirstName = "Ece", LastName = "Bulut", Company = "Mavi Turizm", Email = "ece.bulut@maviturizm.com", Phone = "+90 232 555 55 55", Website = "www.maviturizm.com", Address = "Konak, İzmir", Status = LeadStatus.New, Source = LeadSource.SocialMedia, Description = "Rezervasyon ve müşteri deneyimi yönetimi için dijital platform ihtiyacı." },
                new { FirstName = "Mert", LastName = "Güneş", Company = "Beyaz Perde Medya", Email = "mert.gunes@beyazperde.com", Phone = "+90 212 555 66 66", Website = "www.beyazperde.com", Address = "Şişli, İstanbul", Status = LeadStatus.Contacted, Source = LeadSource.Website, Description = "Reklam ve pazarlama kampanyaları için müşteri veri platformu arayan medya şirketi." },
                new { FirstName = "Gül", LastName = "Korkmaz", Company = "Alfa Mühendislik", Email = "gul.korkmaz@alfamühendislik.com", Phone = "+90 258 555 77 77", Website = "www.alfamühendislik.com", Address = "İzmir, Bornova", Status = LeadStatus.Qualified, Source = LeadSource.Phone, Description = "Proje yönetimi ve mühendislik süreçlerini entegre etmek isteyen firma." },
                new { FirstName = "Orhan", LastName = "Demir", Company = "Ceylan Enerji", Email = "orhan.demir@ceylanenerji.com", Phone = "+90 242 555 88 88", Website = "www.ceylanenerji.com", Address = "Antalya, Konyaaltı", Status = LeadStatus.Converted, Source = LeadSource.Referral, Description = "Enerji verimliliği ve karbon ayak izi yönetimi için dijital çözüm arayan firma." },
                new { FirstName = "Sibel", LastName = "Taş", Company = "Tuna Lojistik", Email = "sibel.tas@tunalojistik.com", Phone = "+90 216 555 99 99", Website = "www.tunalojistik.com", Address = "Üsküdar, İstanbul", Status = LeadStatus.Contacted, Source = LeadSource.Website, Description = "Depo yönetimi ve dağıtım planlaması için sistem arayan lojistik firması." },
                new { FirstName = "Burak", LastName = "Uçar", Company = "Hilal Sağlık", Email = "burak.ucar@hilalsaglik.com", Phone = "+90 312 555 00 00", Website = "www.hilalsaglik.com", Address = "Ankara, Yenimahalle", Status = LeadStatus.New, Source = LeadSource.Email, Description = "Sağlık kurumları için hasta randevu ve takip sistemi arayan firma." },
                new { FirstName = "Sinem", LastName = "Özer", Company = "Ata Teknoloji", Email = "sinem.ozer@atateknoloji.com", Phone = "+90 212 555 11 11", Website = "www.atateknoloji.com", Address = "Kadıköy, İstanbul", Status = LeadStatus.Qualified, Source = LeadSource.SocialMedia, Description = "Mobil uygulama geliştirme ve test süreçlerini yönetecek platform arayan yazılım şirketi." },
                new { FirstName = "İbrahim", LastName = "Şen", Company = "Dora Tarım", Email = "ibrahim.sen@doratarim.com", Phone = "+90 262 555 22 22", Website = "www.doratarim.com", Address = "Kocaeli, Merkez", Status = LeadStatus.Lost, Source = LeadSource.Phone, Description = "Tarımsal üretim planlaması ve hasat takibi için sistem ihtiyacı. Bütçe yetersiz." },
                new { FirstName = "Nazlı", LastName = "Çetin", Company = "Karbon Tekstil", Email = "nazli.cetin@karbontekstil.com", Phone = "+90 232 555 33 33", Website = "www.karbontekstil.com", Address = "İzmir, Konak", Status = LeadStatus.Converted, Source = LeadSource.Website, Description = "Moda endüstrisinde üretim ve tedarik zinciri yönetimini optimize etmek isteyen firma." },
                new { FirstName = "Barış", LastName = "Akman", Company = "Simge Danışmanlık", Email = "baris.akman@simgedanismanlik.com", Phone = "+90 216 555 44 44", Website = "www.simgedanismanlik.com", Address = "Kadıköy, İstanbul", Status = LeadStatus.Contacted, Source = LeadSource.Referral, Description = "Müşteri ilişkileri ve proje yönetimi için entegre sistem arayan danışmanlık firması." },
                new { FirstName = "Gizem", LastName = "Koç", Company = "Nil Petrol", Email = "gizem.koc@nilpetrol.com", Phone = "+90 322 555 55 55", Website = "www.nilpetrol.com", Address = "Adana, Seyhan", Status = LeadStatus.Qualified, Source = LeadSource.Email, Description = "Akaryakıt istasyonları yönetimi ve müşteri sadakati programı için yazılım çözümü arayan firma." },
                new { FirstName = "Cenk", LastName = "Yalçın", Company = "Ada Otelcilik", Email = "cenk.yalcin@adaotelcilik.com", Phone = "+90 212 555 66 66", Website = "www.adaotelcilik.com", Address = "Beşiktaş, İstanbul", Status = LeadStatus.New, Source = LeadSource.SocialMedia, Description = "Otel rezervasyon ve misafir deneyimi yönetimi için platform arayan zincir otel." },
                new { FirstName = "Melis", LastName = "Öztürk", Company = "Has Restoran", Email = "melis.ozturk@hasrestoran.com", Phone = "+90 312 555 77 77", Website = "www.hasrestoran.com", Address = "Ankara, Çankaya", Status = LeadStatus.Contacted, Source = LeadSource.Website, Description = "Restoran zinciri yönetimi ve sipariş takibi için sistem arayan firma." },
                new { FirstName = "Kaan", LastName = "Erkan", Company = "Doğa İnşaat", Email = "kaan.erkan@dogainsaat.com", Phone = "+90 258 555 88 88", Website = "www.dogainsaat.com", Address = "Denizli, Merkez", Status = LeadStatus.Qualified, Source = LeadSource.Phone, Description = "İnşaat projelerinde malzeme takibi ve iş gücü yönetimi için sistem arayan firma." },
                new { FirstName = "Pınar", LastName = "Türker", Company = "Renk Mobilya", Email = "pinar.turker@renkmobilya.com", Phone = "+90 216 555 99 99", Website = "www.renkmobilya.com", Address = "Ümraniye, İstanbul", Status = LeadStatus.Converted, Source = LeadSource.Referral, Description = "Mobilya üretiminde verimlilik ve stok yönetimini dijitalleştirmek isteyen firma." },
                new { FirstName = "Eren", LastName = "Sarı", Company = "Çınar Hukuk", Email = "eren.sari@cinarhukuk.com", Phone = "+90 212 555 00 00", Website = "www.cinarhukuk.com", Address = "Şişli, İstanbul", Status = LeadStatus.Lost, Source = LeadSource.Email, Description = "Dava yönetimi ve evrak takibi için sistem ihtiyacı. Mevcut sistemle devam kararı." }
            };

            var assignedUsers = _users.Where(u => u.UserName == "mustafa.kaya@salesflow.com" || u.UserName == "zeynep.celik@salesflow.com" || u.UserName == "ali.sahin@salesflow.com").ToList();
            var userIndex = 0;

            foreach (var lead in leadData)
            {
                var createdDate = DateTime.UtcNow.AddDays(-_random.Next(1, 180));

                _leads.Add(new Lead
                {
                    FirstName = lead.FirstName,
                    LastName = lead.LastName,
                    CompanyName = lead.Company,
                    Email = lead.Email,
                    PhoneNumber = lead.Phone,
                    Website = lead.Website,
                    Address = lead.Address,
                    Status = lead.Status,
                    Source = lead.Source,
                    Description = lead.Description,
                    AssignedUserId = assignedUsers[userIndex % assignedUsers.Count].Id,
                    CreatedDate = createdDate,
                    UpdatedDate = createdDate.AddDays(_random.Next(1, 10)),
                    IsDeleted = false
                });
                userIndex++;
            }
            await _context.Leads.AddRangeAsync(_leads);
        }
        #endregion

        #region Customers
        private async Task CreateCustomersAsync()
        {
            var customerData = new[]
            {
                new { Company = "İstanbul Teknoloji A.Ş.", ContactFirst = "Ahmet", ContactLast = "Yıldız", Email = "ahmet.yildiz@istanbultech.com", Phone = "+90 212 555 11 11", Website = "www.istanbultech.com", TaxNumber = "1234567890", Address = "Maslak, İstanbul", Type = CustomerType.Company, Description = "Orta ölçekli teknoloji firması. ERP ve iş zekası çözümlerine ihtiyaç duyuyor. 50'den fazla çalışanı var." },
                new { Company = "Ankara Sağlık Çözümleri", ContactFirst = "Elif", ContactLast = "Demirtaş", Email = "elif.demirtas@ankarasaglik.com", Phone = "+90 312 555 22 22", Website = "www.ankarasaglik.com", TaxNumber = "2345678901", Address = "Çankaya, Ankara", Type = CustomerType.Company, Description = "Özel hastane zinciri. Dijital sağlık dönüşümü sürecinde danışmanlık alıyor." },
                new { Company = "Ege Lojistik", ContactFirst = "Murat", ContactLast = "Kurt", Email = "murat.kurt@egelojistik.com", Phone = "+90 232 555 33 33", Website = "www.egelojistik.com", TaxNumber = "3456789012", Address = "Bornova, İzmir", Type = CustomerType.Company, Description = "Lojistik firması. Filo yönetimi ve rota optimizasyonu çözümü arıyor." },
                new { Company = "Güneş Enerji Sistemleri", ContactFirst = "Selin", ContactLast = "Özdemir", Email = "selin.ozdemir@guneSenerji.com", Phone = "+90 242 555 44 44", Website = "www.guneSenerji.com", TaxNumber = "4567890123", Address = "Konyaaltı, Antalya", Type = CustomerType.Company, Description = "Yenilenebilir enerji firması. Güneş enerjisi projeleri için finansman ve teknoloji danışmanlığı alıyor." },
                new { Company = "Marmara Gıda A.Ş.", ContactFirst = "Kemal", ContactLast = "Aydın", Email = "kemal.aydin@marmaragida.com", Phone = "+90 216 555 55 55", Website = "www.marmaragida.com", TaxNumber = "5678901234", Address = "Ümraniye, İstanbul", Type = CustomerType.Company, Description = "Gıda üreticisi. Üretim ve tedarik zinciri yönetimini dijitalleştirme sürecinde." },
                new { Company = "Akdeniz Tekstil", ContactFirst = "Zeynep", ContactLast = "Kara", Email = "zeynep.kara@akdeniztekstil.com", Phone = "+90 326 555 77 77", Website = "www.akdeniztekstil.com", TaxNumber = "6789012345", Address = "Antakya, Hatay", Type = CustomerType.Company, Description = "Tekstil üreticisi. Verimlilik ve stok yönetimi çözümleri arıyor." },
                new { Company = "Kuzey İnşaat", ContactFirst = "Hakan", ContactLast = "Öztürk", Email = "hakan.ozturk@kuzeyinsaat.com", Phone = "+90 322 555 00 00", Website = "www.kuzeyinsaat.com", TaxNumber = "7890123456", Address = "Seyhan, Adana", Type = CustomerType.Company, Description = "İnşaat firması. Proje yönetimi ve kaynak planlaması için entegre sistem arıyor." },
                new { Company = "Batı Kimya", ContactFirst = "Büşra", ContactLast = "Kaya", Email = "busra.kaya@batikimya.com", Phone = "+90 258 555 11 11", Website = "www.batikimya.com", TaxNumber = "8901234567", Address = "Denizli, Merkez", Type = CustomerType.Company, Description = "Kimya üreticisi. Kalite kontrol ve süreç yönetimini iyileştirmek istiyor." },
                new { Company = "Güney Finans", ContactFirst = "Emre", ContactLast = "Kılıç", Email = "emre.kilic@guneyfinans.com", Phone = "+90 216 555 22 22", Website = "www.guneyfinans.com", TaxNumber = "9012345678", Address = "Kadıköy, İstanbul", Type = CustomerType.Company, Description = "Finans kurumu. Müşteri ilişkileri ve kredi süreçlerini dijitalleştiriyor." },
                new { Company = "Kaya Hukuk", ContactFirst = "Aslı", ContactLast = "Aksoy", Email = "asli.aksoy@kayahukuk.com", Phone = "+90 212 555 33 33", Website = "www.kayahukuk.com", TaxNumber = "0123456789", Address = "Beşiktaş, İstanbul", Type = CustomerType.Company, Description = "Hukuk bürosu. Müvekkil yönetimi ve dava takibi için özel yazılım çözümü arıyor." },
                new { Company = "Alfa Mühendislik", ContactFirst = "Gül", ContactLast = "Korkmaz", Email = "gul.korkmaz@alfamühendislik.com", Phone = "+90 258 555 77 77", Website = "www.alfamühendislik.com", TaxNumber = "1234509876", Address = "Bornova, İzmir", Type = CustomerType.Company, Description = "Mühendislik firması. Proje yönetimi ve süreç entegrasyonu arıyor." },
                new { Company = "Ceylan Enerji", ContactFirst = "Orhan", ContactLast = "Demir", Email = "orhan.demir@ceylanenerji.com", Phone = "+90 242 555 88 88", Website = "www.ceylanenerji.com", TaxNumber = "2345610987", Address = "Konyaaltı, Antalya", Type = CustomerType.Company, Description = "Enerji firması. Karbon ayak izi yönetimi ve enerji verimliliği çözümü arıyor." },
                new { Company = "Tuna Lojistik", ContactFirst = "Sibel", ContactLast = "Taş", Email = "sibel.tas@tunalojistik.com", Phone = "+90 216 555 99 99", Website = "www.tunalojistik.com", TaxNumber = "3456721098", Address = "Üsküdar, İstanbul", Type = CustomerType.Company, Description = "Lojistik firması. Depo yönetimi ve dağıtım planlaması için sistem arıyor." },
                new { Company = "Ata Teknoloji", ContactFirst = "Sinem", ContactLast = "Özer", Email = "sinem.ozer@atateknoloji.com", Phone = "+90 212 555 11 11", Website = "www.atateknoloji.com", TaxNumber = "4567832109", Address = "Kadıköy, İstanbul", Type = CustomerType.Company, Description = "Yazılım şirketi. Mobil uygulama geliştirme ve test süreçleri için platform arıyor." },
                new { Company = "Karbon Tekstil", ContactFirst = "Nazlı", ContactLast = "Çetin", Email = "nazli.cetin@karbontekstil.com", Phone = "+90 232 555 33 33", Website = "www.karbontekstil.com", TaxNumber = "5678943210", Address = "Konak, İzmir", Type = CustomerType.Company, Description = "Moda endüstrisinde üretim ve tedarik zinciri yönetimini optimize ediyor." },
                new { Company = "Simge Danışmanlık", ContactFirst = "Barış", ContactLast = "Akman", Email = "baris.akman@simgedanismanlik.com", Phone = "+90 216 555 44 44", Website = "www.simgedanismanlik.com", TaxNumber = "6789054321", Address = "Kadıköy, İstanbul", Type = CustomerType.Company, Description = "Danışmanlık firması. Müşteri ilişkileri ve proje yönetimi için sistem arıyor." },
                new { Company = "Nil Petrol", ContactFirst = "Gizem", ContactLast = "Koç", Email = "gizem.koc@nilpetrol.com", Phone = "+90 322 555 55 55", Website = "www.nilpetrol.com", TaxNumber = "7890165432", Address = "Seyhan, Adana", Type = CustomerType.Company, Description = "Petrol şirketi. Akaryakıt istasyonları yönetimi ve müşteri sadakati programı arıyor." },
                new { Company = "Renk Mobilya", ContactFirst = "Pınar", ContactLast = "Türker", Email = "pinar.turker@renkmobilya.com", Phone = "+90 216 555 99 99", Website = "www.renkmobilya.com", TaxNumber = "8901276543", Address = "Ümraniye, İstanbul", Type = CustomerType.Company, Description = "Mobilya üreticisi. Verimlilik ve stok yönetimini dijitalleştiriyor." }
            };

            var assignedUsers = _users.Where(u => u.UserName != "mehmet.demir@salesflow.com").ToList();
            var userIndex = 0;

            foreach (var customer in customerData)
            {
                var createdDate = DateTime.UtcNow.AddDays(-_random.Next(30, 160));

                _customers.Add(new Customer
                {
                    CompanyName = customer.Company,
                    ContactFirstName = customer.ContactFirst,
                    ContactLastName = customer.ContactLast,
                    Email = customer.Email,
                    PhoneNumber = customer.Phone,
                    Website = customer.Website,
                    TaxNumber = customer.TaxNumber,
                    Address = customer.Address,
                    CustomerType = customer.Type,
                    Description = customer.Description,
                    AssignedUserId = assignedUsers[userIndex % assignedUsers.Count].Id,
                    CreatedDate = createdDate,
                    UpdatedDate = createdDate.AddDays(_random.Next(1, 15)),
                    IsDeleted = false
                });
                userIndex++;
            }
            await _context.Customers.AddRangeAsync(_customers);
        }
        #endregion

        #region Customer Tags
        private async Task CreateCustomerTagsAsync()
        {
            var customerTags = new List<CustomerTag>();

            var tagMappings = new[]
            {
        new { Customer = "İstanbul Teknoloji A.Ş.", Tags = new[] { "Enterprise", "High Value" } },
        new { Customer = "Ankara Sağlık Çözümleri", Tags = new[] { "Healthcare", "Enterprise", "VIP" } },
        new { Customer = "Ege Lojistik", Tags = new[] { "Logistics", "SMB" } },
        new { Customer = "Güneş Enerji Sistemleri", Tags = new[] { "Renewable Energy", "High Value" } },
        new { Customer = "Marmara Gıda A.Ş.", Tags = new[] { "Manufacturing", "Enterprise" } },
        new { Customer = "Akdeniz Tekstil", Tags = new[] { "Manufacturing", "SMB" } },
        new { Customer = "Kuzey İnşaat", Tags = new[] { "High Value", "Enterprise" } },
        new { Customer = "Batı Kimya", Tags = new[] { "Manufacturing", "Enterprise" } },
        new { Customer = "Güney Finans", Tags = new[] { "Enterprise", "High Value", "VIP" } },
        new { Customer = "Kaya Hukuk", Tags = new[] { "SMB", "Decision Maker" } },
        new { Customer = "Alfa Mühendislik", Tags = new[] { "Enterprise", "Decision Maker" } },
        new { Customer = "Ceylan Enerji", Tags = new[] { "Renewable Energy", "High Value" } },
        new { Customer = "Tuna Lojistik", Tags = new[] { "Logistics", "Enterprise" } },
        new { Customer = "Ata Teknoloji", Tags = new[] { "Enterprise", "High Value" } },
        new { Customer = "Karbon Tekstil", Tags = new[] { "Manufacturing", "Enterprise" } },
        new { Customer = "Simge Danışmanlık", Tags = new[] { "SMB", "Decision Maker" } },
        new { Customer = "Nil Petrol", Tags = new[] { "Enterprise", "High Value" } },
        new { Customer = "Renk Mobilya", Tags = new[] { "Manufacturing", "SMB" } }
    };

            foreach (var mapping in tagMappings)
            {
                var customer = _customers.FirstOrDefault(c => c.CompanyName == mapping.Customer);
                if (customer == null)
                    continue;

                var usedTags = new HashSet<Tag>();

                foreach (var tagName in mapping.Tags)
                {
                    var tag = _tags.FirstOrDefault(t => t.Name == tagName);

                    if (tag == null)
                        continue;

                    usedTags.Add(tag);

                    customerTags.Add(new CustomerTag
                    {
                        Customer = customer,
                        Tag = tag
                    });
                }

                // 0 veya 1 ekstra rastgele etiket
                var remainingTags = _tags
                    .Where(t => !usedTags.Contains(t))
                    .ToList();

                var extraTagCount = _random.Next(0, 2);

                for (int i = 0; i < extraTagCount && remainingTags.Count > 0; i++)
                {
                    var randomTag = remainingTags[_random.Next(remainingTags.Count)];

                    customerTags.Add(new CustomerTag
                    {
                        Customer = customer,
                        Tag = randomTag
                    });

                    remainingTags.Remove(randomTag);
                }
            }

            await _context.CustomerTags.AddRangeAsync(customerTags);
        }
        #endregion

        #region Deals
        private async Task CreateDealsAsync()
        {
            var dealData = new[]
            {
                new { Customer = "İstanbul Teknoloji A.Ş.", Title = "ERP Dijital Dönüşüm Projesi", Amount = 380000, Stage = DealStage.Qualified, Description = "Teknoloji firmasının ERP sistemini modernize etme projesi." },
                new { Customer = "Ankara Sağlık Çözümleri", Title = "Hastane Yönetim Sistemi Modernizasyonu", Amount = 720000, Stage = DealStage.Won, Description = "Özel hastane zinciri için dijital sağlık platformu." },
                new { Customer = "Ege Lojistik", Title = "Filo Yönetimi Otomasyonu", Amount = 150000, Stage = DealStage.ProposalSent, Description = "Lojistik firmasının araç filosunu yönetecek sistem." },
                new { Customer = "Güneş Enerji Sistemleri", Title = "Güneş Enerjisi Danışmanlık Platformu", Amount = 480000, Stage = DealStage.Negotiation, Description = "Yenilenebilir enerji projeleri için danışmanlık platformu." },
                new { Customer = "Marmara Gıda A.Ş.", Title = "Tedarik Zinciri Yönetimi Platformu", Amount = 280000, Stage = DealStage.Qualified, Description = "Gıda şirketinin tedarik zincirini optimize edecek sistem." },
                new { Customer = "Akdeniz Tekstil", Title = "Stok Yönetimi ve Verimlilik Sistemi", Amount = 180000, Stage = DealStage.Won, Description = "Tekstil üretiminde verimlilik artışı sağlayacak çözüm." },
                new { Customer = "Kuzey İnşaat", Title = "Proje ve Kaynak Yönetimi Platformu", Amount = 320000, Stage = DealStage.Negotiation, Description = "İnşaat projelerinde kaynak planlaması ve takip sistemi." },
                new { Customer = "Batı Kimya", Title = "Kalite Kontrol Otomasyonu", Amount = 220000, Stage = DealStage.New, Description = "Kimya üretiminde kalite kontrol süreçlerini otomatize eden sistem." },
                new { Customer = "Güney Finans", Title = "Müşteri İlişkileri Yönetimi Platformu", Amount = 420000, Stage = DealStage.Won, Description = "Finans kurumu için kapsamlı CRM ve kredi yönetim platformu." },
                new { Customer = "Kaya Hukuk", Title = "Hukuk Bürosu Dava Yönetim Sistemi", Amount = 90000, Stage = DealStage.ProposalSent, Description = "Hukuk bürosu için özel dava ve müvekkil takip sistemi." },
                new { Customer = "Alfa Mühendislik", Title = "Mühendislik Proje Yönetimi Platformu", Amount = 260000, Stage = DealStage.Lost, Description = "Mühendislik firması için entegre proje yönetim sistemi." },
                new { Customer = "Ceylan Enerji", Title = "Karbon Ayak İzi Yönetim Sistemi", Amount = 350000, Stage = DealStage.Qualified, Description = "Enerji firması için karbon ayak izi takip ve raporlama sistemi." },
                new { Customer = "Tuna Lojistik", Title = "Depo Yönetimi Otomasyonu", Amount = 190000, Stage = DealStage.Negotiation, Description = "Lojistik firması için depo ve dağıtım yönetim sistemi." },
                new { Customer = "Ata Teknoloji", Title = "Mobil Uygulama Test Platformu", Amount = 300000, Stage = DealStage.Won, Description = "Yazılım şirketi için mobil uygulama test ve yayın platformu." },
                new { Customer = "Karbon Tekstil", Title = "Moda Endüstrisi Tedarik Zinciri Platformu", Amount = 230000, Stage = DealStage.ProposalSent, Description = "Tekstil firması için tedarik zinciri ve üretim yönetim platformu." }
            };

            var assignedUsers = _users.Where(u => u.UserName == "mustafa.kaya@salesflow.com" || u.UserName == "zeynep.celik@salesflow.com" || u.UserName == "ali.sahin@salesflow.com").ToList();
            var userIndex = 0;

            foreach (var deal in dealData)
            {
                var customer = _customers.FirstOrDefault(c => c.CompanyName == deal.Customer);
                if (customer == null) continue;

                var createdDate = customer.CreatedDate.AddDays(_random.Next(5, 30));

                _deals.Add(new Deal
                {
                    Title = deal.Title,
                    Description = deal.Description,
                    Amount = deal.Amount,
                    Stage = deal.Stage,
                    CustomerId = customer.Id,
                    AssignedUserId = assignedUsers[userIndex % assignedUsers.Count].Id,
                    ExpectedCloseDate = createdDate.AddDays(_random.Next(30, 90)),
                    CreatedDate = createdDate,
                    UpdatedDate = createdDate.AddDays(_random.Next(1, 10)),
                    IsDeleted = false
                });
                userIndex++;
            }
            await _context.Deals.AddRangeAsync(_deals);
        }
        #endregion

        #region Meetings
        private async Task CreateMeetingsAsync()
        {
            var meetingData = new List<(string Customer, string Title, string Description, MeetingType Type, MeetingStatus Status, int DaysAfter)>
            {
                ("İstanbul Teknoloji A.Ş.", "Keşif Toplantısı", "ERP sistem ihtiyaçlarının belirlenmesi", MeetingType.Office, MeetingStatus.Completed, 3),
                ("İstanbul Teknoloji A.Ş.", "Teknik Değerlendirme", "Teknik altyapı ve entegrasyon analizi", MeetingType.Online, MeetingStatus.Completed, 10),
                ("Ankara Sağlık Çözümleri", "Ön İnceleme", "Hastane yönetim sistemi demo ve ihtiyaç analizi", MeetingType.CustomerVisit, MeetingStatus.Completed, 5),
                ("Ankara Sağlık Çözümleri", "Proje Planlama", "Uygulama planı ve zaman çizelgesi değerlendirmesi", MeetingType.Online, MeetingStatus.Completed, 15),
                ("Ege Lojistik", "Demo Sunumu", "Filo yönetim sistemi demo", MeetingType.Office, MeetingStatus.Completed, 2),
                ("Ege Lojistik", "Teknik Detay", "Sistem entegrasyonu ve özelleştirme gereksinimleri", MeetingType.Phone, MeetingStatus.Completed, 8),
                ("Güneş Enerji Sistemleri", "Proje Başlangıç Toplantısı", "Enerji danışmanlık platformu kapsam belirleme", MeetingType.Online, MeetingStatus.Completed, 7),
                ("Güneş Enerji Sistemleri", "İlerleme Değerlendirme", "Proje ilerlemesi ve sonraki adımlar", MeetingType.Online, MeetingStatus.Scheduled, 20),
                ("Marmara Gıda A.Ş.", "İhtiyaç Analizi", "Tedarik zinciri süreçlerinin detaylandırılması", MeetingType.CustomerVisit, MeetingStatus.Completed, 4),
                ("Akdeniz Tekstil", "Stok Yönetimi Çalıştayı", "Mevcut stok süreçlerinin değerlendirilmesi", MeetingType.Office, MeetingStatus.Completed, 6),
                ("Kuzey İnşaat", "Proje Yönetimi Değerlendirme", "İnşaat projelerinde mevcut zorluklar", MeetingType.CustomerVisit, MeetingStatus.Completed, 12),
                ("Batı Kimya", "Kalite Kontrol Toplantısı", "Kalite kontrol süreçlerinin iyileştirilmesi", MeetingType.Phone, MeetingStatus.Completed, 9),
                ("Güney Finans", "CRM Sistemi Demo", "Finans sektörü CRM çözümü demosu", MeetingType.Online, MeetingStatus.Completed, 3),
                ("Kaya Hukuk", "Hukuk Sistemi İhtiyaç Analizi", "Dava yönetim ihtiyaçlarının belirlenmesi", MeetingType.Office, MeetingStatus.Completed, 5),
                ("Alfa Mühendislik", "Proje Yönetimi Tanıtımı", "Entegre proje yönetim sistemi sunumu", MeetingType.Online, MeetingStatus.Cancelled, 7),
                ("Ceylan Enerji", "Karbon Yönetim Sistemi Demo", "Karbon ayak izi platformu sunumu", MeetingType.Office, MeetingStatus.Completed, 11),
                ("Tuna Lojistik", "Depo Yönetimi İhtiyaç Analizi", "Depo süreçlerinin değerlendirilmesi", MeetingType.CustomerVisit, MeetingStatus.Completed, 14),
                ("Ata Teknoloji", "Mobil Test Platformu Demo", "Mobil test platformu sunumu ve değerlendirme", MeetingType.Online, MeetingStatus.Completed, 8),
                ("Karbon Tekstil", "Tedarik Zinciri Toplantısı", "Moda endüstrisi tedarik zinciri ihtiyaçları", MeetingType.Office, MeetingStatus.Completed, 2),
                ("Simge Danışmanlık", "Danışmanlık İhtiyaç Analizi", "Proje yönetimi ve CRM sistem ihtiyaçları", MeetingType.Phone, MeetingStatus.Completed, 6),
                ("Nil Petrol", "Akaryakıt Yönetim Sistemi Demo", "Müşteri sadakati programı demo", MeetingType.Online, MeetingStatus.Scheduled, 30),
                ("Renk Mobilya", "Üretim Verimlilik Toplantısı", "Mobilya üretiminde verimlilik artışı", MeetingType.CustomerVisit, MeetingStatus.Completed, 10),
                ("İstanbul Teknoloji A.Ş.", "Proje Değerlendirme", "ERP projesi değerlendirme toplantısı", MeetingType.Online, MeetingStatus.Completed, 18),
                ("Ankara Sağlık Çözümleri", "Son Değerlendirme", "Sağlık platformu son değerlendirme", MeetingType.Office, MeetingStatus.Completed, 25),
                ("Ege Lojistik", "Uygulama Planlama", "Filo yönetim sistem uygulama planı", MeetingType.Phone, MeetingStatus.Completed, 15),
                ("Güneş Enerji Sistemleri", "Finansal Değerlendirme", "Proje bütçe ve finansal planlama", MeetingType.Online, MeetingStatus.Completed, 12),
                ("Marmara Gıda A.Ş.", "Sistem Entegrasyonu", "Tedarik zinciri ve ERP entegrasyonu", MeetingType.CustomerVisit, MeetingStatus.Completed, 20),
                ("Akdeniz Tekstil", "Verimlilik Değerlendirmesi", "Stok sisteminin performans değerlendirmesi", MeetingType.Office, MeetingStatus.Completed, 14),
                ("Kuzey İnşaat", "Teknik Değerlendirme", "Proje yönetim sisteminin teknik detayları", MeetingType.Online, MeetingStatus.Scheduled, 45),
                ("Batı Kimya", "İlerleme Toplantısı", "Kalite kontrol sistem ilerlemesi", MeetingType.Phone, MeetingStatus.Completed, 20)
            };

            var assignedUsers = _users.Where(u => u.UserName == "mustafa.kaya@salesflow.com" || u.UserName == "zeynep.celik@salesflow.com" || u.UserName == "ali.sahin@salesflow.com").ToList();
            var userIndex = 0;

            foreach (var meeting in meetingData)
            {
                var customer = _customers.FirstOrDefault(c => c.CompanyName == meeting.Customer);
                if (customer == null) continue;

                var baseDate = customer.CreatedDate.AddDays(meeting.DaysAfter);
                var startHour = _random.Next(8, 16);

                _meetings.Add(new Meeting
                {
                    Title = meeting.Title,
                    Description = meeting.Description,
                    Type = meeting.Type,
                    Status = meeting.Status,
                    Location = meeting.Type == MeetingType.Online ? "Zoom Meeting" :
                               meeting.Type == MeetingType.CustomerVisit ? customer.Address : "SalesFlow Ofisi",
                    CustomerId = customer.Id,
                    AssignedUserId = assignedUsers[userIndex % assignedUsers.Count].Id,
                    StartDate = baseDate.AddHours(startHour),
                    EndDate = baseDate.AddHours(startHour + 1),
                    CreatedDate = baseDate.AddDays(-1),
                    UpdatedDate = baseDate,
                    IsDeleted = false
                });
                userIndex++;
            }
            await _context.Meetings.AddRangeAsync(_meetings);
        }
        #endregion

        #region Tasks
        private async Task CreateTasksAsync()
        {
            var taskData = new[]
            {
                new { Customer = "İstanbul Teknoloji A.Ş.", Title = "Teknik şartname hazırla", Description = "ERP sistemi için teknik gereksinimleri belirle", DaysAfter = 2, Priority = TaskPriority.High, Status = TaskStatus.Completed },
                new { Customer = "İstanbul Teknoloji A.Ş.", Title = "Demo ortamını hazırla", Description = "Özel demo ortamı ve test verileri oluştur", DaysAfter = 5, Priority = TaskPriority.Medium, Status = TaskStatus.Completed },
                new { Customer = "Ankara Sağlık Çözümleri", Title = "Sözleşme taslağını hazırla", Description = "Sağlık platformu sözleşmesini hazırla", DaysAfter = 3, Priority = TaskPriority.High, Status = TaskStatus.Completed },
                new { Customer = "Ankara Sağlık Çözümleri", Title = "Entegrasyon planını yap", Description = "Mevcut sistemlerle entegrasyon planı oluştur", DaysAfter = 10, Priority = TaskPriority.Medium, Status = TaskStatus.Completed },
                new { Customer = "Ege Lojistik", Title = "Filo verilerini analiz et", Description = "Mevcut filo verilerini analiz et ve raporla", DaysAfter = 1, Priority = TaskPriority.High, Status = TaskStatus.Completed },
                new { Customer = "Ege Lojistik", Title = "Fiyat teklifi sun", Description = "Filo yönetim sistemi için fiyat teklifi hazırla", DaysAfter = 7, Priority = TaskPriority.Medium, Status = TaskStatus.Completed },
                new { Customer = "Güneş Enerji Sistemleri", Title = "Proje planı oluştur", Description = "Enerji danışmanlık platformu proje planı", DaysAfter = 2, Priority = TaskPriority.High, Status = TaskStatus.InProgress },
                new { Customer = "Güneş Enerji Sistemleri", Title = "Ar-Ge ekibiyle toplan", Description = "Ar-Ge ekibiyle teknik detayları görüş", DaysAfter = 8, Priority = TaskPriority.Medium, Status = TaskStatus.InProgress },
                new { Customer = "Marmara Gıda A.Ş.", Title = "Tedarik zinciri haritası çıkar", Description = "Mevcut tedarik zinciri süreçlerini haritalandır", DaysAfter = 3, Priority = TaskPriority.Medium, Status = TaskStatus.Completed },
                new { Customer = "Marmara Gıda A.Ş.", Title = "Demo hazırlığı yap", Description = "Tedarik zinciri platformu demo sunumu hazırla", DaysAfter = 12, Priority = TaskPriority.High, Status = TaskStatus.Pending },
                new { Customer = "Akdeniz Tekstil", Title = "Stok verilerini topla", Description = "Mevcut stok verilerini topla ve analiz et", DaysAfter = 2, Priority = TaskPriority.Medium, Status = TaskStatus.Completed },
                new { Customer = "Akdeniz Tekstil", Title = "Rapor hazırla", Description = "Verimlilik artış potansiyeli raporu hazırla", DaysAfter = 8, Priority = TaskPriority.High, Status = TaskStatus.Completed },
                new { Customer = "Kuzey İnşaat", Title = "Proje yönetimi demo hazırla", Description = "İnşaat proje yönetim demo içeriği hazırla", DaysAfter = 4, Priority = TaskPriority.Medium, Status = TaskStatus.Completed },
                new { Customer = "Kuzey İnşaat", Title = "Kaynak planlaması analizi", Description = "Mevcut kaynak kullanımını analiz et", DaysAfter = 10, Priority = TaskPriority.High, Status = TaskStatus.Completed },
                new { Customer = "Batı Kimya", Title = "Kalite standartlarını incele", Description = "Kimya sektörü kalite standartlarını araştır", DaysAfter = 2, Priority = TaskPriority.Medium, Status = TaskStatus.Completed },
                new { Customer = "Batı Kimya", Title = "Otomasyon planı yap", Description = "Kalite kontrol otomasyon planını hazırla", DaysAfter = 9, Priority = TaskPriority.High, Status = TaskStatus.InProgress },
                new { Customer = "Güney Finans", Title = "Finansal analiz yap", Description = "CRM sisteminin finansal getirisini analiz et", DaysAfter = 1, Priority = TaskPriority.High, Status = TaskStatus.Completed },
                new { Customer = "Güney Finans", Title = "Özelleştirme planı", Description = "Finans sektörüne özel özelleştirmeleri planla", DaysAfter = 6, Priority = TaskPriority.Medium, Status = TaskStatus.Completed },
                new { Customer = "Kaya Hukuk", Title = "Dava yönetim sürecini analiz et", Description = "Mevcut dava yönetim süreçlerini değerlendir", DaysAfter = 3, Priority = TaskPriority.Medium, Status = TaskStatus.Completed },
                new { Customer = "Kaya Hukuk", Title = "Örnek veri hazırla", Description = "Demo için örnek dava ve müvekkil verileri hazırla", DaysAfter = 7, Priority = TaskPriority.Low, Status = TaskStatus.Completed },
                new { Customer = "Alfa Mühendislik", Title = "Proje metodolojisi belirle", Description = "Proje yönetimi metodolojisini belirle", DaysAfter = 5, Priority = TaskPriority.Medium, Status = TaskStatus.Cancelled },
                new { Customer = "Ceylan Enerji", Title = "Karbon ayak izi verilerini topla", Description = "Mevcut karbon ayak izi verilerini topla", DaysAfter = 2, Priority = TaskPriority.High, Status = TaskStatus.Completed },
                new { Customer = "Ceylan Enerji", Title = "Rapor şablonu hazırla", Description = "Karbon ayak izi rapor şablonu hazırla", DaysAfter = 8, Priority = TaskPriority.Medium, Status = TaskStatus.InProgress },
                new { Customer = "Tuna Lojistik", Title = "Depo süreçlerini haritalandır", Description = "Mevcut depo iş akışlarını belgele", DaysAfter = 1, Priority = TaskPriority.High, Status = TaskStatus.Completed },
                new { Customer = "Tuna Lojistik", Title = "Otomasyon ihtiyaçlarını belirle", Description = "Depo otomasyon ihtiyaçlarını listele", DaysAfter = 6, Priority = TaskPriority.Medium, Status = TaskStatus.Completed },
                new { Customer = "Ata Teknoloji", Title = "Test stratejisi oluştur", Description = "Mobil test platformu için test stratejisi", DaysAfter = 3, Priority = TaskPriority.High, Status = TaskStatus.Completed },
                new { Customer = "Ata Teknoloji", Title = "Entegrasyon dokümanı hazırla", Description = "API entegrasyon dokümanını hazırla", DaysAfter = 10, Priority = TaskPriority.Medium, Status = TaskStatus.Pending },
                new { Customer = "Karbon Tekstil", Title = "Tedarik zinciri analizi", Description = "Moda endüstrisi tedarik zinciri ihtiyaçları", DaysAfter = 2, Priority = TaskPriority.Medium, Status = TaskStatus.Completed },
                new { Customer = "Karbon Tekstil", Title = "Demo içeriği hazırla", Description = "Özel demo içeriği ve senaryoları hazırla", DaysAfter = 7, Priority = TaskPriority.High, Status = TaskStatus.Completed },
                new { Customer = "Simge Danışmanlık", Title = "Danışmanlık süreç analizi", Description = "Mevcut danışmanlık süreçlerini analiz et", DaysAfter = 3, Priority = TaskPriority.Medium, Status = TaskStatus.Completed },
                new { Customer = "Nil Petrol", Title = "Müşteri verilerini analiz et", Description = "Akaryakıt istasyonları müşteri verilerini analiz et", DaysAfter = 4, Priority = TaskPriority.High, Status = TaskStatus.Completed },
                new { Customer = "Nil Petrol", Title = "Program tasarımını yap", Description = "Müşteri sadakati programı tasarımı", DaysAfter = 12, Priority = TaskPriority.Medium, Status = TaskStatus.Pending },
                new { Customer = "Renk Mobilya", Title = "Üretim verimlilik analizi", Description = "Mobilya üretiminde verimlilik analizi", DaysAfter = 2, Priority = TaskPriority.Medium, Status = TaskStatus.Completed },
                new { Customer = "Renk Mobilya", Title = "İyileştirme önerilerini hazırla", Description = "Verimlilik artışı için iyileştirme önerileri", DaysAfter = 8, Priority = TaskPriority.High, Status = TaskStatus.Completed }
            };

            var assignedUsers = _users.Where(u => u.UserName == "mustafa.kaya@salesflow.com" || u.UserName == "zeynep.celik@salesflow.com" || u.UserName == "ali.sahin@salesflow.com").ToList();
            var userIndex = 0;

            foreach (var task in taskData)
            {
                var customer = _customers.FirstOrDefault(c => c.CompanyName == task.Customer);
                if (customer == null) continue;

                var baseDate = customer.CreatedDate.AddDays(task.DaysAfter);

                _tasks.Add(new TaskItem
                {
                    Title = task.Title,
                    Description = task.Description,
                    Priority = task.Priority,
                    Status = task.Status,
                    CustomerId = customer.Id,
                    AssignedUserId = assignedUsers[userIndex % assignedUsers.Count].Id,
                    DueDate = baseDate.AddDays(_random.Next(1, 14)),
                    CreatedDate = baseDate,
                    UpdatedDate = baseDate.AddDays(_random.Next(1, 3)),
                    IsDeleted = false
                });
                userIndex++;
            }
            await _context.TaskItems.AddRangeAsync(_tasks);
        }
        #endregion

        #region Notes
        private async Task CreateNotesAsync()
        {
            var noteData = new[]
            {
                new { Customer = "İstanbul Teknoloji A.Ş.", Content = "Mevcut ERP sistemleri 5 yıllık ve artık ihtiyaçları karşılamıyor. Yeni sistemde modüler yapı ve API desteği öncelikli. Mevcut veriler SQL Server'da. 50'den fazla kullanıcı mevcut.", DaysAfter = 2 },
                new { Customer = "İstanbul Teknoloji A.Ş.", Content = "Teknik ekip toplantısı yapıldı. Özellikle depolama, finans ve üretim modülleri öncelikli. Raporlama ihtiyaçları Power BI entegrasyonu ile çözülecek.", DaysAfter = 8 },
                new { Customer = "Ankara Sağlık Çözümleri", Content = "Hastane yönetim sisteminde randevu takibi, hasta kayıtları ve fatura modülleri kritik. 15 doktor ve 30 hemşire mevcut. Özel hastane zinciri olduğu için ölçeklenebilirlik önemli.", DaysAfter = 3 },
                new { Customer = "Ankara Sağlık Çözümleri", Content = "Dijital sağlık dönüşümü projesi kapsamında e-Nabız entegrasyonu gerekiyor. Veri güvenliği KVKK ve GDPR uyumu öncelikli.", DaysAfter = 10 },
                new { Customer = "Ege Lojistik", Content = "Filo yönetiminde 40 araç var. Araç bakım planlaması, şoför yönetimi ve rota optimizasyonu mevcut değil. Manuel Excel takibi yapılıyor.", DaysAfter = 1 },
                new { Customer = "Ege Lojistik", Content = "Rota optimizasyon algoritması test edildi. %15 yakıt tasarrufu ve %20 zaman iyileştirmesi potansiyeli görüldü. Deneme sürüşüne başlanacak.", DaysAfter = 6 },
                new { Customer = "Güneş Enerji Sistemleri", Content = "Güneş enerjisi projeleri için 5 farklı lokasyonda saha çalışması yapıldı. Veriler toplandı ve analiz aşamasında.", DaysAfter = 4 },
                new { Customer = "Güneş Enerji Sistemleri", Content = "Finansman seçenekleri değerlendiriliyor. Öncelikli projeler için yatırım geri dönüş süreleri hesaplandı. En iyi senaryoda 7 yıl.", DaysAfter = 10 },
                new { Customer = "Marmara Gıda A.Ş.", Content = "Tedarik zincirinde 100'den fazla tedarikçi var. Mal kabul, kalite kontrol ve stok süreçleri manuel. ERP ile entegrasyon planlanıyor.", DaysAfter = 2 },
                new { Customer = "Marmara Gıda A.Ş.", Content = "Üretim planlamasında talep tahmini ve kapasite yönetimi zorlukları mevcut. Yapay zeka destekli tahmin modeli önerildi.", DaysAfter = 8 },
                new { Customer = "Akdeniz Tekstil", Content = "Mevcut stok takibi Excel'de yapılıyor. 20.000'den fazla ürün çeşidi var. Sipariş karşılama oranı %75, iyileştirme potansiyeli mevcut.", DaysAfter = 1 },
                new { Customer = "Akdeniz Tekstil", Content = "Üretim verimliliği %68 seviyesinde. İyileştirme önerileri değerlendiriliyor. Otomasyon yatırımı yapılacak.", DaysAfter = 7 },
                new { Customer = "Kuzey İnşaat", Content = "Mevcut proje yönetimi Excel tabanlı. 15 aktif proje ve 200+ çalışan. Kaynak çakışmaları ve gecikmeler yaşanıyor.", DaysAfter = 3 },
                new { Customer = "Kuzey İnşaat", Content = "Proje yönetim sistemi demo yapıldı. Özellikle bütçe takibi ve malzeme yönetimi modülleri beğenildi. Pilot uygulama planlanıyor.", DaysAfter = 10 },
                new { Customer = "Batı Kimya", Content = "Kalite kontrol süreçleri laboratuvar ortamında manuel. Örnek alımı, test ve raporlama otomasyonu hedefleniyor.", DaysAfter = 2 },
                new { Customer = "Batı Kimya", Content = "ISO 9001 ve ISO 14001 standartlarına uyum gerekiyor. Kalite kontrol sisteminin bu standartları desteklemesi lazım.", DaysAfter = 8 },
                new { Customer = "Güney Finans", Content = "Mevcut müşteri verileri 10 farklı sistemde dağınık. Müşteri ilişkileri yönetimi için entegre platform ihtiyacı.", DaysAfter = 2 },
                new { Customer = "Güney Finans", Content = "Kredi süreçleri ortalama 15 gün sürüyor. Otomasyonla bu süre 5 güne düşürülebilir. Müşteri memnuniyeti artacak.", DaysAfter = 7 },
                new { Customer = "Kaya Hukuk", Content = "15 avukat ve 5 stajyer avukat var. Dava yönetimi, süre takibi ve belge yönetimi için sistem ihtiyacı.", DaysAfter = 1 },
                new { Customer = "Kaya Hukuk", Content = "KVKK uyumlu dosya yönetimi ve güvenli erişim çok önemli. Müvekkil bilgilerinin gizliliği kritik.", DaysAfter = 5 },
                new { Customer = "Alfa Mühendislik", Content = "Proje yönetiminde mevcut sistem yetersiz. 30 mühendis ve 10 proje sorumlusu var. Proje bazlı maliyet takibi zor.", DaysAfter = 3 },
                new { Customer = "Alfa Mühendislik", Content = "Rekabetçi teklif değerlendirmesi yapıldı. Maliyetler uygun değil, alternatif çözümlere bakılacak.", DaysAfter = 9 },
                new { Customer = "Ceylan Enerji", Content = "Karbon ayak izi verileri manuel toplanıyor. 3 tesis var ve sürdürülebilirlik raporlaması için sistem ihtiyacı.", DaysAfter = 2 },
                new { Customer = "Ceylan Enerji", Content = "Saha ziyaretleri tamamlandı. Veri toplama süreçleri belgelendi. Yazılım entegrasyonu hazır.", DaysAfter = 8 },
                new { Customer = "Tuna Lojistik", Content = "3 depo ve 25 dağıtım aracı var. Depo verimliliği %65 seviyesinde. Otomasyonla %85'e çıkarılabilir.", DaysAfter = 1 },
                new { Customer = "Tuna Lojistik", Content = "Depo yönetim sistemi demo beğenildi. Özellikle RF kullanımı ve stok doğruluğu modülleri önemli.", DaysAfter = 5 },
                new { Customer = "Ata Teknoloji", Content = "Mobil uygulama geliştirme süreci 6 aydır devam ediyor. Test süreçleri manuel ve zaman alıcı.", DaysAfter = 3 },
                new { Customer = "Ata Teknoloji", Content = "CI/CD entegrasyonu ve otomatik testler için platform değerlendirmesi yapılıyor. API testleri önemli.", DaysAfter = 9 },
                new { Customer = "Karbon Tekstil", Content = "Moda endüstrisinde 15 tedarikçi ile çalışılıyor. Tedarikçi yönetimi ve sipariş takibi zorlukları mevcut.", DaysAfter = 2 },
                new { Customer = "Karbon Tekstil", Content = "Sürdürülebilirlik raporlaması ve karbon ayak izi de önemli. Moda sektöründe yeşil dönüşüm trendi.", DaysAfter = 7 },
                new { Customer = "Simge Danışmanlık", Content = "Danışmanlık firmasında 10 danışman ve 50 müşteri var. Proje yönetimi ve CRM sistemine ihtiyaç var.", DaysAfter = 3 },
                new { Customer = "Nil Petrol", Content = "20 akaryakıt istasyonu. Müşteri verileri parçalı ve sadakat programı yok. Müşteri kazanımı ve elde tutumu zor.", DaysAfter = 2 },
                new { Customer = "Nil Petrol", Content = "Mevcut POS sistemleri ile entegrasyon çalışması yapılıyor. Mobil uygulama ve kampanya yönetimi planlanıyor.", DaysAfter = 8 },
                new { Customer = "Renk Mobilya", Content = "Mobilya üretiminde 50 işçi ve 20.000 metrekare üretim alanı. Manuel planlama ve verimlilik sorunları var.", DaysAfter = 2 },
                new { Customer = "Renk Mobilya", Content = "Yeni üretim hattı kurulumu planlanıyor. Otomasyon ve dijitalleşme yatırımları yapılacak.", DaysAfter = 7 }
            };

            var assignedUsers = _users.Where(u => u.UserName == "mustafa.kaya@salesflow.com" || u.UserName == "zeynep.celik@salesflow.com" || u.UserName == "ali.sahin@salesflow.com").ToList();
            var userIndex = 0;

            foreach (var note in noteData)
            {
                var customer = _customers.FirstOrDefault(c => c.CompanyName == note.Customer);
                if (customer == null) continue;

                var baseDate = customer.CreatedDate.AddDays(note.DaysAfter);

                _notes.Add(new Note
                {
                    Content = note.Content,
                    CustomerId = customer.Id,
                    CreatedById = assignedUsers[userIndex % assignedUsers.Count].Id,
                    CreatedDate = baseDate,
                    UpdatedDate = baseDate.AddDays(_random.Next(0, 2)),
                    IsDeleted = false
                });
                userIndex++;
            }
            await _context.Notes.AddRangeAsync(_notes);
        }
        #endregion

        #region Activity Logs
        private async Task CreateActivityLogsAsync()
        {
            var activityLogs = new List<ActivityLog>();

            foreach (var customer in _customers)
            {
                var customerLeads = _leads.Where(l => l.CompanyName == customer.CompanyName).ToList();
                var customerDeals = _deals.Where(d => d.CustomerId == customer.Id).ToList();
                var customerMeetings = _meetings.Where(m => m.CustomerId == customer.Id).ToList();
                var customerTasks = _tasks.Where(t => t.CustomerId == customer.Id).ToList();

                // Lead activities
                foreach (var lead in customerLeads)
                {
                    activityLogs.Add(new ActivityLog
                    {
                        Action = ActivityAction.Create,
                        EntityName = "Lead",
                        EntityId = lead.Id,
                        UserId = lead.AssignedUserId,
                        Description = $"Yeni lead oluşturuldu: {lead.FirstName} {lead.LastName} - {lead.CompanyName}",
                        CreatedDate = lead.CreatedDate
                    });

                    if (lead.Status == LeadStatus.Qualified || lead.Status == LeadStatus.Converted)
                    {
                        activityLogs.Add(new ActivityLog
                        {
                            Action = ActivityAction.Update,
                            EntityName = "Lead",
                            EntityId = lead.Id,
                            UserId = lead.AssignedUserId,
                            Description = $"Lead {lead.Status} olarak güncellendi",
                            CreatedDate = lead.UpdatedDate.Value.AddDays(-2)
                        });
                    }
                }

                // Customer activities
                activityLogs.Add(new ActivityLog
                {
                    Action = ActivityAction.Create,
                    EntityName = "Customer",
                    EntityId = customer.Id,
                    UserId = customer.AssignedUserId,
                    Description = $"Müşteri oluşturuldu: {customer.CompanyName}",
                    CreatedDate = customer.CreatedDate
                });

                // Deal activities
                foreach (var deal in customerDeals)
                {
                    activityLogs.Add(new ActivityLog
                    {
                        Action = ActivityAction.Create,
                        EntityName = "Deal",
                        EntityId = deal.Id,
                        UserId = deal.AssignedUserId,
                        Description = $"Yeni fırsat oluşturuldu: {deal.Title} - {deal.Amount:C}",
                        CreatedDate = deal.CreatedDate
                    });

                    if (deal.Stage != DealStage.New)
                    {
                        activityLogs.Add(new ActivityLog
                        {
                            Action = ActivityAction.Update,
                            EntityName = "Deal",
                            EntityId = deal.Id,
                            UserId = deal.AssignedUserId,
                            Description = $"Fırsat {deal.Stage} aşamasına taşındı",
                            CreatedDate = deal.CreatedDate.AddDays(_random.Next(3, 10))
                        });
                    }
                }

                // Meeting activities
                foreach (var meeting in customerMeetings)
                {
                    activityLogs.Add(new ActivityLog
                    {
                        Action = ActivityAction.Create,
                        EntityName = "Meeting",
                        EntityId = meeting.Id,
                        UserId = meeting.AssignedUserId,
                        Description = $"Toplantı oluşturuldu: {meeting.Title}",
                        CreatedDate = meeting.CreatedDate
                    });

                    if (meeting.Status == MeetingStatus.Completed)
                    {
                        activityLogs.Add(new ActivityLog
                        {
                            Action = ActivityAction.Update,
                            EntityName = "Meeting",
                            EntityId = meeting.Id,
                            UserId = meeting.AssignedUserId,
                            Description = $"Toplantı tamamlandı: {meeting.Title}",
                            CreatedDate = meeting.EndDate
                        });
                    }
                }

                // Task activities
                foreach (var task in customerTasks)
                {
                    activityLogs.Add(new ActivityLog
                    {
                        Action = ActivityAction.Create,
                        EntityName = "Task",
                        EntityId = task.Id,
                        UserId = task.AssignedUserId,
                        Description = $"Yeni görev oluşturuldu: {task.Title}",
                        CreatedDate = task.CreatedDate
                    });

                    if (task.Status == TaskStatus.Completed)
                    {
                        activityLogs.Add(new ActivityLog
                        {
                            Action = ActivityAction.Update,
                            EntityName = "Task",
                            EntityId = task.Id,
                            UserId = task.AssignedUserId,
                            Description = $"Görev tamamlandı: {task.Title}",
                            CreatedDate = task.DueDate
                        });
                    }
                }
            }

            // Add user login activities
            foreach (var user in _users)
            {
                for (int i = 0; i < _random.Next(3, 8); i++)
                {
                    var loginDate = DateTime.UtcNow.AddDays(-_random.Next(1, 60));
                    activityLogs.Add(new ActivityLog
                    {
                        Action = ActivityAction.Login,
                        EntityName = "User",
                        EntityId = user.Id,
                        UserId = user.Id,
                        Description = $"Kullanıcı giriş yaptı: {user.FirstName} {user.LastName}",
                        CreatedDate = loginDate
                    });
                }
            }

            await _context.ActivityLogs.AddRangeAsync(activityLogs);
        }
        #endregion

        #region Attachments
        private async Task CreateAttachmentsAsync()
        {
            var attachmentData = new[]
            {
                new { Customer = "İstanbul Teknoloji A.Ş.", FileName = "ERP_Teknik_Sartname.pdf", FilePath = "/uploads/erp_teknik_sartname.pdf", ContentType = "application/pdf", Size = 2450000 },
                new { Customer = "İstanbul Teknoloji A.Ş.", FileName = "Entegrasyon_Diyagrami.png", FilePath = "/uploads/entegrasyon_diyagrami.png", ContentType = "image/png", Size = 1800000 },
                new { Customer = "Ankara Sağlık Çözümleri", FileName = "Hastane_Yönetim_Sistemi_Proje_Dokumani.pdf", FilePath = "/uploads/hastane_yonetim_proje.pdf", ContentType = "application/pdf", Size = 3200000 },
                new { Customer = "Ankara Sağlık Çözümleri", FileName = "Veri_Gizliligi_Protokolu.docx", FilePath = "/uploads/veri_gizliligi_protokolu.docx", ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document", Size = 850000 },
                new { Customer = "Ege Lojistik", FileName = "Filo_Yonetimi_Teklif.docx", FilePath = "/uploads/filo_yonetimi_teklif.docx", ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document", Size = 1250000 },
                new { Customer = "Ege Lojistik", FileName = "Rota_Optimizasyonu_Raporu.pdf", FilePath = "/uploads/rota_optimizasyonu_raporu.pdf", ContentType = "application/pdf", Size = 2100000 },
                new { Customer = "Güneş Enerji Sistemleri", FileName = "Enerji_Danismanlik_Sozlesmesi.pdf", FilePath = "/uploads/enerji_danismanlik_sozlesmesi.pdf", ContentType = "application/pdf", Size = 1650000 },
                new { Customer = "Marmara Gıda A.Ş.", FileName = "Tedarik_Zinciri_Analizi_Raporu.pdf", FilePath = "/uploads/tedarik_zinciri_analizi.pdf", ContentType = "application/pdf", Size = 2800000 },
                new { Customer = "Akdeniz Tekstil", FileName = "Stok_Yonetimi_Proje_Planı.docx", FilePath = "/uploads/stok_yonetimi_proje_planı.docx", ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document", Size = 950000 },
                new { Customer = "Kuzey İnşaat", FileName = "Proje_Yonetimi_Teklif.docx", FilePath = "/uploads/proje_yonetimi_teklif.docx", ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document", Size = 1350000 },
                new { Customer = "Batı Kimya", FileName = "Kalite_Kontrol_Oto_Planı.pdf", FilePath = "/uploads/kalite_kontrol_oto_planı.pdf", ContentType = "application/pdf", Size = 1950000 },
                new { Customer = "Güney Finans", FileName = "CRM_Sistemi_Proje_Sartnamesi.pdf", FilePath = "/uploads/crm_proje_sartnamesi.pdf", ContentType = "application/pdf", Size = 3100000 },
                new { Customer = "Güney Finans", FileName = "Finansal_Analiz_Raporu.xlsx", FilePath = "/uploads/finansal_analiz_raporu.xlsx", ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Size = 2200000 },
                new { Customer = "Kaya Hukuk", FileName = "Dava_Yonetim_Sistemi_Teklif.docx", FilePath = "/uploads/dava_yonetim_teklif.docx", ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document", Size = 780000 },
                new { Customer = "Ata Teknoloji", FileName = "Test_Platformu_Mimari_Diagrami.png", FilePath = "/uploads/test_platformu_mimari.png", ContentType = "image/png", Size = 1600000 },
                new { Customer = "Ceylan Enerji", FileName = "Karbon_Yonetim_Sistemi_Sartnamesi.pdf", FilePath = "/uploads/karbon_yonetim_sartnamesi.pdf", ContentType = "application/pdf", Size = 2400000 },
                new { Customer = "Tuna Lojistik", FileName = "Depo_Yonetimi_Teknik_Sartname.pdf", FilePath = "/uploads/depo_yonetimi_teknik.pdf", ContentType = "application/pdf", Size = 1850000 },
                new { Customer = "Karbon Tekstil", FileName = "Tedarik_Zinciri_Platformu_Teklif.docx", FilePath = "/uploads/tedarik_zinciri_teklif.docx", ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document", Size = 1150000 },
                new { Customer = "Renk Mobilya", FileName = "Verimlilik_Analiz_Raporu.pdf", FilePath = "/uploads/verimlilik_analiz_raporu.pdf", ContentType = "application/pdf", Size = 1550000 },
                new { Customer = "Nil Petrol", FileName = "Musteri_Sadakati_Programi_Taslak.pdf", FilePath = "/uploads/musteri_sadakati_taslak.pdf", ContentType = "application/pdf", Size = 1750000 }
            };

            foreach (var attachment in attachmentData)
            {
                var customer = _customers.FirstOrDefault(c => c.CompanyName == attachment.Customer);

                if (customer == null)
                    continue;

                _attachments.Add(new Attachment
                {
                    FileName = attachment.FileName,
                    FilePath = attachment.FilePath,
                    ContentType = attachment.ContentType,
                    FileSize = attachment.Size,

                    Customer = customer,

                    CreatedDate = customer.CreatedDate.AddDays(_random.Next(5, 30)),
                    UpdatedDate = customer.CreatedDate.AddDays(_random.Next(6, 31)),
                    IsDeleted = false
                });
            }

            await _context.Attachments.AddRangeAsync(_attachments);
        }
        #endregion

        #region Notifications
        private async Task CreateNotificationsAsync()
        {
            var notificationData = new[]
            {
                new { User = "mustafa.kaya@salesflow.com", Title = "Toplantı Hatırlatması", Message = "İstanbul Teknoloji A.Ş. ile 15:00'da keşif toplantısı var.", Type = NotificationType.Reminder, DaysAgo = 1 },
                new { User = "mustafa.kaya@salesflow.com", Title = "Görev Yaklaşıyor", Message = "ERP sistem demo ortam hazırlığının 2 günü kaldı.", Type = NotificationType.Reminder, DaysAgo = 2 },
                new { User = "ayse.yilmaz@salesflow.com", Title = "Yeni Fırsat", Message = "Güney Finans firmasından 420.000 TL'lik yeni fırsat oluşturuldu.", Type = NotificationType.Success, DaysAgo = 3 },
                new { User = "mustafa.kaya@salesflow.com", Title = "Toplantı İptal", Message = "Alfa Mühendislik ile planlanan toplantı iptal edildi.", Type = NotificationType.Warning, DaysAgo = 4 },
                new { User = "ayse.yilmaz@salesflow.com", Title = "Fırsat Aşaması Değişti", Message = "Ankara Sağlık Çözümleri fırsatı Kazanıldı olarak güncellendi.", Type = NotificationType.Success, DaysAgo = 5 },
                new { User = "zeynep.celik@salesflow.com", Title = "Yeni Lead", Message = "Yeni lead atandı: Murat Kurt - Ege Lojistik", Type = NotificationType.Info, DaysAgo = 1 },
                new { User = "ali.sahin@salesflow.com", Title = "Görev Tamamlandı", Message = "Karbon Tekstil için hazırlanan demo içeriği tamamlandı.", Type = NotificationType.Success, DaysAgo = 2 },
                new { User = "mustafa.kaya@salesflow.com", Title = "Müşteri Güncellemesi", Message = "Marmara Gıda A.Ş. müşteri bilgileri güncellendi.", Type = NotificationType.Info, DaysAgo = 3 },
                new { User = "zeynep.celik@salesflow.com", Title = "Toplantı Hatırlatması", Message = "Güneş Enerji Sistemleri ile 11:00'da online toplantı var.", Type = NotificationType.Reminder, DaysAgo = 1 },
                new { User = "ali.sahin@salesflow.com", Title = "Yeni Not", Message = "Akdeniz Tekstil firmasına yeni not eklendi.", Type = NotificationType.Info, DaysAgo = 2 },
                new { User = "ayse.yilmaz@salesflow.com", Title = "Kazanan Fırsat", Message = "Güney Finans fırsatı kazanıldı! Toplam 420.000 TL.", Type = NotificationType.Success, DaysAgo = 5 },
                new { User = "mustafa.kaya@salesflow.com", Title = "Görev Gecikmesi", Message = "Kuzey İnşaat proje yönetimi görevinde gecikme var.", Type = NotificationType.Warning, DaysAgo = 6 },
                new { User = "zeynep.celik@salesflow.com", Title = "Yeni Fırsat", Message = "Ceylan Enerji firmasından 350.000 TL'lik yeni fırsat oluşturuldu.", Type = NotificationType.Success, DaysAgo = 3 },
                new { User = "ali.sahin@salesflow.com", Title = "Müşteri Ziyareti", Message = "Renk Mobilya firmasına yarın saat 14:00'da ziyaret planlanıyor.", Type = NotificationType.Reminder, DaysAgo = 1 },
                new { User = "ayse.yilmaz@salesflow.com", Title = "Haftalık Rapor", Message = "Haftalık satış raporu hazırlandı. Bu hafta 2 yeni fırsat eklendi.", Type = NotificationType.Info, DaysAgo = 7 }
            };

            foreach (var notification in notificationData)
            {
                var user = _users.FirstOrDefault(u => u.UserName == notification.User);
                if (user == null) continue;

                _notifications.Add(new Notification
                {
                    Title = notification.Title,
                    Message = notification.Message,
                    Type = notification.Type,
                    IsRead = _random.NextDouble() > 0.3,
                    UserId = user.Id,
                    CreatedDate = DateTime.UtcNow.AddDays(-notification.DaysAgo),
                    UpdatedDate = DateTime.UtcNow.AddDays(-notification.DaysAgo),
                    IsDeleted = false
                });
            }
            await _context.Notifications.AddRangeAsync(_notifications);
        }
        #endregion
    }
}


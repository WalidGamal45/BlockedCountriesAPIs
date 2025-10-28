# 🌍 Blocked Countries API

🚫 **Blocked Countries API** هو مشروع .NET 8 Web API بسيط واحترافي لإدارة حظر الدول،  
مع دعم للحظر الدائم والمؤقت، وتخزين البيانات في ملف JSON، وتنظيف تلقائي (Background Service)،  
بالإضافة إلى البحث والتجزئة (Pagination + Filter) واستعلام الدولة من عنوان IP.

---

## 🚀 المميزات

✅ إضافة / حذف دولة من قائمة الحظر  
✅ دعم الحظر الدائم والمؤقت (بمدة محددة بالدقائق)  
✅ حفظ البيانات في ملف `blocked_countries.json` حتى بعد إغلاق البرنامج  
✅ خدمة خلفية (Background Service) تنظف الدول المنتهية تلقائيًا كل دقيقة  
✅ استعلام الدولة من خلال عنوان IP باستخدام موقع [ipapi.co](https://ipapi.co)  
✅ عرض النتائج مع **Pagination** و **Search / Filter**  
✅ واجهة Swagger جاهزة للتجربة  

---## 🧩 هيكل المشروع
BDAssignment/
│
├── Domain/
│ ├── Entities/
│ │ └── BlockedCountry.cs
│ └── Enums/
│ └── CountryBlockType.cs
│
├── Application/
│ ├── Interfaces/
│ │ ├── IGeoLookupService.cs
│ │ ├── ICountryBlockService.cs
│ │ └── IBlockedLogService.cs
│ ├── Models/
│ │ ├── BlockCountryRequestDto.cs
│ │ ├── TemporalBlockRequestDto.cs
│ │ └── IpLookupResultDto.cs
│ └── Services/
│ ├── CountryBlockService.cs
│ ├── GeoLookupService.cs
│ └── TestIpApiService.cs
│
├── Presentation/
│ ├── Controllers/
│ │ └── BlockedCountriesController.cs
│ ├── BackgroundServices/
│ │ └── TemporalBlockCleanupService.cs
│ ├── Program.cs
│ └── appsettings.json
│
└── blocked_countries.json ← بيانات الحظر محفوظة هنا

---

## ⚙️ الإعداد والتشغيل

### 🧱 المتطلبات:
- ✅ .NET 8 SDK  
- ✅ Visual Studio 2022 أو VS Code  
- ✅ اتصال إنترنت (لخدمة IP Lookup)

### ▶️ خطوات التشغيل:
1. افتح المشروع في Visual Studio  
2. اضغط **Run / F5**  
3. هتفتح صفحة Swagger تلقائيًا على:


---

## 🌐 الـ API Endpoints

| النوع | المسار | الوصف |
|-------|--------|--------|
| `POST` | `/api/BlockedCountries/block` | حظر دائم لدولة |
| `POST` | `/api/BlockedCountries/temporal-block` | حظر مؤقت لمدة معينة بالدقائق |
| `GET` | `/api/BlockedCountries/blocked` | عرض كل الدول المحظورة |
| `GET` | `/api/BlockedCountries/paged` | عرض مع Pagination وSearch |
| `DELETE` | `/api/BlockedCountries/block/{countryCode}` | إزالة دولة من الحظر |
| `GET` | `/api/BlockedCountries/ip/test?ipAddress=8.8.8.8` | استعلام بيانات دولة من IP |
| `GET` | `/api/BlockedCountries/ip/check-block?ipAddress=...` | التحقق هل الـ IP تابع لدولة محظورة |

---

## 🧠 أمثلة

### 📌 إضافة حظر دائم
```json
POST /api/BlockedCountries/block
{
"countryCode": "US",
"countryName": "United States"
}
⏳ إضافة حظر مؤقت
POST /api/BlockedCountries/temporal-block
{
  "countryCode": "EG",
  "countryName": "Egypt",
  "durationMinutes": 2
}
❌ حذف دولة من الحظر
DELETE /api/BlockedCountries/block/US
🌍 استعلام IP
GET /api/BlockedCountries/ip/test?ipAddress=8.8.8.8
⚡ خلف الكواليس

🧩 CountryBlockService
تتعامل مع الحظر (إضافة، حذف، تحديث، حفظ في JSON)

🧩 TemporalBlockCleanupService
تشغّل في الخلفية وتزيل الحظر المؤقت المنتهي تلقائيًا

🧩 GeoLookupService
تجلب معلومات الدولة من عنوان IP عبر ipapi.co

🧾 مثال من ملف JSON
[
  {
    "CountryCode": "US",
    "CountryName": "United States",
    "BlockType": "Permanent",
    "ExpiryDate": null
  },
  {
    "CountryCode": "EG",
    "CountryName": "Egypt",
    "BlockType": "Temporal",
    "ExpiryDate": "2025-10-28T21:55:00Z"
  }
]

🧠 أفكار لتطوير المستقبل

🔹 ربط بقاعدة بيانات SQL بدلاً من JSON

🔹 إنشاء لوحة تحكم (Dashboard) بواجهة Angular أو React

🔹 دعم حظر عناوين IP مباشرة

🔹 إضافة JWT Authentication لحماية الـ API

👨‍💻 المطور

Walid Gamal Amin
💼 Backend Developer (.NET)
📧 [Email Placeholder]
🌍 [GitHub Profile Link]

🏁 الرخصة

هذا المشروع مفتوح المصدر لأغراض تعليمية وتدريبية.



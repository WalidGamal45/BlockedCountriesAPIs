# Blocked Countries API

 هو مشروع
 .NET 8 Web API بسيط واحترافي لإدارة حظر الدول،  
مع دعم للحظر الدائم والمؤقت، وتخزين البيانات في ملف JSON، وتنظيف تلقائي (Background Service)،  
بالإضافة إلى البحث والتجزئة (Pagination + Filter) واستعلام الدولة من عنوان IP.

---

##  المميزات

1- إضافة / حذف دولة من قائمة الحظر  
2- دعم الحظر الدائم والمؤقت (بمدة محددة بالدقائق)  
3- حفظ البيانات في ملف `blocked_countries.json` حتى بعد إغلاق البرنامج  
4- خدمة خلفية (Background Service) تنظف الدول المنتهية تلقائيًا كل دقيقة  
5- استعلام الدولة من خلال عنوان IP باستخدام موقع [ipapi.co](https://ipapi.co)  
6- عرض النتائج مع **Pagination** و **Search / Filter**  
7- واجهة Swagger جاهزة للتجربة 
<hr>

-----> ##  هيكل المشروع 
 BDAssignment   


1- Domain ---> ( Entities , Enums ) <br>
2- Application ---> ( Interfaces , Services , DTOs ) <br>
3- Presentation ---> ( Controllers , Program.cs , appsettings.json , BackgroundServices ,  blocked_countries.json ← بيانات الحظر محفوظة هنا)

---

##  الإعداد والتشغيل

###  المتطلبات:
-  .NET 8 SDK  
-  Visual Studio 2022 أو VS Code  
-  اتصال إنترنت (لخدمة IP Lookup)

###  خطوات التشغيل:
1. افتح المشروع في Visual Studio  
2. اضغط **Run / F5**  
3. هتفتح صفحة Swagger تلقائيًا على:


---

##  الـ API Endpoints

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



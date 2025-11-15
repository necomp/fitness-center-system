# Fitness Center System

ASP.NET Core MVC ile Spor Salonu (Fitness Center) Yönetim ve Randevu Sistemi.

## Projenin Amacı
- Spor salonlarının sunduğu hizmetlerin (fitness, yoga vb.) ve antrenör bilgilerinin kolayca yönetilmesi.
- Üyelerin uygun antrenörler ve saatler üzerinden online randevu alabilmesi.
- Yapay zekâ entegrasyonu ile kişiselleştirilmiş egzersiz ve diyet planı önerileri sunulması.

## Kullanılması Planlanan Teknolojiler
-   ASP.NET Core MVC (Güncel LTS)
-   HTML5, CSS3, Bootstrap 5, JavaScript, jQuery
-   SQL Server / PostgreSQL
-   Entity Framework Core
-   Yapay Zekâ Entegrasyonu (OpenAI API)

## Özellikler

1.  **Spor Salonu Tanımlamaları**
    * Hizmet türleri (fitness, pilates, yoga vb.), süreleri ve ücretleri.
    * Salonun çalışma saatlerinin düzenlenmesi.

2.  **Antrenör Yönetimi**
    * Antrenörlerin uzmanlık alanları (kilo verme, kas geliştirme vb.).
    * Her antrenörün kendi müsaitlik takvimini ve programını yönetmesi.
    * Antrenör ekleme, çıkarma, güncelleme işlemleri.

3.  **Üye ve Randevu Sistemi**
    * Üyelerin) online randevu alabileceği sistem.
    * Randevu çakışma kontrolleri.
    * Randevu onaylama ve iptal mekanizması.

4.  **REST API**
    * LINQ kullanılarak veri sorgulama (Örn: Belirli tarihteki uygun antrenörler, üye randevuları).
    * En az bir modülde API üzerinden veri iletişimi.

5.  **Yapay Zekâ Entegrasyonu**
    * Kullanıcının fotoğraf yüklemesi veya boy/kilo/hedef bilgisi girmesi.
    * Bu bilgilere göre kişiye özel egzersiz veya diyet planı önerileri alınması.

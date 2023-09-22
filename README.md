# TelephoneDirectory
Telefon rehberi uygulaması

# Açıklama
TelephoneDirectory, telefon numaraları dizinini yönetmek için kullanılan mikro hizmet tabanlı bir uygulamadır. PersonContact ve ContactReport olmak üzere 2 mikroservisten oluşuyor.

## PersonContact servisinde;

- Kişi oluşturma
- Kişi silme
- Kişiye iletişim bilgisi ekleme (Telefon numarası, mail adresi ve lokasyon)
- Kişiden iletişim bilgisi kaldırma
- Oluşturulan kişilerin listelenmesi
- Oluşturulan bir kişinin detay yani iletişim bilgileri ile getirilmesi
- Lokasyona bağlı unique olarak kaç kişinin kayıtlı olduğu, ve bu unique kişiler üzerinden kaç kişinin Telefon numarası bilgisini eklediği görülmektedir. 

### PersonContact Notları
- Bir kişi kaydı oluşturduktan sonra, GET kısmından kişi Id ‘si ile birlikte kişiyi oluştururken belirtmiş olduğunuz diğer bilgileri görebilirsiniz.
- Bu Id iler birlikte kişiye iletişim bilgisi ekleyebilirsiniz. Kayıt olmuş kişilere iletişim bilgisi eklenebilir.  Sınırsız kayıt eklenebilir.
- ”../person/{id}” kısmında kişi Id ‘sini vererek mevcutta ki iletişim bilgileri ile birlikte kayıtları görebilirsiniz. Burada kişiye eklenen iletişim bilgilerinin Id ‘leri de olacaktır. Bu Id’ leri “DELETE - ../personcontactinfo” ile kişiden silebilirsiniz.
- Yine kişi Id ‘si ile birlikte kişi silinebilir.
- Kişilere iletişim bilgisi olarak girmiş olduğunuz lokasyon bilgisi (“Ankara”, “Paris” ,”Bolu”) üzerinden yukarıda açıkladığım şekilde rapor isteği oluşturulabilir. Burada cevap “Hazırlanıyor” olarak bir cevap ve Rapor isteği de görünecektir.
- Bir kişi için sınırsız sayıda iletişim bilgisi girilebileceği için ( Mesela, bir kişi Id 'sine 10 tane Ankara lokasyonu) lokasyonda ki unique kişi sayısını hesaplanıp, bu rakam verildi. Lokasyonda ki kayıtlı telefon numara sayısı da bu unique mantığı ile hesaplanıp yansıtıldı.
- 
## ContactReport servisinde;

- Sistemin oluşturduğu raporların listelenmesi. 

### ContactReport Notları
- Bütün raporları GET kısmında detaylar ile birlikte görebilirsiniz. Burada “Tamamlandı” olarak rapor statüsü dönecektir. Ayrıca Raporun istek tarih-saati ile birlikte raporun oluşma tarih-saati de görünecektir. Bütün raporlar gelirken raporun istek saatine göre güncel saatten geçmişe yani “Sort By Descending” şeklinde bir sıralama olacaktır.
- Bu listeden alacağınız rapor Id ‘si ile de "GET - ../reports/{id}” kısmında yine o raporun detayını görebilirsiniz.
- Rapor istekleri mesaj kuyruğu(RabbitMq) ile işlenmektedir.

## Teknolojiler
- MongoDB
- .Net 7
- RabbitMq
- Docker

## Uygulamanın çalışması

Uygulamayı başlatmak için projenin kök dizininde;
docker-compose up

komutunu çalıştırın. Sonrasında uygulamayı çalıştırabilirsiniz. Uygulama .Net 7 ile Visual Studi For Mac IDE 'si ile geliştirildi. 

PersonContact API Swagger UI: http://localhost:5011/swagger
ContactReport API Swagger UI: http://localhost:5012/swagger
RabbitMQ GUI: http://localhost:15672 (Username:guest Password:guest)

# KayraExport.ECommerce

Mikroservis tabanlı e-ticaret backend uygulaması.

**Teknolojiler:** .NET 8, Onion Architecture, CQRS/MediatR, ASP.NET Core Identity + JWT, YARP API Gateway, MassTransit + RabbitMQ, Redis, SQL Server, Serilog + Seq, Docker Compose

**Kod Deposu:** [GitHub — KayraExport.ECommerce](<repository-url>)

---

## Kurulum ve Çalıştırma

### Ön Gereksinimler

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (Docker Compose dahil)
- Git

### 1. Repository'yi Klonlayın

```bash
git clone <repository-url>
cd KayraExport.ECommerce
```

### 2. Environment Dosyasını Hazırlayın

Projede `.env.example` dosyası bulunur. Bunu `.env` olarak kopyalayın ve kendi ortamınıza göre düzenleyin:

```bash
cp .env.example .env
```

`.env` dosyasındaki JWT secret, SQL Server şifresi gibi hassas değerleri kendi değerlerinizle güncelleyin. Varsayılan değerler lokal geliştirme için çalışır durumda bırakılmıştır ancak production ortamında mutlaka değiştirilmelidir.

### 3. HTTPS Geliştirme Sertifikası Oluşturun

API Gateway HTTPS ile çalıştığı için .NET dev-certs ile geliştirme sertifikası oluşturmanız gerekir:

```bash
dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p YourCertPassword
dotnet dev-certs https --trust
```

> **Windows PowerShell:**
> ```powershell
> dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p YourCertPassword
> dotnet dev-certs https --trust
> ```

`.env` dosyasındaki sertifika şifresi (`ASPNETCORE_Kestrel__Certificates__Default__Password`) bu adımda belirlediğiniz şifre ile eşleşmelidir.

### 4. Tüm Servisleri Başlatın

```bash
docker compose up -d --build --force-recreate
```

Bu komut API Gateway, Auth Service, Product Service, Log Service ve tüm altyapı container'larını birlikte ayağa kaldırır. Veritabanı migration'ları her servis başlatıldığında otomatik olarak uygulanır.

### 5. Çalıştığını Doğrulayın

**Health Check:**
```bash
curl http://localhost:5000/health
```

**Swagger UI** (tüm servislerin API dokümantasyonu tek noktada):
```
http://localhost:5000/swagger
```

**Seq Log Arayüzü** (merkezi log izleme):
```
http://localhost:8081
```

**RabbitMQ Management:**
```
http://localhost:15672
```

---

## Dağıtım (Deployment)

Proje tamamen Docker Compose ile containerize edilmiştir. Production ortamına dağıtım için:

1. `.env` dosyasındaki tüm değerleri production değerleriyle güncelleyin (güçlü şifreler, production JWT secret vb.)
2. `docker-compose.override.yml` dosyası yalnızca geliştirme ortamına özeldir (dev-certs mounting); production'da kullanılmamalıdır.
3. Servisleri başlatın:

```bash
docker compose -f docker-compose.yml up -d --build
```

Her servis stateless tasarlanmıştır; yatay ölçekleme için instance sayısı artırılabilir:

```bash
docker compose up -d --scale product-service=3
```
<a href="#"><img width="100%" height="auto" src="https://ptf.unze.ba/wp/wp-content/uploads/2018/02/Logo-PTF018.png"/></a>

<h1 align="center">💪 Informacioni sistem teretane</h1>
<h3 align="center">Razvoj informacijskih sistema (VI semestar) — Politehnički fakultet UNZE</h3>

<p align="center">
  <a href="https://dotnet.microsoft.com/"><img alt=".NET" src="https://img.shields.io/badge/.NET-6+-purple?style=flat-square&logo=dotnet"></a>
  <a href="https://www.microsoft.com/sql-server"><img alt="SQL Server" src="https://img.shields.io/badge/SQL_Server-2022-red?style=flat-square&logo=microsoftsqlserver"></a>
  <a href="https://www.docker.com/"><img alt="Docker" src="https://img.shields.io/badge/Docker-Compose-blue?style=flat-square&logo=docker"></a>
</p>

---

## 👨‍🎓 Autor

- **Ensar Krehmić**  
- 📧 [ensar.krehmic.22@size.ba](mailto:ensar.krehmic.22@size.ba)

## 👨‍🏫 Mentorstvo

- Profesor: **dr. sc. Denis Čeke**  
  📧 [denis.ceke@unze.ba](mailto:denis.ceke@unze.ba)

- Asistent: **Ehlimana Krupalija**  
  📧 [ehlimana.krupalija@unze.ba](mailto:ehlimana.krupalija@unze.ba)

---

## 📌 Opis projekta

Ovaj projekat predstavlja **informacioni sistem za teretanu** koji omogućava digitalizaciju i optimizaciju poslovanja fitnes centara.  
Sistem povezuje **goste, klijente, trenere i zaposlene** kroz centralizovanu platformu.  

Cilj sistema je da:  
- ✅ olakša evidenciju članova i njihovih članarina  
- ✅ omogući zakazivanje i upravljanje treninzima  
- ✅ pruži transparentan uvid u finansije i zalihe  
- ✅ omogući personalizovan pristup treninzima i planovima ishrane  

---

## 🛠 Tehnologije

- **Backend & Frontend**: ASP.NET Core (C#), Razor Pages, Tailwind CSS / Bootstrap  
- **ORM**: Entity Framework Core  
- **Baza podataka**: Microsoft SQL Server  
- **Mobilna aplikacija (prototip)**: .NET MAUI  
- **Deploy**: Docker & Docker Compose  

---

## 🚀 Funkcionalnosti

👤 **Gost**  
- Registracija i prijava  
- Pregled javnih informacija o teretani i cjenovniku  

💪 **Klijent**  
- Upravljanje članarinom  
- Rezervacija termina za treninge  
- Pregled plana treninga i napretka  

👨‍💻 **Zaposlenik**  
- Upravljanje korisnicima i članarinama  
- Administracija termina i rezervacija  
- Evidencija uplata i izdavanje računa  

🏋️ **Trener**  
- Kreiranje i upravljanje treninzima  
- Dodjela individualnih i grupnih planova  
- Praćenje rezultata i napretka klijenata  

---

## ⚙️ Zahtjevi sistema

- .NET 6 ili noviji  
- SQL Server 2022  
- Docker i Docker Compose  
- Git za verzionisanje  

---

## ▶️ Upute za pokretanje

Za jednostavno pokretanje aplikacije koristi se **docker-compose**:  

```bash
docker-compose up --build

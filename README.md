# Parking Lot Management System

Parking Lot Management System është një aplikacion i zhvilluar me ASP.NET MVC
dhe Entity Framework Core, i krijuar për të thjeshtuar dhe automatizuar menaxhimin e një parkingu. 
Sistemi synon të ofrojë një përvojë efikase për administrimin e automjeteve, abonimeve dhe tarifave,
duke kombinuar performancën me lehtësinë e përdorimit.

Ky projekt përfaqëson një zgjidhje të skalueshme për mjedise urbane dhe komerciale,
duke përfshirë module të dedikuara për monitorim, analizë dhe raportim.

## Funksionalitete kryesore

- Regjistrim automatik i hyrjes dhe daljes së automjeteve
- Menaxhim fleksibël i abonentëve dhe planëve tarifore
- Llogaritje dinamike e çmimeve bazuar në kohën e qëndrimit
- Gjenerim i raporteve statistikore mbi trafikun dhe të ardhurat
- Autorizim përdoruesish me role të ndara për kontroll të sigurisë

## Struktura e databazës

Sistemi mbështetet në një skemë databaze të organizuar me tabela specifike për çdo funksionalitet kryesor:

| Emri i tabelës        | Përshkrimi                                                  |
|-----------------------|--------------------------------------------------------------|
| `ParkingSpots`        | Vendparkimet dhe statusi i tyre (i lirë / i zënë)           |
| `PricingPlans`        | Tarifat dhe kushtet e planit të zgjedhur                    |
| `Subscribers`         | Të dhënat personale të përdoruesve me abonim               |
| `Subscriptions`       | Marrëveshjet e abonimeve me datat përkatëse                |
| `Logs`                | Regjistrime të veprimeve të sistemit për auditim           |
| `__EFMigrationsHistory` | Historiku i migrimeve të Entity Framework                |

Struktura është ndërtuar me relacione logjike ndërmjet tabelave dhe mbështet migrime automatike për përditësim efikas.

### Skenarë testimi:

- Simulo hyrje dhe dalje të një automjeti
- Krijo një abonim dhe llogarit tarifën përkohore
- Gjenero raport statistikor për një javë aktivitet
- Testo sjelljen në skenarë jo-standardë si abonim i skaduar ose hyrje pa dalje

## Deploy & përdorim

Sistemi mund të përgatitet për shpërndarje duke përdorur Docker ose hostim në ambient cloud. Siguria menaxhohet përmes autentikimit me role dhe dokumentacioni i API-ve mund të zgjerohet për bashkëpunim me zhvillues të tjerë.

## Autore

Ky aplikacion është konceptuar dhe zhvilluar nga Melisa dhe Armando.

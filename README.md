# Wymagania:

Aplikacja na potrzeby kompilacji i uruchomienia wymaga środowiska .NET 8 (.NET SDK).

Pakiet .NET SDK można pobrać ze strony [https://dotnet.microsoft.com/en-us/download/dotnet/8.0](<https://dotnet.microsoft.com/en-us/download/dotnet/8.0>).

W skład aplikacji wchodzą następujące projekty: • ExchangeRate.WebService – projekt zawiera główny serwis aplikacji działający w oparciu o platformę .NET Core. • ExchangeRate.WebService.IntegrationTests – projekt testów integracyjnych weryfikujących działanie głównego endpointu aplikacji /api/v1/Providers/{provider}/rates • ExchangeRate.Blazor – projekt zawierający interfejs graficzny przygotowany z wykorzystaniem technologii Blazor oraz Bootstrap.

Na potrzeby testowania lokalnego serwis ExchangeRate.WebService domyślnie zostaje uruchomiony pod adresem [https://localhost:7064](<https://localhost:7064>). Aplikacja kliencka, tj. ExchangeRate.Blazor domyślnie zostaje uruchomiona lokalnie pod adresem [https://localhost:7286/](<https://localhost:7286/>).

Serwis uruchomiony w trybie deweloperskim umożliwia uruchomienie interfejsu Swagger UI pozwalającego na zapoznanie się z dokumentacją udostępnionych endpointów oraz pozwala na przetestowanie działania REST API. Swagger UI dostępny jest pod adresem [https://localhost:7064/swagger](<https://localhost:7064/swagger>). Dokumentacja serwisu zgodna ze standardem OpenAPI dostępna jest pod adresem [https://localhost:7064/swagger/v1/swagger.json](<https://localhost:7064/swagger/v1/swagger.json>).

Inne adresy, pod którymi uruchamiany jest serwis, takie jak niezabezpieczony adres HTTP [http://localhost:5038/](<http://localhost:5038/>) można odszukać w pliku ExchangeRate.WebService\\Properties\\launchSettings.json.

Analogicznie dla interfejsu graficznego alternatywne adresy udostępniania aplikacji znajdują się w katalogu ExchangeRate.Blazor\\Properties\\launchSettings.json.

W tym miejscu można również dokonać niezbędnej zmiany portu, jeśli zajdzie taka potrzeba.

Uwaga: Do obsługi żądań poprzez protokół HTTPS konieczne jest, aby na stanowisku był zainstalowany tzw. certyfikat deweloperski dla localhost. Certyfikat można wygenerować lokalnie za pomocą polecenia:

bash

```
dotnet dev-certs https --trust
```

Kompilacja i uruchomienie:

W katalogu głównym dla obu projektów znajduje się plik rozwiązania ExchangeRateService.sln należy otworzyć go w programie Visual Studio 2022 lub nowszym, a następnie uruchomić projekt. Otworzą się dwa niezależne okna przeglądarki, jedno dla ExchangeRate.WebService a drugie dla ExchangeRate.Blazor.

Alternatywnie można skompilować i uruchomić obie aplikacje niezależnie z wiersza poleceń bez potrzeby wykorzystania Visual Studio. Z poziomu katalogu głównego aplikacji należy wykonać następujące komendy w trybie wiersza poleceń.

bash

```
dotnet run -c Debug --project ExchangeRate.WebService\ExchangeRate.WebService.csproj --urls  https://localhost:7064
```

oraz

bash

```
dotnet run -c Debug --project ExchangeRate.Blazor\ExchangeRate.Blazor.csproj --urls  https://localhost:7286
```

alternatywnie można uruchomić testy integracyjne poprzez wykonanie polecenia (wymagany dostęp do internetu)

bash

```
dotnet test ExchangeRate.WebService.IntegrationTests\ExchangeRate.WebService.IntegrationTests.csproj
```

Uwaga: Uruchomienie serwisu oraz aplikacji klienckiej poprzez dotnet run wymusza również manualne otwarcie okna przeglądarki i przejścia pod wskazane adresy, na których działają serwisy.

Dodatkowo w podkatalogach ExchangeRate.WebService i ExchangeRate.Blazor znajduje się plik appsettings.json, w którym możemy dokonać niezbędnych konfiguracji. Aplikacja nie wykorzystuje interfejsów IOptionsMonitor\<T\> i IOptionsSnapshot\<T\>, a każda zmiana w pliku konfiguracyjnym wymaga przeładowania usługi.

Plik ExchangeRate.WebService\\appsettings.json umożliwia konfigurację np. sposobu wyświetlania daty, kolejności sortowania walut oraz katalogu zapisu logów aplikacji. Odpowiadają za to odpowiednio wartości DateFormat, CurrencySort, LogFilePath. W pliku appsettings.json aplikacji klienckiej Blazor znajduje się wartość ServiceUrl, która pozwala skonfigurować lub zmienić domyślny adres, pod którym udostępniony został serwis.

Uwaga: Aplikacja kliencka wymaga dostępu do internetu w celu pobrania arkuszy stylów CSS dla biblioteki Bootstrap oraz ikon z serwerów CDN. Wykorzystywane zasoby CDN:

HTMLZawijaj wierszeSkopiuj kodPodgląd

```
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
        rel="stylesheet"
        integrity="sha384-sRIl4kxILFvY47J16cr9ZwB07vP4J8+LH7qKQnuqkuIAvNWLzeN8tE5YBujZqJLB"
        crossorigin="anonymous" />

  <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.13.2/font/bootstrap-icons.min.css"
        integrity="sha256-ecmcGh4Ok3+AilgWVyi37QCADwLvXvz80Q3+cUZOZ10="
        crossorigin="anonymous">
```
dotnet dev-certs https --trust


Kompilacja i uruchomienie:

W katalogu głównym dla obu projektów znajduje się plik rozwiązania ExchangeRateService.sln należy otworzyć go w programie Visual Studio 2022 lub nowszym, a następnie uruchomić projekt. Otworzą się dwa niezależne okna przeglądarki, jedno dla ExchangeRate.WebService a drugie dla ExchangeRate.Blazor.

Alternatywnie można skompilować i uruchomić obie aplikacje niezależnie z wiersza poleceń bez potrzeby wykorzystania Visual Studio. Z poziomu katalogu głównego aplikacji należy wykonać następujące komendy w trybie wiersza poleceń.

bash
dotnet run -c Debug --project ExchangeRate.WebService\ExchangeRate.WebService.csproj --urls  https://localhost:7064


oraz

bash
dotnet run -c Debug --project ExchangeRate.Blazor\ExchangeRate.Blazor.csproj --urls  https://localhost:7286


alternatywnie można uruchomić testy integracyjne poprzez wykonanie polecenia (wymagany dostęp do internetu)

bash
dotnet test ExchangeRate.WebService.IntegrationTests\ExchangeRate.WebService.IntegrationTests.csproj


Uwaga: Uruchomienie serwisu oraz aplikacji klienckiej poprzez dotnet run wymusza również manualne otwarcie okna przeglądarki i przejścia pod wskazane adresy, na których działają serwisy.

Dodatkowo w podkatalogach ExchangeRate.WebService i ExchangeRate.Blazor znajduje się plik appsettings.json, w którym możemy dokonać niezbędnych konfiguracji. Aplikacja nie wykorzystuje interfejsów IOptionsMonitor<T> i IOptionsSnapshot<T>, a każda zmiana w pliku konfiguracyjnym wymaga przeładowania usługi.

Plik ExchangeRate.WebService\appsettings.json umożliwia konfigurację np. sposobu wyświetlania daty, kolejności sortowania walut oraz katalogu zapisu logów aplikacji. Odpowiadają za to odpowiednio wartości DateFormat, CurrencySort, LogFilePath. W pliku appsettings.json aplikacji klienckiej Blazor znajduje się wartość ServiceUrl, która pozwala skonfigurować lub zmienić domyślny adres, pod którym udostępniony został serwis.

Uwaga: Aplikacja kliencka wymaga dostępu do internetu w celu pobrania arkuszy stylów CSS dla biblioteki Bootstrap oraz ikon z serwerów CDN. Wykorzystywane zasoby CDN:

HTML
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
        rel="stylesheet"
        integrity="sha384-sRIl4kxILFvY47J16cr9ZwB07vP4J8+LH7qKQnuqkuIAvNWLzeN8tE5YBujZqJLB"
        crossorigin="anonymous" />

  <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.13.2/font/bootstrap-icons.min.css" 
        integrity="sha256-ecmcGh4Ok3+AilgWVyi37QCADwLvXvz80Q3+cUZOZ10=" 
        crossorigin="anonymous">
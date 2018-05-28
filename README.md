# HttpHelper

Вспомогательная библиотека для работы с http-запросами.

```csharp
using HttpHelper;

string response = await new HttpRequest()
                .MethodAdd(HttpMethod.Post)
                .UrlAdd("http://clc.to")
                .ActionAdd("V85cWg")
                .TimeOut(10)
                .SendAsync();
```

Попытка реализовать паттерн Builder
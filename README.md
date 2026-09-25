# AccessDoor

Login shell for the EPSB AccessDoor web portal. The user enters the portal URL, a username and a key;
the app opens `…?username=…&key=…&token=<device id>` in an embedded browser and reads the server's
`msg` / `IsAllow` redirect to decide whether access was granted.

## Projects (.NET 10)

| Path | What | Replaces |
|---|---|---|
| `src/AccessDoor.Core` | Shared login URL building/validation and result parsing | duplicated logic in each app |
| `src/AccessDoor.Mobile` | .NET MAUI app for Android and iOS | Xamarin.Forms 4.4 (`AccessDoor/`) — out of support since May 2024 |
| `src/AccessDoor.Desktop` | WinForms app on .NET 10 using Microsoft Edge WebView2 | `AccessDoorWinforms/` (.NET Framework 4.7.2 + CefSharp 79) and `Epsb AccessDoor (Desktop)/` (WPF + commercial EO.WebBrowser) |
| `tests/AccessDoor.Core.Tests` | xUnit v3 tests for the core logic | — |

Open `AccessDoor.slnx` (Visual Studio 2026 / Rider / `dotnet` CLI).

```sh
dotnet test tests/AccessDoor.Core.Tests
dotnet build src/AccessDoor.Desktop                       # Windows; needs the WebView2 runtime to run
dotnet workload install maui-android maui-ios
dotnet build src/AccessDoor.Mobile -f net10.0-android
```

## Compatibility notes

- **Device tokens are kept.** Desktop still sends the lower-cased CPU `ProcessorId`; Android reuses the
  `my_id` value stored by the old app. New Android installs use `ANDROID_ID` (the old `Build.Serial`
  is unreadable on Android 10+); iOS uses `identifierForVendor`.
- The Android package id `AccessDoor.AccessDoor` and iOS bundle id `com.companyname.AccessDoor` are
  unchanged, so the new app installs as an update.
- Query values are now URL-escaped, so usernames/keys containing `&`, `=`, spaces, etc. work.
- The login response must carry `IsAllow=true`; a page without a result (wrong URL, captive portal)
  is rejected. After a confirmed login, further page navigation is not checked.

## Security to-dos

- The server is plain HTTP on a bare IP, so the key travels unencrypted. Cleartext is now allowed only for
  `103.82.228.86` on Android (instead of everywhere) and only for web content on iOS. Move the portal to
  HTTPS and remove those exceptions.
- Sending the key in the query string leaks it into server/proxy logs and browser history; a POST-based
  login would be better.

## Legacy code

`AccessDoor/`, `AccessDoorWinforms/`, `Epsb AccessDoor (Desktop)/` and `AccessDoor.sln` are the original
projects, left in place for reference. Delete them once the new apps are verified.

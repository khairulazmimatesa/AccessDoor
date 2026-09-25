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
dotnet test --project tests/AccessDoor.Core.Tests
dotnet build src/AccessDoor.Desktop                       # Windows; needs the WebView2 runtime to run
dotnet workload install maui-android maui-ios
dotnet build src/AccessDoor.Mobile -f net10.0-android
```

## Dependencies

NuGet versions are pinned in `Directory.Packages.props` (Central Package Management). `global.json` pins
the SDK (patch updates only). CI installs a pinned workload set (`WORKLOAD_VERSION` in the workflows:
MAUI 10.0.20, Android 36.1.69, iOS 26.5.10318); install the same locally with
`dotnet workload install maui-android maui-ios --version 10.0.401`. The `Microsoft.Maui.Controls` package
(10.0.110) may be newer than the workload's MAUI (10.0.20): MAUI servicing releases ship on NuGet before
they reach a workload set. It must never be older than the workload's MAUI. To upgrade, change the version there; project files carry no versions.

## Signed iOS builds

`.github/workflows/ios-release.yml` produces a signed `.ipa` (uploaded as a workflow artifact) when a
`v*` tag is pushed or the workflow is run manually. It needs four repository secrets, listed at the top
of that file: the distribution certificate (.p12, base64) and its password, the provisioning profile for
`com.companyname.AccessDoor` (base64), and the certificate name.

## Compatibility notes

- **Device tokens are kept.** Desktop still sends the lower-cased CPU `ProcessorId`; Android reuses the
  `my_id` value stored by the old app. New Android installs use `ANDROID_ID` (the old `Build.Serial`
  is unreadable on Android 10+); iOS uses `identifierForVendor`.
- The Android package id `AccessDoor.AccessDoor` and iOS bundle id `com.companyname.AccessDoor` are
  unchanged, so the new app installs as an update.
- Query values are now URL-escaped, so usernames/keys containing `&`, `=`, spaces, etc. work.
- The login response must carry `IsAllow=true`; a page without a result (wrong URL, captive portal)
  is rejected. After a confirmed login, further page navigation is not checked.

## Stored data

- The last successful login URL (which contains the key) is kept so the app can resume the session.
  Mobile stores it in the platform keystore/keychain (`SecureStorage`); desktop encrypts it with
  Windows DPAPI for the current user (`%LocalAppData%\AccessDoor\session.bin`).
- Logging out deletes it and clears the embedded browser's cookies (desktop) or cookies, web storage
  and caches (mobile).

## Security to-dos

- The server is plain HTTP on a bare IP, so the key travels unencrypted. Cleartext is now allowed only for
  `103.82.228.86` on Android (instead of everywhere) and only for web content on iOS. Move the portal to
  HTTPS and remove those exceptions.
- Sending the key in the query string leaks it into server/proxy logs and browser history; a POST-based
  login would be better.

## Legacy code

The original Xamarin.Forms, .NET Framework WinForms and WPF projects were removed; they remain in git
history before this change.

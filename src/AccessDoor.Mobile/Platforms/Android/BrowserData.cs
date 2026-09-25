using Android.Webkit;

namespace AccessDoor.Mobile.Services;

internal static partial class BrowserData
{
    public static partial Task ClearAsync()
    {
        var done = new TaskCompletionSource();
        var cookies = CookieManager.Instance;
        if (cookies is null)
            return Task.CompletedTask;

        cookies.RemoveAllCookies(new Callback(done));
        cookies.Flush();
        WebStorage.Instance?.DeleteAllData();
        return done.Task;
    }

    private sealed class Callback(TaskCompletionSource done) : Java.Lang.Object, IValueCallback
    {
        public void OnReceiveValue(Java.Lang.Object? value) => done.TrySetResult();
    }
}

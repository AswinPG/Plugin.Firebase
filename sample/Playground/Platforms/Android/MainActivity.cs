using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Plugin.Firebase.Auth;
using Plugin.Firebase.CloudMessaging;
using Plugin.Firebase.DynamicLinks;

namespace Playground;

// Activity attribute is not needed since MainActivity gets registered
// in AndroidManifest.xml because of the Firebase Dynamic Link feature
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        HandleIntent(Intent);
        CreateNotificationChannelIfNeeded();
    }

    private static void HandleIntent(Intent intent)
    {
        FirebaseCloudMessagingImplementation.OnNewIntent(intent);
        FirebaseDynamicLinksImplementation.HandleDynamicLinkAsync(intent).Ignore();
    }
    
    private void CreateNotificationChannelIfNeeded()
    {
        if(Build.VERSION.SdkInt >= BuildVersionCodes.O) {
            CreateNotificationChannel();
        }
    }

    private void CreateNotificationChannel()
    {
        var channelId = $"{PackageName}.general";
        var notificationManager = (NotificationManager) GetSystemService(NotificationService);
        var channel = new NotificationChannel(channelId, "General", NotificationImportance.Default);
        notificationManager.CreateNotificationChannel(channel);
        FirebaseCloudMessagingImplementation.ChannelId = channelId;
        FirebaseCloudMessagingImplementation.SmallIconRef = Resource.Drawable.ic_push_small;
    }
    
    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
    {
        Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }
    
    protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
    {
        base.OnActivityResult(requestCode, resultCode, data);
        FirebaseAuthImplementation.HandleActivityResultAsync(requestCode, resultCode, data);
    }
    
    protected override void OnNewIntent(Intent intent)
    {
        base.OnNewIntent(intent);
        HandleIntent(intent);
    }
}


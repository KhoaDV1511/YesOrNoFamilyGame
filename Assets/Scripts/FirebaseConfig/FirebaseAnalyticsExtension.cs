#if !UNITY_WEBGL
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.DynamicLinks;
#endif
#if UNITY_WEBGL 
using System.Runtime.InteropServices;
#endif
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Object = System.Object;


public class FirebaseAnalyticsExtension : MonoBehaviour
{
    private static FirebaseAnalyticsExtension _Instance = null;

    const string KEY = "AIzaSyB9==";

    public static FirebaseAnalyticsExtension Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = FindObjectOfType<FirebaseAnalyticsExtension>();
                if (!_Instance)
                {
                    _Instance = new GameObject("Firebase Extension").AddComponent<FirebaseAnalyticsExtension>();
                }

                DontDestroyOnLoad(_Instance);
            }

            return _Instance;
        }
    }

#if !UNITY_WEBGL 
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
#endif

    public static string DeepLinkQuery = "";
    public static string CacheUsedQueryLink = "";

    private void Awake()
    {
#if !UNITY_WEBGL
        DynamicLinks.DynamicLinkReceived += (sender, args) =>
        {
            var dynamicLinkEventArgs = args as ReceivedDynamicLinkEventArgs;
            Debug.LogFormat("Received dynamic link {0}",
                dynamicLinkEventArgs.ReceivedDynamicLink.Url.Query + "   " + "CacheUsedLink: " + CacheUsedQueryLink);
            var currentLink = dynamicLinkEventArgs.ReceivedDynamicLink.Url.Query;
            if (CacheUsedQueryLink != null && CacheUsedQueryLink != currentLink)
            {
                DeepLinkQuery = currentLink;
            }
        };
#else

        Application.deepLinkActivated += OnDeepLinkActive;

        if (!string.IsNullOrEmpty(Application.absoluteURL))
        {
            OnDeepLinkActive(Application.absoluteURL);
        }

#endif

    }

    private void OnDeepLinkActive(string str)
    {
        var currentLink = new Uri(str).Query;

        if (CacheUsedQueryLink != null && CacheUsedQueryLink != currentLink)
        {
            DeepLinkQuery = currentLink;
        }
    }

    public void OnReceiveQueryLink(string currentLink)
    {
        if (CacheUsedQueryLink != null && CacheUsedQueryLink != currentLink)
        {
            DeepLinkQuery = currentLink;
        }
    }

    private void Start()
    {
#if !UNITY_WEBGL 
        Init();
#endif
        DontDestroyOnLoad(this);
    }

#if UNITY_WEBGL 
    [DllImport("__Internal")]
    private static extern void FirebaseLogEvent(string eventName);

    [DllImport("__Internal")]
    private static extern void FirebaseLogEventWithParam(string eventName, string paramName, string paramValue);
#endif

#if !UNITY_WEBGL
    private void Init()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
        if (dependencyStatus == DependencyStatus.Available)
        {
            var app = Firebase.FirebaseApp.DefaultInstance;
            InitializeFirebase();
        }
        else
        {
            Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
        }
        });
    }

    private void InitializeFirebase()
    {
         Debug.Log("Enabling data collection.");
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
        if (string.IsNullOrEmpty(LocalStorageUtils.GetFCMToken()))
        {
            GetTokenAsync();
        }
    }

    private async void GetTokenAsync()
    {
        var task = Firebase.Messaging.FirebaseMessaging.GetTokenAsync();

        await task;

        if (task.IsCompleted)
        {
            var token = task.Result;
            LocalStorageUtils.SetFCMToken(token);
        }
    }

    public void ResetAnalyticsData()
    {
        // if (isFirebaseDebugOn) Debug.Log("Reset analytics data.");
        FirebaseAnalytics.ResetAnalyticsData();
    }
#endif

    public void LogEvent(string eventName)
    {
        if(Application.platform == RuntimePlatform.WindowsEditor) return;
        try
        {
            Debug.Log("Firebase log event name: " + eventName);
#if UNITY_WEBGL
            FirebaseLogEvent(eventName);
#endif

#if !UNITY_WEBGL 
        FirebaseAnalytics.LogEvent(eventName);
#endif

        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public void LogEventWithParam(string eventName, string paramName, string paramValue)
    {
        if(Application.platform == RuntimePlatform.WindowsEditor) return;
        // Debug.Log("Firebase log event name: " + eventName + ", paramName: " + paramName + ", paramValue: " + paramValue);
        try
        {
#if UNITY_WEBGL 
        FirebaseLogEventWithParam(eventName, paramName, paramValue);
#endif

#if !UNITY_WEBGL 
        FirebaseAnalytics.LogEvent(eventName, paramName, paramValue);
#endif
    }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public void LogEventWithParams(string eventName, string[] paramNames, string[] paramValues)
    {
        try
        {
        // Debug.Log("Firebase log event name: " + eventName + ", paramName: " + paramName + ", paramValue: " + paramValue);
#if UNITY_WEBGL 
        var paramName = "";
        var paramValue = "";
        for (int i = 0; i < paramNames.Length - 1; i++)
        {
            paramName += paramNames[i] + "|";
            paramValue += paramValues[i] + "|";
        }

        paramName += paramNames.Last();
        paramValue += paramValues.Last();
        FirebaseLogEventWithParam(eventName, paramName, paramValue);
#endif

#if !UNITY_WEBGL 
        var prams = new Parameter[paramNames.Length];
        for (int i = 0; i < paramNames.Length; i++)
        {
            prams[i] = new Parameter(paramNames[i], paramValues[i]);
        }
        FirebaseAnalytics.LogEvent(eventName, prams);
#endif
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

#if !UNITY_WEBGL
    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        Debug.Log("Received Registration Token: " + token.Token);
        LocalStorageUtils.SetFCMToken(token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        Debug.Log("Received a new message from: " + e.Message.From);
        Debug.Log("Received a new message: " + e.Message.RawData);
    }

#endif

    public static string CreateDynamicLink(string query)
    {
#if !UNITY_WEBGL
        var components = new DynamicLinkComponents(
            // The base Link.
            //new Uri($"https://www.sandinh.com/game/chan5g?{query}"),
            new Uri($"{Cryptor.Decrypt("7LCFL1brwiKsOspsUljO4DI3Pc1W7W2a0lhVo3y84bc=", KEY)}?{query}"),
            // The dynamic link URI prefix.
#if !UNITY_ANDROID
            "https://chan5.page.link")
#else
            Cryptor.Decrypt("vYTaiYjjUHfz94ZxFbbeRsk6cI+8cNE+gWKWlPOLMIk=",KEY))
#endif
        {
            IOSParameters = new IOSParameters(Cryptor.Decrypt("CfW/9uA6flg5QmFms1XVFg==",KEY)),
            AndroidParameters = new AndroidParameters(
                "san.dinh.chanonline"),
        };

        var options = new DynamicLinkOptions
        {
            PathLength = DynamicLinkPathLength.Unguessable
        };

        var dnLink = components.LongDynamicLink.ToString();

        DynamicLinks.GetShortLinkAsync(components, options).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("GetShortLinkAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("GetShortLinkAsync encountered an error: " + task.Exception);
                return;
            }

            // Short Link has been created.
            var link = task.Result;
            dnLink = link.Url.ToString();

            var warnings = new System.Collections.Generic.List<string>(link.Warnings);
            if (warnings.Count > 0)
            {
                // Debug logging for warnings generating the short link.
            }
        });
        return dnLink;
#endif
        return
            $"https://chan5.page.link/?afl=&amv=0&apn=com.sdg.chan5&ibi=com.sds.chan5g&ifl=&ipfl=&link=https://www.sandinh.com/game/chan5g?{query}";
    }
}
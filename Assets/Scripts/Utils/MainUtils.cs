using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
#if UNITY_WEBGL
using System.Runtime.InteropServices;
#endif
using System.Security.Cryptography;
using System.Text;
using LuaFramework;
using UnityEngine;
using UnityEngine.Networking;

public class MainUtils
{
    public static byte[] key = Convert.FromBase64String("AQIDBAUGBWGJCGSMDQ0PAA==");
    public static bool isWeakSystem = false;
    public static bool IsAppDesktop = false;
    public static bool IsAppFacebook = false;
    public static string GameVersion = Application.version;
    
    public static string Encrypt(string decodedStr)
    {
        // SDLogger.Log("Encrypt: " + decodedStr);
        byte[] data = Encoding.UTF8.GetBytes(decodedStr);
        using (AesCryptoServiceProvider csp = new AesCryptoServiceProvider())
        {
            csp.KeySize = 256;
            csp.BlockSize = 128;
            csp.Key = key;
            csp.Padding = PaddingMode.PKCS7;
            csp.Mode = CipherMode.ECB;

            using (ICryptoTransform encrypter = csp.CreateEncryptor())
            {
                var arr = encrypter.TransformFinalBlock(data, 0, data.Length);
                return Convert.ToBase64String(arr);
            }
        }
    }

    public static string Decrypt(string encodedStr)
    {
        // SDLogger.Log("Decrypt: " + encodedStr);
        byte[] data = Convert.FromBase64String(encodedStr.Replace("_", "/"));
        using (AesCryptoServiceProvider csp = new AesCryptoServiceProvider())
        {
            csp.KeySize = 256;
            csp.BlockSize = 128;
            csp.Key = key;
            csp.Padding = PaddingMode.PKCS7;
            csp.Mode = CipherMode.ECB;

            using (ICryptoTransform decrypter = csp.CreateDecryptor())
            {
                var arr = decrypter.TransformFinalBlock(data, 0, data.Length);
                return Encoding.UTF8.GetString(arr);
            }
        }
    }

    public static string DecryptBase64(string base64)
    {
        var mBytes = Convert.FromBase64String(base64);
        var domain = ASCIIEncoding.ASCII.GetString(mBytes);
        return domain;
    }
    
    public static int CompareVersion(string versionA, string versionB)
    {
        // SDLogger.Log("CompareVersion: " + versionA + " vs " + versionB);
        var vA = versionA.Split('.');
        var vB = versionB.Split('.');
        for (var i = 0; i < vA.Length; ++i)
        {
            var a = int.Parse(vA[i]);
            var b = int.Parse(i < vB.Length ? vB[i] : "0");
            if (a != b)
            {
                return a - b;
            }
        }

        if (vB.Length > vA.Length)
        {
            return -1;
        }

        return 0;
    }
    
    public static void SendRequest(string method, string url,
        Action<string> onSuccess, Action<string> onFailure = null, string jsonData = "", bool isPure = false)
    {
        Executors.RunOnCoroutineNoReturn(Request(method, url, onSuccess, onFailure, jsonData, isPure));
    }
    
    private static IEnumerator Request(string method, string url,
        Action<string> onSuccess, Action<string> onFailure, string jsonData, bool isPure = false)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, method))
        {
            var bodyRaw = string.IsNullOrEmpty(jsonData) ? null : Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.certificateHandler = new BypassCertificate();
            if (!isPure)
            {
                request.SetRequestHeader("content-type", "application/json");
                request.SetRequestHeader("SdType", "2");
            }
            else
            {
                request.SetRequestHeader("content-type", "text/plain");

            }

            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                //SDLogger.Log("Error: " + webRequest.error);
                onFailure?.Invoke(request.error);
            }
            else
            {
                //SDLogger.Log("Received: " + webRequest.downloadHandler.text);
                var data = request.downloadHandler.text;
                onSuccess(data);
            }
        }
    }
    
    public static bool IsWeb()
    {
        return Application.platform == RuntimePlatform.WebGLPlayer;
    }
    
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void OpenURLInExternalWindow(string url);
#endif

    public static void OpenUrl(string url)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        OpenURLInExternalWindow( url);
        return;
#endif
        Application.OpenURL(url);
    }

    public static bool IsEditor()
    {
#if UNITY_EDITOR
        return true;
#endif

        return false;
    }
    
    public static DateTime ConvertSecondToDateTimeLocal(long seconds)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(seconds).AddHours(7);
        return dateTime;
    }
    
    public static string ConvertSecondToTime(long seconds, string format)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(seconds);
        return dateTime.ToString(format);
    }

    public static string FormatMoney(double value)
    {
        return value.ToString("N0", CultureInfo.GetCultureInfo("de"));
    }
    
    public static string FormatMoneyK(double value)
    {
        if (value >= 1000000000)
        {
            value /= 1000000000;
            return Math.Round(value, 2) + "B";
        }

        if (value >= 1000000)
        {
            value /= 1000000;
            return Math.Round(value, 2) + "M";
        }

        if (!(value >= 1000)) return value.ToString();
        value /= 1000;
        return Math.Round(value, 2) + "K";
    }

    public static bool ZipFile(string filePath, string zipPath)
    {
        try
        {
            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }

            using var zipToOpen = new FileStream(zipPath, FileMode.Create);
            using var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update);
            archive.CreateEntryFromFile(filePath, "log.txt");
            return true;
        }
        catch(Exception e)
        {
            return false;
        }
    }
}

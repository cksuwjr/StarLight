using UnityEngine;

public static class Vibration
{
#if UNITY_ANDROID && !UNITY_EDITOR
    private static AndroidJavaObject vibrator = null;
    private static AndroidJavaObject context = null;
#endif

    private static void Init()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (vibrator == null)
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                vibrator = context.Call<AndroidJavaObject>("getSystemService", "vibrator");
            }
        }
#endif
    }

    public static void Vibrate()
    {
        Vibrate(100); // default 100ms
    }

    public static void Vibrate(long milliseconds)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        Init();
        if (vibrator == null) return;

        using (AndroidJavaClass version = new AndroidJavaClass("android.os.Build$VERSION"))
        {
            int sdkInt = version.GetStatic<int>("SDK_INT");

            if (sdkInt >= 26)
            {
                using (AndroidJavaClass vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect"))
                {
                    AndroidJavaObject vibrationEffect = vibrationEffectClass.CallStatic<AndroidJavaObject>("createOneShot", milliseconds, vibrationEffectClass.GetStatic<int>("DEFAULT_AMPLITUDE"));
                    vibrator.Call("vibrate", vibrationEffect);
                }
            }
            else
            {
                vibrator.Call("vibrate", milliseconds);
            }
        }
#endif
    }

    public static void Vibrate(long[] pattern, int repeat)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        Init();
        if (vibrator == null) return;

        using (AndroidJavaClass version = new AndroidJavaClass("android.os.Build$VERSION"))
        {
            int sdkInt = version.GetStatic<int>("SDK_INT");

            if (sdkInt >= 26)
            {
                using (AndroidJavaClass vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect"))
                {
                    AndroidJavaObject vibrationEffect = vibrationEffectClass.CallStatic<AndroidJavaObject>("createWaveform", pattern, repeat);
                    vibrator.Call("vibrate", vibrationEffect);
                }
            }
            else
            {
                vibrator.Call("vibrate", pattern, repeat);
            }
        }
#endif
    }

    public static void Cancel()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        Init();
        if (vibrator == null) return;
        vibrator.Call("cancel");
#endif
    }

    public static bool HasVibrator()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        Init();
        if (vibrator == null) return false;
        return vibrator.Call<bool>("hasVibrator");
#else
        return false;
#endif
    }
}
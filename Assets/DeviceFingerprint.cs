using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SimpleJSON;
public class DeviceFingerprint : MonoBehaviour
{
    public string apiKey = ""; //ADD IN INSPECTOR
    public TMP_Text res;
    public static DeviceFingerprint Instance;
    AndroidJavaObject unityActivity;
    AndroidJavaObject InitializePlugin(String pluginName)
    {
        AndroidJavaClass unityClass;

        AndroidJavaObject _pluginInstance;

        unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
        _pluginInstance = new AndroidJavaObject(pluginName);
        if (_pluginInstance == null)
        {
            Debug.LogError(pluginName + " not found");
        }
        //_pluginInstance.CallStatic("receiveUnityActivity", unityActivity);
        return _pluginInstance;
    }
    // Start is called before the first frame update
    void Start()
    {

        Instance = this;
        Debug.Log("hello world");
    }


    class AndroidPluginCallback : AndroidJavaProxy
    {

        public AndroidPluginCallback() : base("io.zebedee.ipqssdk.PluginCallback") { }

        public void onSuccess(string result)
        {
            JSONNode data = JSON.Parse(result);

            DeviceFingerprint.Instance.res.text = result;
            Debug.Log("ENTER callback onSuccess: " + result);
        }
        public void onError(string errorMessage)
        {
            Debug.Log("ENTER callback onError: " + errorMessage);
        }
    }

    public void Test()
    {
        try
        {
            AndroidJavaObject zebedeeTools = InitializePlugin("io.zebedee.ipqssdk.DeviceCheck");
            zebedeeTools.Call("Init", unityActivity, apiKey, new AndroidPluginCallback());
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

    }
    // Update is called once per frame
    void Update()
    {

    }
}

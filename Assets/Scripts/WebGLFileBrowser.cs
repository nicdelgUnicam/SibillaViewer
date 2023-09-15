using System;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// This script is added to the GameObject spawned by the helper class
/// </summary>
public class WebGLFileBrowser : MonoBehaviour
{
    private void Start()
    {
        // Don't destroy the object this script is attached to, because the file browser system is singleton
        DontDestroyOnLoad(gameObject);
    }
    
    // This method is called from JS via SendMessage
    // ReSharper disable once UnusedMember.Local
    private void FileRequestCallback(string path)
    {
        // Sending the received link back to the helper
        WebGLFileBrowserHelper.SetResult(path);
    }
}

public static class WebGLFileBrowserHelper
{
    private static Action<string> _pathCallback;
    
    
    static WebGLFileBrowserHelper()
    {
        const string methodName = "FileRequestCallback";
        const string objectName = nameof(WebGLFileBrowserHelper);
        
        // Create a helper object for the FileBrowser system
        // ReSharper disable once ObjectCreationAsStatement
        new GameObject(objectName, typeof(WebGLFileBrowser));
        
        // Initializing the JS part of the FileBrowser system
        InitFileBrowser(objectName, methodName);
        Debug.Log("Initialized WebGL file browser");
    }
    
    
    /// <summary>
    /// Starts the file browser
    /// </summary>
    /// 
    /// <param name="callback">
    /// Will be called after the user selects a file, the Http path to the file is passed as a parameter
    /// </param>
    /// <param name="extensions">File extensions that can be selected, example: ".jpg, .jpeg, .png"</param>
    public static void RequestFile(Action<string> callback, string extensions = "")
    {
        Debug.Log("Starting WebGL file browser");
        _pathCallback = callback;
        RequestUserFile(extensions);
    }
    
    /// <summary>
    /// For internal use, invokes the callback and resets the file browser
    /// </summary>
    /// <param name="path">The path to the file</param>
    public static void SetResult(string path)
    {
        _pathCallback.Invoke(path);
        Dispose();
    }

    private static void Dispose()
    {
        ResetFileBrowser();
        _pathCallback = null;
    }

    
    // External functions from the .jslib file
    [DllImport("__Internal")]
    private static extern void InitFileBrowser(string objectName, string methodName);

    [DllImport("__Internal")]
    private static extern void RequestUserFile(string extensions);

    [DllImport("__Internal")]
    private static extern void ResetFileBrowser();
}
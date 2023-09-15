#if UNITY_WEBGL && !UNITY_EDITOR
using System;
using System.Collections;
using UnityEngine.Networking;
#else
using SFB;
#endif

using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public GameObject agent;
    [SerializeField] private GameObject indexSelector;
    [SerializeField] private GameObject instructionsText;
    [SerializeField] private GameObject cancelButton;

    private string _file;
    private int[] _indices = {0, 1, 2};


    /*private void Start()
    {
        var files = Directory.GetFiles("Assets/Resources", "*.csv");
        foreach (var file in files)
        {
            InstantiateAgent(file);
        }
    }*/

    #region UI Callbacks
    public void AddAgent()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        StartCoroutine(LoadAgentFile(url, InstantiateAgent));
#else
        InstantiateAgent(_file);
#endif
        indexSelector.SetActive(false);
        _indices = new []{0, 1, 2};
    }
    
    public void SelectFile()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        WebGLFileBrowserHelper.RequestFile(url =>
        {
            if (string.IsNullOrWhiteSpace(url))
                return;
            
            _file = url;
            indexSelector.SetActive(true);
        }, ".csv");
#else
        var filters = new ExtensionFilter[]
        {
            new("Csv files", "csv")
        };
        var paths = StandaloneFileBrowser.OpenFilePanel("Open a file", "", filters, false);
        if (paths.Length != 1 || paths[0].Length == 0) return;

        _file = paths[0];
        indexSelector.SetActive(true);
#endif
    }
    
    public void StartAgentSpriteChange()
    {
        Time.timeScale = 0f;
        ChangeSpriteScript.isRunning = false;

#if UNITY_WEBGL && !UNITY_EDITOR
        WebGLFileBrowserHelper.RequestFile(url =>
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                CancelAgentSpriteChange();
                return;
            }
            
            ChangeSpriteScript.spriteFile = url;
            instructionsText.SetActive(true);
            cancelButton.SetActive(true);
        }, ".png");
#else
        var filters = new ExtensionFilter[]
        {
            new("Images", "png")
        };
        var paths = StandaloneFileBrowser.OpenFilePanel("Open a file", "", filters, false);
        if (paths.Length != 1 || paths[0].Length == 0)
        {
            CancelAgentSpriteChange();
            return;
        }

        ChangeSpriteScript.spriteFile = paths[0];
        instructionsText.SetActive(true);
        cancelButton.SetActive(true);
#endif
    }

    public void OnTimeDropdownChanged(int index) => _indices[0] = index;
    public void OnXDropdownChanged(int index) => _indices[1] = index;
    public void OnYDropdownChanged(int index) => _indices[2] = index;
    #endregion
    
    public void CancelAgentSpriteChange()
    {
        Time.timeScale = 1f;
        ChangeSpriteScript.isRunning = true;
        ChangeSpriteScript.spriteFile = "";
        instructionsText.SetActive(false);
        cancelButton.SetActive(false);
    }

    private void InstantiateAgent(string agentFile)
    {
        var newObject = Instantiate(agent);
        
        var moveScript = newObject.GetComponent<MoveScript>();
        moveScript.sourceFile = agentFile;
        moveScript.indices = _indices;
        
        var changeSpriteScript = newObject.GetComponent<ChangeSpriteScript>();
        changeSpriteScript.instructionsText = instructionsText;
        changeSpriteScript.cancelButton = cancelButton;
    }
    
#if UNITY_WEBGL && !UNITY_EDITOR
    private static IEnumerator LoadAgentFile(string url, Action<string> finishCallback)
    {
        var request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"Request error: {request.error}");
            yield break;
        }

        finishCallback(request.downloadHandler.text);
    }
#endif
}

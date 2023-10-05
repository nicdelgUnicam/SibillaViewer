#if UNITY_WEBGL && !UNITY_EDITOR
using System;
using System.Collections;
using UnityEngine.Networking;
#else
using SFB;
#endif

using UnityEngine;

public class AgentSpawner : MonoBehaviour
{
    public GameObject agent;
    [SerializeField] private GameObject indexSelector;
    [SerializeField] private GameObject instructionsText;
    [SerializeField] private GameObject cancelButton;

    private string _file;
    private int[] _indices = {0, 1, 2};


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

    public void OnTimeDropdownChanged(int index) => _indices[0] = index;
    public void OnXDropdownChanged(int index) => _indices[1] = index;
    public void OnYDropdownChanged(int index) => _indices[2] = index;
    #endregion

    private void InstantiateAgent(string agentFile)
    {
        var newAgent = Instantiate(agent);
        
        var movementController = newAgent.GetComponent<AgentMovementController>();
        movementController.sourceFile = agentFile;
        movementController.indices = _indices;
        
        var changeSpriteScript = newAgent.GetComponent<ChangeSpriteScript>();
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

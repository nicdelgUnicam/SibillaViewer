using SFB;
using UnityEngine;

public class SpriteChangeController: MonoBehaviour
{
    public static bool isRunning = true;
    /// <summary>
    /// The path of the sprite file, or the blob url on WebGL
    /// </summary>
    public static string spriteFile = "";
    
    [SerializeField] private GameObject instructionsText;
    [SerializeField] private GameObject cancelButton;
    
    
    public void StartAgentSpriteChange()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        WebGLFileBrowserHelper.RequestFile(url =>
            {
                if (string.IsNullOrWhiteSpace(url))
                {
                    CancelAgentSpriteChange();
                    return;
                }
                
                OnFileChosen(url);
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

        OnFileChosen(paths[0]);
#endif
    }
    
    public void CancelAgentSpriteChange()
    {
        Time.timeScale = 1f;
        isRunning = true;
        spriteFile = "";
        instructionsText.SetActive(false);
        cancelButton.SetActive(false);
    }

    private void OnFileChosen(string file)
    {
        Time.timeScale = 0f;
        isRunning = false;
        spriteFile = file;
        instructionsText.SetActive(true);
        cancelButton.SetActive(true);
    }
}
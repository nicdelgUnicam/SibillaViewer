using SFB;
using UnityEngine;

public class SpriteChangeController: MonoBehaviour
{
    [SerializeField] private GameObject instructionsText;
    [SerializeField] private GameObject cancelButton;
    
    
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
    
    public void CancelAgentSpriteChange()
    {
        Time.timeScale = 1f;
        ChangeSpriteScript.isRunning = true;
        ChangeSpriteScript.spriteFile = "";
        instructionsText.SetActive(false);
        cancelButton.SetActive(false);
    }
}

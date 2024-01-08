#if UNITY_WEBGL && !UNITY_EDITOR
using System;
using System.Collections;
using UnityEngine.Networking;
#else
using System.IO;
#endif

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SpriteChanger : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent spriteChanged; // Calls SpriteChangeController.CancelAgentSpriteChange()
    

    public void OnPointerClick(PointerEventData eventData)
    {
        if (SpriteChangeController.isRunning || string.IsNullOrEmpty(SpriteChangeController.spriteFile))
            return;

#if UNITY_WEBGL && !UNITY_EDITOR
        StartCoroutine(LoadSpriteFromUrl(SpriteChangeController.spriteFile, SetSprite));
#else
        SetSprite(LoadSpriteFromFile(SpriteChangeController.spriteFile));
#endif
    }

#if UNITY_WEBGL && !UNITY_EDITOR
    // ReSharper disable Unity.PerformanceAnalysis
    private static IEnumerator LoadSpriteFromUrl(string url, Action<Texture2D> finishCallback)
    {
        var request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"Request error: {request.error}");
            yield break;
        }

        finishCallback(DownloadHandlerTexture.GetContent(request));
    }
#else
    private static Texture2D LoadSpriteFromFile(string filePath)
    {
        var bytes = File.ReadAllBytes(filePath);
        Texture2D texture = new(2, 2);
        return texture.LoadImage(bytes) ? texture : null;
    }
#endif

    private void SetSprite(Texture2D texture)
    {
        var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        GetComponent<SpriteRenderer>().sprite = sprite;
        GetComponent<CircleCollider2D>().radius = (sprite.bounds.extents.x + sprite.bounds.extents.y) / 2.0f;

        spriteChanged.Invoke();
    }
}

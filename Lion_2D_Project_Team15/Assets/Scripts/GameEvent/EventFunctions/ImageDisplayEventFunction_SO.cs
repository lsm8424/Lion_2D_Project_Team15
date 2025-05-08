using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(
    fileName = "ImageDisplayEventFunction_SO",
    menuName = "Scriptable Objects/EventFunction/ImageDisplayEventFunction_SO"
)]
public class ImageDisplayEventFunction_SO : EventFunction_SO
{
    [Header("이미지 설정")]
    public Sprite ImageToShow;
    public bool Show = true;

    [Header("태그 설정")]
    [SerializeField]
    private string targetTag = "CutImage"; // 기본값 설정

    private GameObject targetImage;

    public override void Setup()
    {
        // 비활성화된 오브젝트도 찾기 위해 Scene에서 모든 루트 오브젝트를 검색
        var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var root in rootObjects)
        {
            // 현재 오브젝트와 자식 오브젝트들 중에서 지정된 태그를 가진 오브젝트 검색
            targetImage = root.transform.FindRecursively(targetTag);
            if (targetImage != null)
                break;
        }

        if (targetImage == null)
        {
            Debug.LogError($"[ImageDisplayEvent] '{targetTag}' 태그를 가진 오브젝트를 찾을 수 없습니다.");
            return;
        }

        var img = targetImage.GetComponent<UnityEngine.UI.Image>();
        if (img == null)
        {
            Debug.LogError("[ImageDisplayEvent] Image 컴포넌트가 없습니다.");
        }
    }

    public override IEnumerator Execute()
    {
        if (targetImage == null)
        {
            Debug.LogError("[ImageDisplayEvent] Setup되지 않은 오브젝트입니다.");
            yield break;
        }

        var img = targetImage.GetComponent<UnityEngine.UI.Image>();
        if (img == null)
        {
            yield break;
        }

        if (Show)
        {
            img.sprite = ImageToShow;
            targetImage.SetActive(true);
        }
        else
        {
            targetImage.SetActive(false);
        }

        yield return null;
    }
}

// Transform 확장 메서드
public static class TransformExtensions
{
    public static GameObject FindRecursively(this Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
                return child.gameObject;

            var result = child.FindRecursively(tag);
            if (result != null)
                return result;
        }
        return null;
    }
}

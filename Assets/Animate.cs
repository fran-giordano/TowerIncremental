using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SimpleGifPlayer : MonoBehaviour
{
    [SerializeField] private Image targetImage;
    [SerializeField] private Sprite[] frames; // Drag your sliced frames here in the Inspector
    [SerializeField] private float framesPerSecond = 15f;

    private void Awake()
    {
        targetImage.gameObject.SetActive(false);
    }

    public void PlayGif()
    {
        StartCoroutine(PlayRoutine());
    }

    private IEnumerator PlayRoutine()
    {
        targetImage.gameObject.SetActive(true);
        float delay = 1f / framesPerSecond;
        for (int i = 0; i < frames.Length; i++)
        {
            targetImage.sprite = frames[i];
            yield return new WaitForSeconds(delay);
        }
    }
}
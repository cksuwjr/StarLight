using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlay : MonoBehaviour
{
    private VideoPlayer player;
    [SerializeField] private VideoClip[] clips;
    public Action OnVideoEnd;

    private void Awake()
    {
        TryGetComponent<VideoPlayer>(out player);
    }

    public void Play()
    {
        StartCoroutine("PlayVideos");
    }

    private IEnumerator PlayVideos()
    {
        for(int i = 0; i < clips.Length; i++)
        {
            player.clip = clips[i];
            UIManager.Instance.PlayVideo(player.clip.width, player.clip.height);
            player.Play();
            yield return YieldInstructionCache.WaitForSeconds((float)player.clip.length);
        }
        UIManager.Instance.StopVideo();
        OnVideoEnd?.Invoke();
    }
}

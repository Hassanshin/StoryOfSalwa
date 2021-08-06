using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoLoader : MonoBehaviour
{
    //https://drive.google.com/uc?export=download&id=DRIVE_FILE_ID
    //https://drive.google.com/uc?export=download&id=1adwiC6xkdZy20fIllVf1hZPe4cl9UiE1

    public string MUrl = "";
    private string mVideoName = "videoTest";
    public bool mClearCache = false;

    private VideoPlayer mPlayer = null;
    private AssetBundle mBundle = null;

    private void Awake()
    {
        mPlayer = GetComponent<VideoPlayer>();
        Caching.compressionEnabled = false;

        if (mClearCache)
        {
            Caching.ClearCache();
        }

        StartCoroutine(downloadAndPlay());
    }

    private IEnumerator downloadAndPlay()
    {
        LoadingHandler.Instance.ShowLoading(test);

        yield return download();

        if (!mBundle)
        {
            Debug.Log("BUNDLE FAILED TO LOAD");
            yield break;
        }

        VideoClip downloadedClip = mBundle.LoadAsset<VideoClip>(mVideoName);
        mPlayer.clip = downloadedClip;
        mPlayer.Play();
    }

    private void test()
    {
        Debug.Log("YEEY");
    }

    private IEnumerator download()
    {
        WWW request = WWW.LoadFromCacheOrDownload(MUrl, 0);

        while (!request.isDone)
        {
            LoadingHandler.Instance.Progress(request.progress);
            yield return null;
        }

        if (request.error == null)
        {
            mBundle = request.assetBundle;
            LoadingHandler.Instance.FinishedLoading();

            Debug.Log($"Downloading Success! {request.assetBundle}");
        }
        else
        {
            LoadingHandler.Instance.FailLoading(request.error);

            Debug.Log($"Downloading Failed because {request.error}");
        }

        yield return null;
    }
}
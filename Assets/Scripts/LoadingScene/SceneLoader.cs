using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Image imgFill;

    private void Start()
    {
        StartCoroutine(LoadScene(1));
    }

    private IEnumerator LoadScene(int index)
    {
        //FirebaseAnalyticsExtension.Instance.LogEvent(FirebaseEvent.Logging);
        imgFill.fillAmount = 0;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index);
        asyncOperation.allowSceneActivation = false;
        var progress = 0f;
        while (!asyncOperation.isDone)
        {
            progress = Mathf.MoveTowards(progress, asyncOperation.progress, Time.deltaTime);
            imgFill.fillAmount = progress;
            if (progress >= 0.9f)
            {
                //FirebaseAnalyticsExtension.Instance.LogEvent(FirebaseEvent.Lobby);
                
                imgFill.fillAmount = 1;
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
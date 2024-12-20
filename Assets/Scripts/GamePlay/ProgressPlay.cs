using UnityEngine;
using UnityEngine.UI;

public class ProgressPlay : MonoBehaviour
{
    [SerializeField] private Image imgFill;
    [SerializeField] private GameObject[] mileProgress;

    public void ShowView(int progress, int totalProgress)
    {
        imgFill.fillAmount = (float)(Mathf.Max(0, progress  - 1)) / totalProgress;
        if (progress >= totalProgress)
        {
            imgFill.fillAmount = 1f;
            GamePlayModle.Instance.Level += 1;
            return;
        }
        for (int i = 0; i < mileProgress.Length; i++)
        {
            mileProgress[i].SetActive(i <= progress - 1);
        }
    }
}
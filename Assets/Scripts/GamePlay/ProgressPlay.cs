using UnityEngine;
using UnityEngine.UI;

public class ProgressPlay : MonoBehaviour
{
    [SerializeField] private Image imgFill;
    [SerializeField] private GameObject[] mileProgress;
    private GamePlayModle _gamePlayModle = GamePlayModle.Instance;

    public void ShowView()
    {
        var fillAmount = (float)(Mathf.Max(0, _gamePlayModle.Level - 1)) / (GamePlayModle.levelSix - 1);
        imgFill.fillAmount = fillAmount;
        if (_gamePlayModle.Level >= GamePlayModle.levelSix)
        {
            imgFill.fillAmount = 1f;
            ShowInter();
        }

        if (_gamePlayModle.Level <= GamePlayModle.levelSix - 1)
        {
            for (int i = 0; i < mileProgress.Length; i++)
            {
                if (_gamePlayModle.Level - 2 < 0)
                {
                    mileProgress[i].Hide();
                }
                else
                {
                    mileProgress[i].SetActive(i <= _gamePlayModle.Level - 2);
                }
            }
        }
    }

    private void ShowInter()
    {
        AdsManager.Instance.ShowAds(TypeAds.INTERSTITIAL, (b, placement) =>
        {
            if (b)
            {
                Debug.Log("show reward");
                PopupManager.OpenPopup<RewardUnlock>(p =>
                {
                    p.ShowView();
                });
            }
        });
    }
}
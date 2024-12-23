using UnityEngine;
using UnityEngine.UI;

public class ProgressPlay : MonoBehaviour
{
    [SerializeField] private Image imgFill;
    [SerializeField] private GameObject[] mileProgress;
    private GamePlayModle _gamePlayModle = GamePlayModle.Instance;

    public void ShowView()
    {
        imgFill.fillAmount = (float)(Mathf.Max(0, _gamePlayModle.Level - 1)) / GamePlayModle.levelMax;
        if (_gamePlayModle.Level >= GamePlayModle.levelMax)
        {
            imgFill.fillAmount = 1f;
        }

        if (_gamePlayModle.Level <= GamePlayModle.levelMax - 1)
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
}
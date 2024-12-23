using UnityEngine;
using UnityEngine.UI;

public class SettingPopup : BaseUIPopup
{
    [SerializeField] private Button btnTermOfUser, btnPolicy;

    protected override void Start()
    {
        base.Start();
        btnTermOfUser.onClick.AddListener(Term);
        btnPolicy.onClick.AddListener(Policy);
    }

    private void Term()
    {
        Application.OpenURL("");
    }
    private void Policy()
    {
        Application.OpenURL("");
    }
}
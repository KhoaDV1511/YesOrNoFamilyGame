using UnityEngine;
using UnityEngine.UI;

public class SettingPopup : BaseUIPopup
{
    [SerializeField] private Button btnTermOfUser, btnPolicy;
    [SerializeField] private ToggleSetting music, sound, bell;
    private GamePlayModle _gamePlayModle = GamePlayModle.Instance;

    protected override void Start()
    {
        base.Start();
        btnTermOfUser.onClick.AddListener(Term);
        btnPolicy.onClick.AddListener(Policy);
        music.onclick.AddListener(SetMusic);
        sound.onclick.AddListener(SetSound);
        music.SetToggle(_gamePlayModle.IsOpenMusic);
        sound.SetToggle(_gamePlayModle.IsOpenSound);
    }

    private void SetMusic()
    {
        _gamePlayModle.IsOpenMusic = !_gamePlayModle.IsOpenMusic;
        music.SetToggle(_gamePlayModle.IsOpenMusic);
    }

    private void SetSound()
    {
        _gamePlayModle.IsOpenSound = !_gamePlayModle.IsOpenSound;
        sound.SetToggle(_gamePlayModle.IsOpenSound);
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
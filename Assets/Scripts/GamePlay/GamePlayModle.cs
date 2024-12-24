using UnityEngine;

public class GamePlayModle : Singleton<GamePlayModle>
{
    public const int levelSix = 6;
    public const int coinRewardLeveComplete = 100;
    public const int coinRewardUnlock = 500;
    public int currentLevel;
    public bool IsMaxLevel()
    {
        return Level >= levelSix;
    }
    
    private int _level = -1;
    private const string KeyLevel = "level_game_play";
    public int Level
    {
        get => _level < 0
            ? Mathf.Min(levelSix, PlayerPrefs.GetInt(KeyLevel, 1)) : Mathf.Min(levelSix, _level);
        set
        {
            _level = Mathf.Min(levelSix, value);
            if (_level == levelSix - 1)
            {
                AdsManager.Instance.LoadAds(TypeAds.INTERSTITIAL);
            }
            PlayerPrefs.SetInt(KeyLevel, _level);
        }
    }

    private long _coin = -1;
    private const string KeyCoin = "coin_game_play";
    public long Coin
    {
        get => _coin < 0
            ? long.Parse(PlayerPrefs.GetString(KeyCoin,
                0.ToString())) : _coin;
        set
        {
            _coin = value;
            Signals.Get<UpdateCoinSignal>().Dispatch();
            PlayerPrefs.SetString(KeyCoin, value.ToString());
        }
    }
    private string _isMusic = "";
    private const string KeyMusic = "key_music";
    public bool IsOpenMusic
    {
        get => _isMusic == "" ? bool.Parse(PlayerPrefs.GetString(KeyMusic, "True")) : bool.Parse(_isMusic);
        set
        {
            _isMusic = value.ToString();
            PlayerPrefs.SetString(KeyMusic, value.ToString());
        }
    }
    private string _isSound = "";
    private const string KeyOpenSound = "key_sound";
    public bool IsOpenSound
    {
        get => _isSound == "" ? bool.Parse(PlayerPrefs.GetString(KeyOpenSound, "True")) : bool.Parse(_isSound);
        set
        {
            _isSound = value.ToString();
            PlayerPrefs.SetString(KeyOpenSound, value.ToString());
        }
    }
}
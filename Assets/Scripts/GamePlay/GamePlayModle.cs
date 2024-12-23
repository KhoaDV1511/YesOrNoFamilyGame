using UnityEngine;

public class GamePlayModle : Singleton<GamePlayModle>
{
    public const int levelMax = 6;
    public const int coinReward = 100;
    public int currentLevel;
    public bool IsMaxLevel()
    {
        return Level >= levelMax;
    }
    
    private int _level = -1;
    private const string KeyLevel = "level_game_play";
    public int Level
    {
        get => _level == -1
            ? Mathf.Min(levelMax, PlayerPrefs.GetInt(KeyLevel, 1)) : Mathf.Min(levelMax, _level);
        set
        {
            _level = Mathf.Min(levelMax, value);
            PlayerPrefs.SetInt(KeyLevel, _level);
        }
    }

    private long _coin = -1;
    private const string KeyCoin = "coin_game_play";
    public long Coin
    {
        get => _level == -1
            ? long.Parse(PlayerPrefs.GetString(KeyCoin,
                0.ToString())) : _coin;
        set
        {
            _coin = value;
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
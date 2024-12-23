using UnityEngine;

public class GamePlayModle : Singleton<GamePlayModle>
{
    public const int levelMax = 5;
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
            ? PlayerPrefs.GetInt(KeyLevel, 1) : _level;
        set
        {
            _level = value;
            PlayerPrefs.SetInt(KeyLevel, value);
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
}
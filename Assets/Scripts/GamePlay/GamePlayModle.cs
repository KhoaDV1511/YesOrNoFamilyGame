using UnityEngine;

public class GamePlayModle : Singleton<GamePlayModle>
{
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
    public long _coin = -1;
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
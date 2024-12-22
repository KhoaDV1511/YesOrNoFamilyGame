using System;
using UnityEngine;

public class GlobalDataManager : MonoBehaviour
{
    private static GlobalDataManager _ins;
    public static GlobalDataManager Ins
    {
        get
        {
            if (!_ins)
            {
                _ins = FindObjectOfType<GlobalDataManager>();
            }

            return _ins;
        }
    }

    private void Awake()
    {
        _ins = FindObjectOfType<GlobalDataManager>();
    }

    public LevelOneData levelOneData;
    public LevelTwoData levelTwoData;
    public LevelThreeData levelThreeData;
    public LevelFourData levelFourData;
    public FlashPanel flashPanel;
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Store : BaseUIPopup
{
    [SerializeField] private DailyStore dailyStore;
    [SerializeField] private PotatoCoin potatoCoin;
    [SerializeField] private Button btnStoreDaily, btnBuyPotatoCoin;
    [SerializeField] private Sprite choose, unChoose;
    [SerializeField] private List<ShopPopup> shopPopups;

    private void OnEnable()
    {
        shopPopups.ForEach(sh => sh.obj.Hide());
        shopPopups.ForEach(sh => sh.img.sprite = unChoose);
        shopPopups.First().obj.Show();
        shopPopups.First().img.sprite = choose;
    }

    protected override void Start()
    {
        base.Start();
        btnStoreDaily.onClick.AddListener(ShowDailyStore);
        btnBuyPotatoCoin.onClick.AddListener(ShowPotatoCoin);
        foreach (var s in shopPopups)
        {
            s.btn.onClick.AddListener(() =>
            {
                shopPopups.ForEach(sh => sh.obj.Hide());
                shopPopups.ForEach(sh => sh.img.sprite = unChoose);
                s.obj.Show();
                s.img.sprite = choose;
            });
        }
    }

    private void ShowDailyStore()
    {
        
    }

    private void ShowPotatoCoin()
    {
        potatoCoin.ShowPotatoStore();
    }
}

[Serializable]
public class ShopPopup
{
    public Button btn;
    public Image img;
    public GameObject obj;
}
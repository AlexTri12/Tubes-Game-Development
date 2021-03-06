using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsPanel : MonoBehaviour
{
    public Panel panel;
    public Sprite allyBackground;
    public Sprite enemyBackground;
    public Image background;
    public Image avatar;
    public Text nameLabel;
    public Text hpLabel;
    public Text mpLabel;
    public Text lvlLabel;

    public void Display(GameObject obj)
    {
        Alliance alliance = obj.GetComponent<Alliance>();
        background.sprite = alliance.type == Alliances.Enemy ? enemyBackground : allyBackground;

        avatar.sprite = obj.GetComponent<Unit>().avatar;
        nameLabel.text = obj.name;
        Stats stats = obj.GetComponent<Stats>();

        if (stats)
        {
            hpLabel.text = string.Format("HP {0} / {1}", stats[StatsTypes.HP], stats[StatsTypes.MHP]);
            mpLabel.text = string.Format("MP {0} / {1}", stats[StatsTypes.MP], stats[StatsTypes.MMP]);
            lvlLabel.text = string.Format("LV. {0}", stats[StatsTypes.LVL]);
        }
    }
}

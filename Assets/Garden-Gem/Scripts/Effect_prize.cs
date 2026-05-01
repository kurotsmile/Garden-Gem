using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Effect_prize : MonoBehaviour
{
    public Image img_icon;
    public Text txt_msg;

    public void set_icon(Sprite sp_ico)
    {
        this.img_icon.sprite = sp_ico;
    }

    public void set_msg(string s_txt)
    {
        this.txt_msg.text = s_txt;
    }

    public void stop_ani()
    {
        Destroy(this.gameObject);
    }
}

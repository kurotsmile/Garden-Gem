using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot_Item : MonoBehaviour
{
    public Image img_item;
    public Image img_item_color;
    public Animator ani;
    public GameObject obj_effect_select;
    public int type;
    private int score;

    public void load(Sprite sp,int index_type,int index_score)
    {
        this.img_item_color.color = Color.white;
        this.obj_effect_select.SetActive(false);
        this.type = index_type;
        this.score = index_score;
        this.img_item.sprite = sp;
        this.img_item.transform.localScale=new Vector3(1f, 1f, 1f);
    }

    public int get_type()
    {
        return this.type;
    }

    public int get_score()
    {
        return this.score;
    }

    public void win(Color32 c)
    {
        this.img_item_color.color = c;
        this.obj_effect_select.SetActive(true);
        this.ani.Play("slot_item_win");
    }

    public void reset()
    {
        this.obj_effect_select.SetActive(false);
        this.ani.Play("slot_item_nomal");
    }

    public void stop_anim()
    {
        this.ani.Play("slot_item_nomal");
    }

    public void done()
    {
        this.ani.Play("slot_item_done");
    }

    public void play_welcome()
    {
        this.ani.Play("slot_item_welcome");
    }

    public void click()
    {
        GameObject.Find("Game").GetComponent<Games>().show_reward_rules_by_type(this.type);
    }

}

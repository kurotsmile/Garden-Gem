using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SLot_Line : MonoBehaviour
{
    public Slot_Item[] slot_item_check;
    public GameObject obj_line_start;
    public GameObject obj_line_end;
    public LineRenderer line_render;
    public Color32 color_line;

    private bool is_win = false;
    private bool is_win_jackpot = false;

    private List<Slot_Item> list_slot_item_win;

    public void load()
    {
        this.line_render.startWidth = 0.1f;
        this.line_render.endWidth = 0.1f;
        this.line_render.startColor = this.color_line;
        this.line_render.endColor = this.color_line;
        this.obj_line_start.GetComponent<Image>().color = this.color_line;
        this.obj_line_end.GetComponent<Image>().color = this.color_line;

        this.line_render.SetPosition(0, this.obj_line_start.transform.position);
        for(int i = 0; i < this.slot_item_check.Length; i++)
        {
            this.line_render.SetPosition(i+1, this.slot_item_check[i].transform.position);
        }
        this.line_render.SetPosition(6, this.obj_line_end.transform.position);
        this.line_render.enabled = false;
    }

    public void sprin()
    {
        this.list_slot_item_win = new List<Slot_Item>();
        this.is_win = false;
        this.is_win_jackpot = false;
        this.line_render.enabled = false;
    }

    private bool check_true_jackpot_all()
    {
        int type_check = this.slot_item_check[0].get_type();
        for(int i = 1; i < this.slot_item_check.Length; i++)
        {
            if (this.slot_item_check[i].get_type() != type_check)
            {
                return false;
            }
        }
        return true;
    }

    public void check_reward()
    {
        if (this.check_true_jackpot_all())
        {
            this.wind_jackpot();
            this.is_win = true;
            this.is_win_jackpot = true;
        }
        else
        {
            Slot_Item[] arr_item_find_4 = this.FindFourObjectsWithSameValue(this.slot_item_check);
            if (arr_item_find_4 != null)
            {
                for(int i = 0; i < arr_item_find_4.Length; i++)
                {
                    this.list_slot_item_win.Add(arr_item_find_4[i]);
                }
                this.is_win = true;
            }
            else
            {
                Slot_Item[] arr_item_find_3 = this.FindThreeObjectsWithSameValue(this.slot_item_check);
                if (arr_item_find_3 != null)
                {
                    for (int i = 0; i < arr_item_find_3.Length; i++)
                    {
                        this.list_slot_item_win.Add(arr_item_find_3[i]);
                    }
                   
                    this.is_win = true;
                }

            }
        }

    }

    public bool get_status_win()
    {
        return this.is_win;
    }

    public bool get_status_win_jackpot()
    {
        return this.is_win_jackpot;
    }

    public void wind_jackpot()
    {
        for (int i = 0; i < this.slot_item_check.Length; i++) this.list_slot_item_win.Add(this.slot_item_check[i]);
    }

    public int get_score_win()
    {
        return this.list_slot_item_win[0].get_score();
    }

    public Sprite get_sprite_win_slot_item()
    {
        return this.list_slot_item_win[0].img_item.sprite;
    }

    public int get_num_win_slot_item()
    {
        return this.list_slot_item_win.Count;
    }

    public void show_slot_item_win()
    {
        for (int i = 0; i < this.list_slot_item_win.Count; i++) this.list_slot_item_win[i].win(this.color_line);
        this.line_render.enabled = true;
    }


    public void visible(bool is_show)
    {
        this.line_render.enabled = is_show;
    }

    public Slot_Item[] FindThreeObjectsWithSameValue(Slot_Item[] arr)
    {
        for (int i = 0; i < arr.Length - 2; i++)
        {
            if (arr[i].get_type()==arr[i + 1].get_type() && arr[i + 1].get_type()==arr[i + 2].get_type())
            {
                return new Slot_Item[] { arr[i], arr[i + 1], arr[i + 2] };
            }
        }
        return null;
    }

    public Slot_Item[] FindFourObjectsWithSameValue(Slot_Item[] arr)
    {
        for (int i = 0; i < arr.Length - 3; i++)
        {
            if (arr[i].get_type() == arr[i + 1].get_type() && arr[i + 1].get_type() == arr[i + 2].get_type()&& arr[i + 2].get_type()== arr[i + 3].get_type())
            {
                return new Slot_Item[] { arr[i], arr[i + 1], arr[i + 2], arr[i + 3] };
            }
        }
        return null;
    }

    internal int get_score()
    {
        return (int) Math.Pow(this.list_slot_item_win[0].get_score(), this.get_num_win_slot_item());
    }
}

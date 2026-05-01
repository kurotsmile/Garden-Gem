using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Slot_Item[] slot_item;
    public Image[] img_item_slot_effect_animation;
    public Animator ain;
    public GameObject obj_effect_slot;
    public GameObject obj_all_slot;
    private Sprite[] list_sp_item_slot;
    private int[] list_score_item_slot;
    private bool is_spin = false;

    public void on_load(Sprite[] sp_array,int[] score_array)
    {
        this.obj_effect_slot.SetActive(false);
        this.obj_all_slot.SetActive(true);
        this.list_sp_item_slot = sp_array;
        this.list_score_item_slot = score_array;
        this.change_all_sp_item_slot();

        for (int i = 0; i < this.slot_item.Length; i++) this.slot_item[i].play_welcome();
    }

    public void act_spin()
    {
        for(int i = 0; i < this.img_item_slot_effect_animation.Length; i++)
        {
            this.img_item_slot_effect_animation[i].sprite = this.get_random_sprite_slot_item();
        }

        this.obj_all_slot.SetActive(false);
        this.obj_effect_slot.SetActive(true);
        this.ain.Play("slot_play_"+Random.Range(1,5));
        this.change_all_sp_item_slot();
        this.is_spin = true;
    }

    public Sprite get_random_sprite_slot_item()
    {
        int index_rand = Random.Range(0, this.slot_item.Length);
        return this.slot_item[index_rand].img_item.sprite;
    }

    private void change_all_sp_item_slot()
    {
        for (int i = 0; i < this.slot_item.Length; i++)
        {
            int rand_index = Random.Range(0, this.list_sp_item_slot.Length);
            this.slot_item[i].load(this.list_sp_item_slot[rand_index], rand_index,this.list_score_item_slot[rand_index]);
        }
    }


    public void stop_anim()
    {
        this.ain.Play("slot_nomal");
        this.obj_effect_slot.SetActive(false);
        this.obj_all_slot.SetActive(true);
        this.is_spin = false;
        GameObject.Find("Game").GetComponent<Games>().slot_manager.check_reward();
        for (int i = 0; i < this.slot_item.Length; i++) this.slot_item[i].done();
    }

    public bool get_status_spin()
    {
        return this.is_spin;
    }
}

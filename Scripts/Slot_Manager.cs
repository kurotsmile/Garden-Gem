using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot_Manager : MonoBehaviour
{
    [Header("Obj Main")]
    public Slot[] slots;
    public SLot_Line[] slot_line;
    public Sprite[] list_sp_item_slot;
    public int[] list_score_item_slot;
    public string[] list_name_item_slot;

    [Header("Ui Game")]
    public Image img_btn_spin;
    public Image img_btn_spin_auto;
    public Text txt_amount;
    public Text txt_amount_best;
    public Text txt_count_spin;
    public Text txt_number_line;
    public Text txt_recent_wins_val;
    public Text txt_highest_reward_val;
    public Text txt_msg_title;
    public Text txt_msg_tip;
    public Sprite sp_spin_on;
    public Sprite sp_spin_off;
    public Sprite sp_spin_auto_on;
    public Sprite sp_spin_auto_off;
    public GameObject obj_btn_add_best;
    public GameObject obj_btn_remove_best;
    public GameObject obj_btn_add_line;
    public GameObject obj_btn_remove_line;

    [Header("Effect")]
    public Transform area_body_all_effect;
    public GameObject effect_prize_prefab;

    private bool is_spin = false;
    private bool is_spin_auto = false;
    private int amount;
    private int amount_best;
    private int num_line;
    private int count_spin;
    private int scores_recent_wins_val;
    private int scores_highest_wins_val;
    private List<SLot_Line> line_slot_win;

    private bool is_show_line_slot_rewards = false;
    private int index_line_show_rewards = 0;
    private float timer_line_show_rewards = 0f;

    public void on_load()
    {
        this.num_line = this.slot_line.Length;
        this.amount = PlayerPrefs.GetInt("amount",1000);
        if (this.amount <= 0) this.amount = 1000;
        this.amount_best = 10;
        this.count_spin = 0;
        this.scores_highest_wins_val = PlayerPrefs.GetInt("scores_highest_wins_val",0);
        this.onload_all_line();

        this.update_amount_ui();
        this.update_spin_auto_ui();
        

        this.txt_msg_title.text = "Waiting for the prize draw";
        this.txt_msg_tip.text = "Welcome";
    }

    private void Update()
    {
        if (this.is_show_line_slot_rewards)
        {
            this.timer_line_show_rewards += (1.2f * Time.deltaTime);
            if (this.timer_line_show_rewards > 1f)
            {
                this.timer_line_show_rewards = 0f;
                this.show_next_line_rewards();
            }
        }
    }

    public void spin()
    {
        if (this.is_spin == false)
        {
            if (this.amount > 0)
            {
                if (UnityEngine.Random.Range(0, 3) <= 1)
                    this.txt_msg_title.text = "Spinning prize...";
                else
                    this.txt_msg_title.text = (this.count_spin + 1) + "th prize draw...";

                this.txt_msg_tip.text = "Good luck to you!";

                this.GetComponent<Games>().play_sound(0);
                this.amount -= this.amount_best;
                this.count_spin++;
                this.update_amount_ui();

                this.is_spin = true;
                this.img_btn_spin.sprite = this.sp_spin_off;
                for (int i = 0; i < this.slots.Length; i++)
                {
                    this.slots[i].act_spin();
                }

                for (int i = 0; i < this.slot_line.Length; i++)
                {
                    this.slot_line[i].sprin();
                }
            }
            else
            {
                this.txt_msg_title.text = "Running out of coins";
                this.txt_msg_tip.text = "You don't have enough coins to spin, please add coins at the shop";
                this.GetComponent<Games>().play_sound(3);
                this.GetComponent<Games>().carrot.play_vibrate();
                this.GetComponent<Games>().carrot.delay_function(2f, this.GetComponent<Games>().btn_shop);
            }

        }
        else
        {
            this.GetComponent<Games>().play_sound(3);
            this.GetComponent<Games>().carrot.play_vibrate();
        }
    }

    public void spin_auto()
    {
        if (this.is_spin_auto)
        {
            this.txt_msg_title.text = "Change the prize draw mode";
            this.txt_msg_tip.text = "Turn off auto mode!";
            this.is_spin_auto = false;
        }
        else
        {
            this.txt_msg_title.text = "Change the prize draw mode";
            this.txt_msg_tip.text = "Turn on auto mode!";
            this.is_spin_auto = true;
        }
            
        this.update_spin_auto_ui();
    }

    private void update_spin_auto_ui()
    {
        if (this.is_spin_auto)
            this.img_btn_spin_auto.sprite = this.sp_spin_auto_off;
        else
            this.img_btn_spin_auto.sprite = this.sp_spin_auto_on;
    }

    public void check_reward()
    {
        this.GetComponent<Games>().play_sound(1);
        int count_slots_done = 0;
        for(int i = 0; i < this.slots.Length; i++)
        {
            if(this.slots[i].get_status_spin()==false) count_slots_done++;
        }

        if (count_slots_done >= this.slots.Length)
        {
            this.check_reward_all_line();
        }
    }

    private void act_on_sprin()
    {
        this.img_btn_spin.sprite = this.sp_spin_on;
        this.is_spin = false;
    }

    private void check_reward_all_line()
    {
        this.line_slot_win = new List<SLot_Line>();
        int line_score = 0;
        for(int i = 0; i < this.slot_line.Length; i++)
        {
            if (this.slot_line[i].gameObject.activeInHierarchy)
            {
                this.slot_line[i].check_reward();
                if (this.slot_line[i].get_status_win())
                {
                    this.line_slot_win.Add(this.slot_line[i]);
                    SLot_Line line_win = this.slot_line[i];
                    if (line_win.get_status_win_jackpot())
                    {
                        line_score+=(((int)Math.Pow(line_win.get_score_win(), line_win.get_num_win_slot_item()) * 5) * this.amount_best) / this.num_line;
                    }
                    else
                    {
                        line_score+=((int)Math.Pow(line_win.get_score_win(), line_win.get_num_win_slot_item()) * this.amount_best) / this.num_line;
                    }
                }
            }
        }
        this.scores_recent_wins_val = line_score;
        if (this.scores_recent_wins_val > this.scores_highest_wins_val)
        {
            this.scores_highest_wins_val = this.scores_recent_wins_val;
            PlayerPrefs.SetInt("scores_highest_wins_val", this.scores_highest_wins_val);
            this.GetComponent<Games>().carrot.game.update_scores_player(this.scores_highest_wins_val);
        }

        if (line_score == 0)
        {
            this.txt_msg_title.text = "Get Rewards";
            this.txt_msg_tip.text = "Very sorry this bonus round you do not score!";
            this.GetComponent<Games>().play_sound(3);
            PlayerPrefs.SetInt("amount", this.amount);
        }
        else
        {
            this.txt_msg_title.text = "Get Rewards";
            this.txt_msg_tip.text = "+"+ this.scores_recent_wins_val+" ("+ this.line_slot_win.Count+" Line)";
        }
        this.update_amount_ui();
        this.show_line_slot_rewards();
    }

    private void show_line_slot_rewards()
    {
        if (this.line_slot_win.Count > 0)
        {
            this.index_line_show_rewards = 0;
            this.is_show_line_slot_rewards = true;
        }
        else
        {
            this.act_on_sprin();
            if (this.is_spin_auto) this.GetComponent<Games>().carrot.delay_function(1f, this.spin);
        }
        
    }

    private void show_next_line_rewards()
    {
        this.GetComponent<Games>().play_sound(2);
        this.GetComponent<Games>().game_ani.play_check_line();
        SLot_Line line_win = this.line_slot_win[this.index_line_show_rewards];
        line_win.show_slot_item_win();
        if (line_win.get_status_win_jackpot())
        {
            this.amount += (((int)Math.Pow(line_win.get_score_win(), line_win.get_num_win_slot_item()) * 5) * this.amount_best) / this.num_line;
            this.GetComponent<Games>().carrot.play_vibrate();
        }
        else
        {
            this.amount += ((int)Math.Pow(line_win.get_score_win(), line_win.get_num_win_slot_item()) * this.amount_best) / this.num_line;
        }
        PlayerPrefs.SetInt("amount", this.amount);
        this.create_effect_prize(line_win.get_sprite_win_slot_item(),"x"+line_win.get_num_win_slot_item());
        this.update_amount_ui();
        this.index_line_show_rewards++;
        if (this.index_line_show_rewards >= this.line_slot_win.Count)
        {
            this.is_show_line_slot_rewards = false;
            this.timer_line_show_rewards = 0;
            this.act_on_sprin();
            if (this.is_spin_auto) this.GetComponent<Games>().carrot.delay_function(1f, this.spin);
        }
    }

    private void update_amount_ui()
    {
        this.txt_amount.text = this.amount.ToString();
        this.txt_amount_best.text = this.amount_best.ToString();
        this.txt_count_spin.text = this.count_spin.ToString();
        this.txt_number_line.text = this.num_line.ToString();
        this.txt_recent_wins_val.text = this.scores_recent_wins_val.ToString();
        this.txt_highest_reward_val.text = this.scores_highest_wins_val.ToString();

        if (this.amount_best > 2000)
            this.obj_btn_add_best.SetActive(false);
        else
            this.obj_btn_add_best.SetActive(true);

        if (this.amount_best <= 10)
            this.obj_btn_remove_best.SetActive(false);
        else
            this.obj_btn_remove_best.SetActive(true);

        if (this.num_line >= this.slot_line.Length)
            this.obj_btn_add_line.SetActive(false);
        else
            this.obj_btn_add_line.SetActive(true);

        if (this.num_line <= 4)
            this.obj_btn_remove_line.SetActive(false);
        else
            this.obj_btn_remove_line.SetActive(true);
    }

    public void btn_add_best()
    {
        this.amount_best++;
        this.update_amount_ui();
    }

    public void btn_remove_best()
    {
        this.amount_best--;
        this.update_amount_ui();
    }

    public void btn_add_line()
    {
        this.num_line++;
        this.update_amount_ui();
        this.onload_all_line();
        this.check_line();
    }

    public void btn_remove_line()
    {
        this.num_line--;
        this.update_amount_ui();
        this.onload_all_line();
        this.check_line();
    }

    private void check_line()
    {
        for(int i = 0; i < this.slot_line.Length; i++)
        {
            if (this.slot_line[i].gameObject.activeInHierarchy)
            {
                this.slot_line[i].load();
                this.slot_line[i].visible(true);
            }  
        }
    }

    public void onload_all_line()
    {
        for(int i = 0; i < this.slot_line.Length; i++)
        {
            this.slot_line[i].load();
            if (i <= this.num_line)
                this.slot_line[i].gameObject.SetActive(true);
            else
                this.slot_line[i].gameObject.SetActive(false);
        }
    }

    public void hide_all_line_check()
    {
        for (int i = 0; i < this.slot_line.Length; i++)
        {
            this.slot_line[i].visible(false);
        }
    }

    public void create_effect_prize(Sprite icon,string s_msg)
    {
        GameObject obj_effect = Instantiate(this.effect_prize_prefab);
        obj_effect.transform.SetParent(this.area_body_all_effect);
        obj_effect.transform.position = Vector3.zero;
        obj_effect.transform.localScale = new Vector3(1f, 1f, 1f);
        obj_effect.GetComponent<Effect_prize>().set_icon(icon);
        obj_effect.GetComponent<Effect_prize>().set_msg(s_msg);
    }

    public int get_amount_best()
    {
        return this.amount_best;
    }

    public int get_num_line()
    {
        return this.num_line;
    }

    public void change_theme(Sprite[] list_sp_new,string[] list_name_theme)
    {
        this.list_sp_item_slot = list_sp_new;
        this.list_name_item_slot = list_name_theme;
        for (int i = 0; i < this.slots.Length; i++) this.slots[i].on_load(this.list_sp_item_slot, this.list_score_item_slot);
    }

    public void add_amount(int num_amount,Sprite coin_sp)
    {
        this.amount += num_amount;
        PlayerPrefs.SetInt("amount", this.amount);
        this.GetComponent<Games>().play_sound(4);
        this.update_amount_ui();
        this.create_effect_prize(coin_sp, "+"+ num_amount);
    }
}

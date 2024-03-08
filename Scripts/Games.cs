using System;
using System.Collections.Generic;
using UnityEngine;

public class Games : MonoBehaviour
{
    [Header("Obj main")]
    public Carrot.Carrot carrot;
    public Slot_Manager slot_manager;
    public Sprite sp_reward_rules;
    public Game_Ani game_ani;

    [Header("Sound")]
    public AudioSource[] sound;
    private Carrot.Carrot_Box box_setting;

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        this.carrot.Load_Carrot();
        this.carrot.shop.onCarrotPaySuccess+=this.GetComponent<Shop_Manager>().on_buy_success;
        this.carrot.ads.set_act_Rewarded_Success(this.GetComponent<Shop_Manager>().on_reward_success);

        this.game_ani.play_load();

        if (this.carrot.get_status_sound()) this.carrot.game.load_bk_music(this.sound[5]);

        this.GetComponent<Shop_Manager>().load_asset_theme();
        this.slot_manager.on_load();
    }

    public void btn_spin()
    {
        this.slot_manager.spin();
    }

    public void btn_spin_auto()
    {
        this.carrot.ads.show_ads_Interstitial();
        this.carrot.play_sound_click();
        this.slot_manager.spin_auto();
    }

    public void play_sound(int index)
    {
        if (this.carrot.get_status_sound()) this.sound[index].Play();
    }

    public void btn_setting()
    {
        this.carrot.ads.show_ads_Interstitial();
        this.slot_manager.hide_all_line_check();
        this.box_setting=this.carrot.Create_Setting();
        
        Carrot.Carrot_Box_Item store_shop_item=this.box_setting.create_item_of_top("setting_shop");
        store_shop_item.set_icon(this.GetComponent<Shop_Manager>().sp_icon_shop);
        store_shop_item.set_title("Shop");
        store_shop_item.set_tip("You can buy game support items");
        store_shop_item.set_act(this.btn_shop);

        this.box_setting.update_color_table_row();
        this.box_setting.set_act_before_closing(this.act_close_setting);
    }

    private void act_close_setting(List<string> list_change)
    {
        foreach(string s in list_change)
        {
            if (s == "list_bk_music") this.carrot.game.load_bk_music(this.sound[5]);
        }
        if (this.carrot.get_status_sound())
            this.sound[5].Play();
        else
            this.sound[5].Stop();
    }

    public void btn_user()
    {
        this.slot_manager.hide_all_line_check();
        this.carrot.show_login();
    }

    public void btn_show_reward_rules()
    {
        this.show_reward_rules_by_type(-1);
    }

    public void show_reward_rules_by_type(int type=-1)
    {
        this.carrot.ads.show_ads_Interstitial();
        this.carrot.play_sound_click();
        this.slot_manager.hide_all_line_check();
        Carrot.Carrot_Box box_rule = this.carrot.Create_Box("Reward Rules");
        box_rule.set_icon(this.sp_reward_rules);

        for (int i = 0; i < this.slot_manager.list_sp_item_slot.Length; i++)
        {
            int s_item_score = this.slot_manager.list_score_item_slot[i];
            Carrot.Carrot_Box_Item item_rule = box_rule.create_item("rule_" + i);
            item_rule.set_icon_white(this.slot_manager.list_sp_item_slot[i]);
            item_rule.set_title(this.slot_manager.list_name_item_slot[i]);
            item_rule.set_tip("x3 multiplier reward = " + (Math.Pow(s_item_score, 3) * this.slot_manager.get_amount_best()) + " , x4 multiplier reward = " + (Math.Pow(s_item_score, 4) * this.slot_manager.get_amount_best()) + " , Jackpot =" + ((Math.Pow(s_item_score, 5) * 5) * this.slot_manager.get_amount_best()) + " / " + this.slot_manager.get_num_line()+" Line");
            if (i == type)
            {
                Carrot.Carrot_Box_Btn_Item btn_view=item_rule.create_item();
                btn_view.set_color(this.carrot.color_highlight);
                btn_view.set_icon(this.carrot.user.icon_user_info);
            }
        }
        box_rule.update_color_table_row();
    }

    public void btn_rate()
    {
        this.carrot.ads.show_ads_Interstitial();
        this.slot_manager.hide_all_line_check();
        this.carrot.show_rate();
    }

    public void btn_share()
    {
        this.carrot.ads.show_ads_Interstitial();
        this.slot_manager.hide_all_line_check();
        this.carrot.show_share();
    }

    public void btn_carrot_app()
    {
        this.carrot.ads.show_ads_Interstitial();
        this.slot_manager.hide_all_line_check();
        this.carrot.show_list_carrot_app();
    }

    public void btn_rank()
    {
        this.carrot.ads.show_ads_Interstitial();
        this.slot_manager.hide_all_line_check();
        this.carrot.game.Show_List_Top_player();
    }

    public void btn_shop()
    {
        this.carrot.ads.show_ads_Interstitial();
        this.slot_manager.hide_all_line_check();
        this.carrot.play_sound_click();
        this.GetComponent<Shop_Manager>().show_shop();
    }

    public void btn_shop_coin()
    {
        this.carrot.ads.show_ads_Interstitial();
        this.slot_manager.hide_all_line_check();
        this.carrot.play_sound_click();
        this.GetComponent<Shop_Manager>().show_shop_coin();
    }
}

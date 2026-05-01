using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Item_Shop_Category
{
    coin,theme,all
}

public enum Item_Shop_Type
{
    coin_a,coin_b,coin_c,theme_a,theme_b,theme_c
}

public class Shop_Manager : MonoBehaviour
{
    [Header("Icon")]
    public Sprite sp_icon_shop;
    public Sprite sp_icon_coin_shop;
    public Sprite sp_icon_theme_shop;
    public Sprite sp_icon_ads;
    public Sprite sp_icon_buy;

    [Header("Asset Theme")]
    public Sprite[] list_sp_themea;
    public Sprite[] list_sp_themeb;
    public Sprite[] list_sp_themec;
    public string[] list_name_themea;
    public string[] list_name_themeb;
    public string[] list_name_themec;
    private string s_sel_name_theme;

    public Sprite[] list_icon_item;
    public string[] list_name_item;
    public string[] list_tip_item;
    public bool[] list_buy_item;
    public Item_Shop_Type[] list_type_item;
    public Item_Shop_Category[] list_category_item;

    private Carrot.Carrot_Box box_shop;
    private Carrot.Carrot_Window_Msg msg_shop;
    private int index_temp_act_shop = -1;

    public void load_asset_theme()
    {
        this.s_sel_name_theme = PlayerPrefs.GetString("s_sel_name_theme", "theme_a");
        switch (this.s_sel_name_theme)
        {
            case "theme_a":
                this.GetComponent<Games>().slot_manager.change_theme(this.list_sp_themea, this.list_name_themea);
                break;
            case "theme_b":
                this.GetComponent<Games>().slot_manager.change_theme(this.list_sp_themeb, this.list_name_themeb);
                break;
            case "theme_c":
                this.GetComponent<Games>().slot_manager.change_theme(this.list_sp_themec, this.list_name_themec);
                break;
        }
    }

    public void show_shop()
    {
        this.show_shop_by_category(Item_Shop_Category.all);
    }

    public void show_shop_coin()
    {
        this.show_shop_by_category(Item_Shop_Category.coin);
    }

    public void show_shop_theme()
    {
        this.show_shop_by_category(Item_Shop_Category.theme);
    }

    public void show_shop_by_category(Item_Shop_Category cat)
    {
        this.check_list_by_data();
        if (this.box_shop != null) this.box_shop.close();

        this.box_shop = this.GetComponent<Games>().carrot.Create_Box("Shop");
        this.box_shop.set_icon(this.sp_icon_shop);
        
        Carrot.Carrot_Box_Btn_Item btn_all_cat=this.box_shop.create_btn_menu_header(this.GetComponent<Games>().carrot.icon_carrot_all_category);
        if (cat == Item_Shop_Category.all) btn_all_cat.set_icon_color(this.GetComponent<Games>().carrot.color_highlight);
        btn_all_cat.set_act(this.show_shop);

        Carrot.Carrot_Box_Btn_Item btn_coin_cat=this.box_shop.create_btn_menu_header(this.sp_icon_coin_shop);
        if (cat == Item_Shop_Category.coin) btn_coin_cat.set_icon_color(this.GetComponent<Games>().carrot.color_highlight);
        btn_coin_cat.set_act(this.show_shop_coin);

        Carrot.Carrot_Box_Btn_Item btn_theme_cat=this.box_shop.create_btn_menu_header(this.sp_icon_theme_shop);
        if (cat == Item_Shop_Category.theme) btn_theme_cat.set_icon_color(this.GetComponent<Games>().carrot.color_highlight);
        btn_theme_cat.set_act(this.show_shop_theme);

        for (int i = 0; i < this.list_icon_item.Length; i++)
        {
            if (cat != Item_Shop_Category.all)
            {
                if (this.list_category_item[i] != cat) continue;
            }

            var index_i = i;
            Carrot.Carrot_Box_Item box_item_shop = this.box_shop.create_item("item_shop_" + i);
            box_item_shop.set_icon_white(this.list_icon_item[i]);
            box_item_shop.set_title(this.list_name_item[i]);
            box_item_shop.set_tip(this.list_tip_item[i]);
            box_item_shop.set_act(() => this.select_item_shop(index_i));

            if (this.list_buy_item[i])
            {
                Carrot.Carrot_Box_Btn_Item btn_item_buy = box_item_shop.create_item();
                btn_item_buy.set_icon(this.sp_icon_buy);
                btn_item_buy.set_color(this.GetComponent<Games>().carrot.color_highlight);
                btn_item_buy.set_act(() => this.buy_shop_item(index_i));

                Carrot.Carrot_Box_Btn_Item btn_item_ads = box_item_shop.create_item();
                btn_item_ads.set_icon(this.sp_icon_ads);
                btn_item_ads.set_color(this.GetComponent<Games>().carrot.color_highlight);
                btn_item_ads.set_act(() => this.watch_ads_shop_item(index_i));
            }
        }
    }

    private void select_item_shop(int index_item)
    {
        if (this.list_buy_item[index_item])
        {
            this.msg_shop=this.GetComponent<Games>().carrot.show_msg("Shop", "Can you buy or watch ads to receive this item?", Carrot.Msg_Icon.Question);
            this.msg_shop.add_btn_msg("Buy",()=> buy_shop_item(index_item));
            this.msg_shop.add_btn_msg("Watch ads", ()=> watch_ads_shop_item(index_item));
            this.msg_shop.add_btn_msg("Cancel",this.close_msg_shop);
        }
        else
        {
            this.act_item_shop(index_item);
        }
    }

    private void buy_shop_item(int index_item)
    {
        this.index_temp_act_shop = index_item;
        this.GetComponent<Games>().carrot.buy_product(2);
    }

    private void watch_ads_shop_item(int index_item)
    {
        this.index_temp_act_shop = index_item;
        this.GetComponent<Games>().carrot.ads.show_ads_Rewarded();
    }

    private void close_msg_shop()
    {
        if(this.msg_shop!=null)  this.msg_shop.close();
    }

    private void act_item_shop(int index_item)
    {
        Item_Shop_Type i_type = this.list_type_item[index_item];

        switch (i_type)
        {
            case Item_Shop_Type.coin_a:
                this.GetComponent<Games>().slot_manager.add_amount(1000, this.list_icon_item[0]);
                break;
            case Item_Shop_Type.coin_b:
                this.GetComponent<Games>().slot_manager.add_amount(5000, this.list_icon_item[1]);
                break;
            case Item_Shop_Type.coin_c:
                this.GetComponent<Games>().slot_manager.add_amount(99999, this.list_icon_item[2]);
                break;
            case Item_Shop_Type.theme_a:
                PlayerPrefs.SetString("s_sel_name_theme", "theme_a");
                this.GetComponent<Games>().slot_manager.change_theme(this.list_sp_themea,this.list_name_themea);
                break;
            case Item_Shop_Type.theme_b:
                PlayerPrefs.SetString("s_sel_name_theme", "theme_b");
                this.GetComponent<Games>().slot_manager.change_theme(this.list_sp_themeb, this.list_name_themeb);
                break;
            case Item_Shop_Type.theme_c:
                PlayerPrefs.SetString("s_sel_name_theme", "theme_c");
                this.GetComponent<Games>().slot_manager.change_theme(this.list_sp_themec, this.list_name_themec);
                break;
        }
        this.close_shop();
    }

    private void close_shop()
    {
        if (this.msg_shop != null) this.msg_shop.close();
        if (this.box_shop != null) this.box_shop.close();
    }

    public void on_buy_success(string s_id)
    {
        if (s_id == this.GetComponent<Games>().carrot.shop.get_id_by_index(2))
        {
            if (this.index_temp_act_shop > 3)
            {
                PlayerPrefs.SetInt("is_buy_item_shop_" + this.index_temp_act_shop, 1);
            }
            this.act_item_shop(this.index_temp_act_shop);
            this.check_list_by_data();
        }
    }

    public void on_reward_success()
    {
        this.act_item_shop(this.index_temp_act_shop);
    }

    private void check_list_by_data()
    {
        for(int i = 0; i < this.list_buy_item.Length; i++)
        {
            if (PlayerPrefs.GetInt("is_buy_item_shop_" + i) == 1) this.list_buy_item[i] = false;
        }
    }
}

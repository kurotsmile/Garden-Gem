using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Ani : MonoBehaviour
{
    public Animator ani;

    public void play_load()
    {
        this.ani.Play("game_ain_load");
    }

    public void stop_anim()
    {
        this.ani.Play("game_ain");
        GameObject.Find("Game").GetComponent<Games>().slot_manager.onload_all_line();
    }

    public void play_check_line()
    {
        this.ani.Play("game_ain_check_line");
    }
}

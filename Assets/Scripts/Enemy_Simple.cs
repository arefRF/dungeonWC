﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Simple : Enemy {


    public override void SetNextPos()
    {
        UpdatePlayerPos();
        if (PlayerPos == Position)
            engine.EnemyMoveFinished();
        List<int> selected = new List<int>();
        float min = 10000;
        for(int i=0; i<4; i++)
        {
            Vector2 temppos = ToolKit.VectorSum(Position, ToolKit.IntToDirection(i));
            if (!CanMoveToPosition(temppos))
                continue;
            float temp = Vector2.SqrMagnitude(temppos - PlayerPos);
            if (temp < min && min - temp > 0.01)
            {
                selected.Clear();
                selected.Add(i);
                min = temp;
            }
            else if (min - temp < 0.01 && min >= temp)
            {
                selected.Add(i);
            }
        }
        if (selected.Count == 0)
            engine.EnemyMoveFinished();
        if(selected.Count == 1)
        {
            NextPos = ToolKit.VectorSum(Position, ToolKit.IntToDirection(selected[0]));
        }
        else
        {
            int sel = selected[0];
            if (PlayerPos == engine.player.Position)
            {
                min = 1000;
                for (int i = 0; i < selected.Count; i++)
                {
                    Debug.Log(selected[i]);
                    Vector2 temppos = ToolKit.VectorSum(Position, ToolKit.IntToDirection(selected[i]));
                    float temp = Vector2.SqrMagnitude(temppos - engine.player.prevpos);
                    if (temp < min)
                    {
                        sel = selected[i];
                        min = temp;
                    }
                }
            }
            NextPos = ToolKit.VectorSum(Position, ToolKit.IntToDirection(sel));
        }
    }

    public override void Move()
    {
        engine.RemovefromDatabase(this);
        Position = NextPos;
        transform.position = NextPos;
        engine.AddtoDatabase(this);
        engine.EnemyMoveFinished();
    }
}

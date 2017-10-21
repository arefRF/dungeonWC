﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Unit {

    public Trap trap { get; set; }

    public void Move(Direction direction)
    {
        if (trap != null)
            trap.Move(direction);
        engine.AddToSnapshot(Clone());
        engine.RemovefromDatabase(this);
        Position = ToolKit.VectorSum(Position, direction);
        transform.position = ToolKit.VectorSum(transform.position, direction);
        engine.AddtoDatabase(this);
    }

	public bool CanMoveToPosition(Vector2 position, Direction direction)
    {
        if (!(position.x >= 0 && position.y >= 0 && position.x < engine.sizeX && position.y < engine.sizeY))
            return false;
        List<Unit> units = engine.units[(int)position.x, (int)position.y];
        for(int i=0; i<units.Count; i++)
        {
            if (units[i] is Box || units[i] is Block || units[i] is Enemy || units[i] is TNT)
                return false;
            else if(units[i] is Trap)
            {
                if((units[i] as Trap).CanMoveToPosition(ToolKit.VectorSum(Position, direction)))
                {
                    trap = units[i] as Trap;
                    return true;
                }
                return false;
            }
        }
        return true;
    }

    public override Clonable Clone()
    {
        return new ClonableBox(this);
    }
}

public class ClonableBox : Clonable
{
    public ClonableBox(Box box)
    {
        original = box;
        position = box.Position;
        trasformposition = box.transform.position;
    }

    public override void Undo()
    {
        Box box = original as Box;
        box.engine.RemovefromDatabase(original);
        box.Position = position;
        box.transform.position = trasformposition;
        box.engine.AddtoDatabase(original);
    }
}

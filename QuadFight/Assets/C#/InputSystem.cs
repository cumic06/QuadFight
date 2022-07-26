using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputSystem : MonoSingleton<InputSystem>
{
    Vector2 moveDir;
    public Vector2 MoveDir => moveDir;

    public Action guardEvent;
    public Action attackEvent;
    public Action guardmoveEvent;

    private void Update()
    {
        moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(DataManager.keyData.guardKey))
        {
            guardEvent?.Invoke();
        }

        if (Input.GetKeyDown(DataManager.keyData.Fire))
        {
            attackEvent?.Invoke();
        }
    }
}

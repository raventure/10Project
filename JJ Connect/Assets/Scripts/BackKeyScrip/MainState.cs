using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainState {
    public enum State
    {
        Home,
        Ingame,
        GameOver,
        SelectMap,
        SelectLevel,
        Waiting,
    }
    public static State state;
    public static bool allowButton;

    public static void SetState(State newState)
    {
        state = newState;
    }

    public static State GetState
    {
        get { return state; }
    }

    public enum StateBack
    {
        Home,
        Continue,
        SelectLevel,
        SelectMap,
    }

    public static StateBack stateBack;
    public static void SetStateBack(StateBack newState)
    {
        stateBack = newState;
    }
    public static StateBack GetStateGack
    {
        get { return stateBack; }
    }

}

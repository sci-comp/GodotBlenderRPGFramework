using Godot;

public static partial class Mouse
{
    /* As a game grows in size, it can become easy to lose control of the cursor's state.
     * This static class means to centralize access the mouse's mode.
     *
     * If this class is in use, then, generally speaking, we should never directly set Input.MouseMode. */

    public static void SetCaptured(string message = "")
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        GD.Print("[Mouse] Set captured" + (message == "" ? "" : " by " + message));
    }

    public static void SetConfined(string message = "")
    {
        Input.MouseMode = Input.MouseModeEnum.Confined;
        GD.Print("[Mouse] Set confined" + (message == "" ? "" : " by " + message));
    }

    public static void SetConfinedHidden(string message = "")
    {
        Input.MouseMode = Input.MouseModeEnum.ConfinedHidden;
        GD.Print("[Mouse] Set confinded-hidden" + (message == "" ? "" : " by " + message));
    }

    public static void SetHidden(string message = "")
    {
        Input.MouseMode = Input.MouseModeEnum.Hidden;
        GD.Print("[Mouse] Set hidden" + (message == "" ? "" : " by " + message));
    }

    public static void SetVisible(string message = "")
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
        GD.Print("[Mouse] Set visible" + (message == "" ? "" : " by " + message));
    }

    public static bool IsCursorCaptured(string message = "")
    {
        return Input.MouseMode == Input.MouseModeEnum.Captured;
    }

    public static bool IsCursorConfined()
    {
        return Input.MouseMode == Input.MouseModeEnum.Confined;
    }

    public static bool IsCursorConfinedHidden()
    {
        return Input.MouseMode == Input.MouseModeEnum.ConfinedHidden;
    }

    public static bool IsCursorHidden()
    {
        return Input.MouseMode == Input.MouseModeEnum.Hidden;
    }

    public static bool IsCursorVisible()
    {
        return Input.MouseMode == Input.MouseModeEnum.Visible;
    }

}


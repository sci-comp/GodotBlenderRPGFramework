using Godot;

namespace Toolbox
{
    public static class Audio
    {
        public static float Db2Linear(float db)
        {
            if (db <= -80f)
            {
                return 0f;
            }
            else if (db >= 0f)
            {
                return 1f;
            }
            else
            {
                float linear = Mathf.Pow(10, db / 20f);
                return linear;
            }
        }

        public static float Linear2Db(float linear)
        {
            if (linear <= 0)
            {
                return -80f;
            }
            else if (linear >= 1)
            {
                return 0f;
            }
            else
            {
                return 20f * Mathf.Log(linear) / Mathf.Log(10);
            }
        }
    }

}


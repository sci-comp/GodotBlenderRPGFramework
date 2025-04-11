using Godot;
using System.Collections.Generic;

namespace Toolbox
{
    public enum CharacterLocation
    {
        Above,
        Behind,
        Center,
        Dynamic,
        Feet,
        Front,
        Head,
        WaterLine
    }

    public static class Toolbox
    {
        public static void FindAndPopulate<T>(Node node, List<T> list) where T : class
        {
            foreach (Node child in node.GetChildren())
            {
                if (child is T item)
                {
                    list.Add(item);
                }

                FindAndPopulate(child, list);
            }
        }

    }

}


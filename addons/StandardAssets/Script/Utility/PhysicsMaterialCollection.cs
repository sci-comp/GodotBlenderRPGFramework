using Godot;
using System.Collections.Generic;

namespace Game
{
    public static class PhysicsMaterialCollection
    {
        private static string PhysicsMaterialPath = "res://addons/StandardAssets/PhysicsMaterial/";

        private static Dictionary<string, PhysicsMaterial> physicsMaterial;

        public static Dictionary<string, PhysicsMaterial> PhysicsMaterial
        {
            get
            {
                if (physicsMaterial == null)
                {
                    physicsMaterial = new Dictionary<string, PhysicsMaterial>();

                    using var dir = DirAccess.Open(PhysicsMaterialPath);
                    
                    if (dir != null)
                    {
                        dir.ListDirBegin();
                        string fileName = dir.GetNext();
                        while (fileName != "")
                        {
                            // DirAccess returns
                            //   in an exported build: dir/fileName.extension.import
                            //   in the editor: dir/fileName.extension
                            // In an exported build, ResourceLoader can load from the original path
                            fileName = fileName.Replace(".import", "");
                            fileName = fileName.Replace(".remap", "");

                            if (ResourceLoader.Exists(PhysicsMaterialPath + fileName))
                            {
                                if (ResourceLoader.Load(PhysicsMaterialPath + fileName) is PhysicsMaterial physMat)
                                {
                                    string physMatName = fileName.TrimSuffix(".tres");
                                    physicsMaterial[physMatName] = physMat;
                                }
                            }

                            fileName = dir.GetNext();
                        }
                    }
                }

                return physicsMaterial;
            }
        }

    }

}


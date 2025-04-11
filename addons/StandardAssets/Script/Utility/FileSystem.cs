using Godot;
using System;
using System.Collections.Generic;
using System.IO;

namespace Toolbox
{
    public static class FileSystem
    {
        /*These seem to have problems in 4.4 beta2.. let's find an alternate approach

        public static List<string> GetPathsWithExtension(string dir_path, string extension)
        {
            // Given a path to a directory, return a list of all files with an extension 
                matching the parameter. This method checks the contents of nested directories.

            List<string> paths = [];
            DirAccess dir = DirAccess.Open(dir_path);

            if (dir != null)
            {
                dir.ListDirBegin();
                string fileName = dir.GetNext();

                // DirAccess returns
                //   in an exported build: dir/fileName.extension.import
                //   in the editor: dir/fileName.extension
                // In an exported build, ResourceLoader can load from the original path
                fileName = fileName.Replace(".import", "");

                while (fileName != "")
                {
                    if (dir.CurrentIsDir())
                    {
                        if (fileName != "." && fileName != "..")
                        {
                            paths.AddRange(GetPathsWithExtension(dir_path + "/" + fileName, extension));
                        }
                    }
                    else if (fileName.EndsWith(extension))
                    {

                        paths.Add(dir_path + "/" + fileName);
                    }

                    fileName = dir.GetNext();
                }
            }
            dir.close

            return paths;
        }

        public static List<T> LoadResourcesFromDirectory<T>(string path) where T : Resource
        {
            List<T> resources = [];
            DirAccess dir = DirAccess.Open(path);

            if (dir != null)
            {
                dir.ListDirBegin();
                string fileName = dir.GetNext();
                while (fileName != "")
                {
                    if (dir.CurrentIsDir() && fileName != "." && fileName != "..")
                    {
                        resources.AddRange(LoadResourcesFromDirectory<T>(path + "/" + fileName));
                    }
                    else
                    {
                        string resourcePath = path + "/" + fileName.Replace(".import", "").Replace(".remap", "");
                        if (ResourceLoader.Exists(resourcePath) && ResourceLoader.Load(resourcePath) is T resource)
                        {
                            resources.Add(resource);
                        }
                    }
                    fileName = dir.GetNext();
                }
                dir.ListDirEnd();
            }
            return resources;
        }

        public static Dictionary<string, Texture2D> LoadImagesFromDirectory(string path, bool recursive = false)
        {
            var textures = new Dictionary<string, Texture2D>();
            var dir = DirAccess.Open(path);
            if (dir == null)
            {
                GD.PrintErr($"Failed to open directory: {path}");
                return textures;
            }

            dir.ListDirBegin();
            string fileName;
            while ((fileName = dir.GetNext()) != "")
            {
                if (fileName == "." || fileName == "..")
                {
                    continue;
                }

                if (dir.CurrentIsDir())
                {
                    if (recursive)
                    {
                        var nestedPath = Path.Combine(path, fileName);
                        var nestedTextures = LoadImagesFromDirectory(nestedPath, recursive);
                        foreach (var kvp in nestedTextures)
                        {
                            if (!textures.ContainsKey(kvp.Key))
                            {
                                textures[kvp.Key] = kvp.Value;
                            }
                            else
                            {
                                GD.PrintErr($"Duplicate texture key: {kvp.Key}");
                            }
                        }
                    }
                }
                else if (fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                {
                    string filePath = Path.Combine(path, fileName);
                    var texture = ResourceLoader.Load<Texture2D>(filePath);
                    if (texture != null)
                    {
                        string key = Path.GetFileNameWithoutExtension(fileName);
                        if (!textures.ContainsKey(key))
                        {
                            textures[key] = texture;
                        }
                        else
                        {
                            GD.PrintErr($"Duplicate texture key: {key}");
                        }
                    }
                    else
                    {
                        GD.PrintErr($"Failed to load texture: {filePath}");
                    }
                }
            }

            dir.ListDirEnd();
            return textures;
        }

        */
    }

}


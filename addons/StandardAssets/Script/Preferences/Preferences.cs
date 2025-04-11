using Godot;
using System;

namespace Game
{
    public partial class Preferences : Node
    {
        private readonly string savePath = "user://preferences.tres";

        public PreferencesResource Data;

        public event Action PreferencesUpdated;

        public override void _Ready()
        {
            if (ResourceLoader.Exists(savePath))
            {
                Data = (PreferencesResource)ResourceLoader.Load(savePath);
                GD.Print("[Preferences] Data loaded from path: ", savePath);
            }
            else
            {
                Data = new();
                GD.Print("[Preferences] Data not found at path: ", savePath, ", using defaults");
            }

            ApplyAudioPreferences();
            PreferencesUpdated?.Invoke();
            GD.Print("[Preferences] Ready");
        }

        public void SavePreferences()
        {
            Error result = ResourceSaver.Save(Data, savePath);

            if (result != Error.Ok)
            {
                GD.PrintErr("[Preferences] Failed to save preferences to location: " + savePath);
            }
            else
            {
                GD.Print("[Preferences] Saved player preferences to: " + savePath);
            }
        }

        public void ApplyAudioPreferences()
        {
            if (Data.EnableAudio && AudioServer.IsBusMute(AudioServer.GetBusIndex("Master")))
            {
                AudioServer.SetBusMute(AudioServer.GetBusIndex("Master"), false);
            }
            else if (!Data.EnableAudio && !AudioServer.IsBusMute(AudioServer.GetBusIndex("Master")))
            {
                AudioServer.SetBusMute(AudioServer.GetBusIndex("Master"), true);
            }

            if (Data.EnableMusic && AudioServer.IsBusMute(AudioServer.GetBusIndex("Music")))
            {
                AudioServer.SetBusMute(AudioServer.GetBusIndex("Music"), false);
            }
            else if (!Data.EnableMusic && !AudioServer.IsBusMute(AudioServer.GetBusIndex("Music")))
            {
                AudioServer.SetBusMute(AudioServer.GetBusIndex("Music"), true);
            }

            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), Data.MasterVolume);
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Ambient"), Data.AmbientVolume);
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Environment"), Data.EnvironmentVolume);
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music"), Data.MusicVolume);
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("SFX"), Data.SFXVolume);
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("UI"), Data.UIVolume);
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Voice"), Data.VoiceVolume);

            GD.Print("[Preferences] Audio settings restored");
        }

    }

}


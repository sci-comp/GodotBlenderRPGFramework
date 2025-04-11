using Godot;

public partial class PreferencesResource : Resource
{
    [Export] public bool ExtraLuck = false;
    [Export] public bool UseGamepad = false;
    [Export] public bool EnableDebug = false;

    [Export] public bool EnableAudio = true;
    [Export] public bool EnableMusic = true;

    [Export] public float MasterVolume = 0.7f;
    [Export] public float AmbientVolume = 0.7f;
    [Export] public float EnvironmentVolume = 0.7f;
    [Export] public float MusicVolume = 0.7f;
    [Export] public float SFXVolume = 0.7f;
    [Export] public float UIVolume = 0.7f;
    [Export] public float VoiceVolume = 0.7f;
}


using Godot;
using System.Collections.Generic;

namespace Game
{
    public partial class SoundGroup3D : Node
    {
        [Export] public int MaxVoices = 3;
        [Export] public Vector2 VaryPitch = new(0.97f, 1.03f);
        [Export] public Vector2 VaryVolume = new(0.95f, 1.0f);

        private RandomNumberGenerator rnd = new();
        private SFX sfx;

        public List<AudioStreamPlayer3D> AvailableSources = [];
        public List<AudioStreamPlayer3D> ActiveSources = [];

        public int TotalVariations => ActiveSources.Count + AvailableSources.Count;

        public void Initialize(SFX _sfx)
        {
            sfx = _sfx;

            foreach (Node child in GetChildren())
            {
                if (child is AudioStreamPlayer3D audioStreamPlayer3D)
                {
                    audioStreamPlayer3D.Finished += () => OnAudioFinished(audioStreamPlayer3D);
                    AvailableSources.Add(audioStreamPlayer3D);
                }
            }

            if (MaxVoices > AvailableSources.Count)
            {
                MaxVoices = AvailableSources.Count;
            }
        }

        public void OnAudioFinished(AudioStreamPlayer3D src)
        {
            ActiveSources.Remove(src);
            AvailableSources.Add(src);
        }

        public void Stop(AudioStreamPlayer3D src)
        {
            src.Stop();
            ActiveSources.Remove(src);
            AvailableSources.Add(src);
        }

        public AudioStreamPlayer3D GetAvailableSource()
        {
            AudioStreamPlayer3D src;

            // Stop an active source if necessary
            if ((AvailableSources.Count > 0 && ActiveSources.Count >= MaxVoices)
                || AvailableSources.Count == 0)
            {
                src = ActiveSources[0];
                Stop(src);
            }

            int idx = rnd.RandiRange(0, AvailableSources.Count - 1);
            src = AvailableSources[idx];
            src.PitchScale = (float)GD.RandRange(VaryPitch.X, VaryPitch.Y);
            src.VolumeDb = Toolbox.Audio.Linear2Db((float)GD.RandRange(VaryVolume.X, VaryVolume.Y));
            AvailableSources.RemoveAt(idx);
            ActiveSources.Add(src);

            return (src);
        }

    }

}


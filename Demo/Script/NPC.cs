using Godot;
using Godot.Collections;
using System;

namespace Game
{
    public partial class NPC : Node
    {
        /// <summary>
        /// Autoload singleton accessible at /root/NPC
        /// This class is a stub, included here for demo purposes only.
        /// </summary>

        public Dictionary<string, DialogueActor> Actors { get; set; } = [];

        public override void _Ready()
        {
            DialogueActor.ActorSpawned += RegisterActor;
            DialogueActor.ActorDestroyed += UnregisterActor;
        }

        public void RegisterActor(string actorId, DialogueActor actor)
        {
            if (string.IsNullOrEmpty(actorId))
            {
                GD.PrintErr("Attempted to register actor with null or empty ID: ", actorId);
                return;
            }

            if (actor == null)
            {
                GD.PrintErr($"Attempted to register null actor with ID: {actorId}");
                return;
            }

            if (Actors.ContainsKey(actorId))
            {
                GD.PushWarning($"Actor with ID {actorId} is already registered. Updating reference.");
                Actors[actorId] = actor;
                return;
            }

            Actors.Add(actorId, actor);

            GD.Print("[NPC] Actor joined: ", actorId);
        }

        public void UnregisterActor(string actorId)
        {
            if (string.IsNullOrEmpty(actorId))
            {
                GD.PrintErr("[NPC] Attempted to unregister actor with null or empty actorID: ", actorId);
                return;
            }

            if (!Actors.ContainsKey(actorId))
            {
                GD.PushWarning($"[NPC] Attempted to unregister non-existent actor: {actorId}");
                return;
            }

            Actors.Remove(actorId);

            GD.Print("[NPC] Actor left: ", actorId);
        }

        public void Camera(string actorId, string angleName)
        {
            if (!Actors.TryGetValue(actorId, out DialogueActor actor))
            {
                GD.PrintErr($"[NPC] Actor not found: {actorId}");
                return;
            }

            if (Enum.TryParse(angleName, out CameraAngle angle))
            {
                actor.SetCameraAngle(angle);
            }
            else
            {
                GD.Print($"[NPC] Angle {angleName} not found for actor: {actorId}");
            }
        }
    }

}


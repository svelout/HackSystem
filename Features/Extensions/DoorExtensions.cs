using Exiled.API.Features;
using Exiled.API.Features.Doors;
using UnityEngine;

namespace HackSystem.Features.Extensions;

internal static class DoorExtensions
{
    internal static bool RollHack(this Door door)
    {
        return Random.Range(0f, 100f) <= (Plugin.Instance.Config.CustomChance
            .TryGetValue(door.Type, out var value)
            ? value
            : Plugin.Instance.Config.HackChance);
    }
}
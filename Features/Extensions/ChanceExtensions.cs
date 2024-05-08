using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using Exiled.API.Features.Roles;
using PlayerRoles;
using UnityEngine;

namespace HackSystem.Features.Extensions;

internal static class ChanceExtensions
{
    internal static bool RollHack(this Door door)
    {
        return Random.Range(0f, 100f) <= (Plugin.Instance.Config.CustomChanceDoors
            .TryGetValue(door.Type, out var value)
            ? value
            : Plugin.Instance.Config.HackChance);
    }

    internal static bool RollHack(this Role role)
    {
        return Random.Range(0f, 100f) <= (Plugin.Instance.Config.CustomChanceRoles
            .TryGetValue(role.Type, out var value)
            ? value
            : Plugin.Instance.Config.HackChance);
    }

    internal static bool RollHack(this Item item)
    {
        return Random.Range(0f, 100f) <= (Plugin.Instance.Config.CustomChanceItems
            .TryGetValue(item.Type, out var value)
            ? value
            : Plugin.Instance.Config.HackChance);
    }

    internal static bool RollHack(this double obj)
    {
        return Random.Range(0f, 100f) <= obj;
    }
}
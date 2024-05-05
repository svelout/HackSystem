using System.Collections.Generic;
using Exiled.API.Features;
using MEC;

namespace HackSystem.Features;

public static class CurrentObjects
{
    internal static readonly List<CoroutineHandle> CurrentCoroutines = new();
    internal static readonly Dictionary<Player, int> PlayersDoesntHack = new();
}
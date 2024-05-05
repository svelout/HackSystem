using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using AdvancedHintsSvelout;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using HackSystem.Features.Extensions;
using MEC;
using PlayerRoles.PlayableScps.Scp106;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HackSystem.Features;

internal sealed class Hack
{
    private const char _loadingSymbol = '\u25a0';
    private const int _fullLoadingSymbolsCount = 10;

    public static List<Hack> CurrentHacks => new();
    
    public Player Hacker { get; }
    
    public Door Target { get; }
    
    public bool IsHacking { get; internal set; }

    public Hack(Player hacker, Door target)
    {
        Hacker = hacker;
        Target = target;
        _instanceList.Add(this);
    }

    private CoroutineHandle _progressBarCoroutine;
    private static List<Hack> _instanceList = new();

    public static Hack Get(Player hacker)
    {
        return _instanceList.Find(h => h.Hacker == hacker);
    }

    public static Hack Get(Door door)
    {
        return _instanceList.Find(h => h.Target == door);
    }
    public void Start()
    {
        if (!IsHacking && _progressBarCoroutine.Tag != Hacker.Nickname)
        {
            _progressBarCoroutine = Timing.RunCoroutine(ProgressBar(Hacker));
            _progressBarCoroutine.Tag = Hacker.Nickname;
            IsHacking = true;
            CurrentHacks.Add(this);
        }
    }

    private void Destroy()
           => _instanceList.Remove(this);
    
    private void End(bool success)
    {
        KillProgressBarCoroutine();
        CurrentHacks.Remove(this);
        if (success)
        {
            if (Plugin.Instance.Config.IsDoorsTimelyUnlock)
            {
                var openTime = Plugin.Instance.Config.CustomDoorsOpenTime.TryGetValue(Target.Type, out var time)
                    ? (float)time
                    : Plugin.Instance.Config.OpensTime;
                Target.Unlock(openTime, DoorLockType.AdminCommand);
                Timing.RunCoroutine(TimelyUnlocking(Target, openTime));
            }
            else
            {
                Target.Unlock();
                Target.IsOpen = true;
            }
        }
        IsHacking = false;
        if (Plugin.Instance.Config.CustomCooldownRoles.TryGetValue(Hacker.Role, out var role))
        {
            CurrentObjects.PlayersDoesntHack.Add(Hacker, role);
        }
        else if (Plugin.Instance.Config.CustomCooldownItems.TryGetValue(Hacker.CurrentItem.Type, out var item))
        {
            CurrentObjects.PlayersDoesntHack.Add(Hacker, item);
        }
        else
        {
            CurrentObjects.PlayersDoesntHack.Add(Hacker, Plugin.Instance.Config.HackCooldown);
        }
        Timing.RunCoroutine(CooldownWaiter(Hacker));
    }

    private void KillProgressBarCoroutine()
    {
        if (_progressBarCoroutine.Tag != null)
        {
            Timing.KillCoroutines(_progressBarCoroutine);
            _progressBarCoroutine.Tag = null;
        }
    }

    private IEnumerator<float> CooldownWaiter(Player player)
    {
        for (int i = CurrentObjects.PlayersDoesntHack[player]; i > 0; i--)
        {
            CurrentObjects.PlayersDoesntHack[player] = i;
            yield return Timing.WaitForSeconds(1f);
        }

        CurrentObjects.PlayersDoesntHack.Remove(Hacker);
    }

    private IEnumerator<float> TimelyUnlocking(Door door, float time)
    {
        door.IsOpen = true;
        yield return Timing.WaitForSeconds(time);
        door.IsOpen = false;
    }


    private IEnumerator<float> ProgressBar(Player player)
    {
        float time = Plugin.Instance.Config.CustomHackTime.ContainsKey(Target.Type) 
            ? (float)Plugin.Instance.Config.CustomHackTime[Target.Type] 
            : (float)Plugin.Instance.Config.InternalHackTime;
        float loadingSpeed = time / _fullLoadingSymbolsCount;
        string message = Plugin.Instance.Config.CurrentHackingText + '\n';
        Vector3 playerPos = player.Position;
        int? posToStop = Plugin.Instance.Config.ChancesEnabled
            ? Target.RollHack() ? (int)Random.Range(3f, _fullLoadingSymbolsCount) : null
            : null;
        bool isSucccessed = true;
        for (int i = 0; i <= _fullLoadingSymbolsCount; i++)
        {
            if (posToStop != null && posToStop == i)
            {
                player.ShowManagedHint(Plugin.Instance.Config.FailedHackText);
                isSucccessed = false;
                break;
            }
            var currentPos = player.Position;
            if (currentPos != playerPos)
            {
                KillProgressBarCoroutine();
                yield break;
            }
            message += _loadingSymbol;
            player.ShowManagedHint(message, 0.5f, overrideQueue: true);
            yield return Timing.WaitForSeconds(loadingSpeed);
        }
        player.ShowManagedHint(string.Empty, 1f, overrideQueue: false);
        End(isSucccessed);
    }
}
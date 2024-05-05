using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AdvancedHintsSvelout;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using HackSystem.Features;
using MEC;
using PlayerRoles;
using Utils.NonAllocLINQ;
using YamlDotNet.Core.Tokens;

namespace HackSystem
{
    internal sealed class EventHandlers
    {
        // Раунд начинается
        public void OnRoundStarted()
        {
            // Блокировка всех дверей указанных в конфиге
            if (Plugin.Instance.Config.DoorsToLock.Count > 0)
            {
                foreach (var doorType in Plugin.Instance.Config.DoorsToLock)
                {
                    var door = Door.Get(doorType);
                    door.ChangeLock(DoorLockType.AdminCommand);
                }
            }
        }
        
        // Игрок взаимодействует с дверью (до основного кода)
        public void OnInteractingDoors(InteractingDoorEventArgs ev)
        {
            // Проверка закрыта ли дверь
            if (!ev.IsAllowed && !ev.Door.IsOpen && (Hack.Get(ev.Door) == null || !Hack.Get(ev.Door).IsHacking))
            {
                // Проверка на кулдаун
                if (CurrentObjects.PlayersDoesntHack.TryGetValue(ev.Player, out var cooldown))
                {
                    ev.Player.ShowManagedHint(Plugin.Instance.Config.CooldownText.Replace("%TIME%", 
                        cooldown.ToString()), 2.5f, overrideQueue: false);
                }
                // Проверка нажал ли игрок второй раз кнопку
                else if (CurrentObjects.CurrentCoroutines.Any(c => c.Tag == ev.Player.Nickname))
                {
                    ev.Player.ShowManagedHint(string.Empty, 0.1f, overrideQueue: false);
                    var hack = new Hack(ev.Player, ev.Door);
                    hack.Start();
                }
                // Проверка имеет ли игрок подходящую роль или предмет указанную в конфиге чтобы началть взлом
                else if (Plugin.Instance.Config.HackRoles.Contains(ev.Player.Role)
                         || (ev.Player.CurrentItem != null && Plugin.Instance.Config.HackItems.Contains(ev.Player.CurrentItem.Type)))
                {
                    ev.Player.ShowManagedHint(Plugin.Instance.Config.CanStartText, 2f, overrideQueue: false);
                    var coroutine = Timing.RunCoroutine(WaitingInput());
                    coroutine.Tag = ev.Player.Nickname;
                    CurrentObjects.CurrentCoroutines.Add(coroutine);
                }
            }
        }

        // Ожидание...
        private IEnumerator<float> WaitingInput()
        {
            yield return Timing.WaitForSeconds(2f);
        }
     }
}
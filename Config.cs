using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Enums;
using Exiled.API.Features.Doors;
using Exiled.API.Interfaces;
using PlayerRoles;

namespace HackSystem
{
  public class Config : IConfig
  {
      public bool IsEnabled { get; set; } = true;
      public bool Debug { get; set; } = false;
      
      [Description("List of doors whose will be unlock only by hacking before starting round")]
      public List<DoorType> DoorsToLock { get; set; } = new()
      {
          DoorType.Scp096
      };
      
      #region HackCooldownSettings
      [Description("internal hack cooldown (int)")] 
      public int HackCooldown { get; set; } = 10;

      [Description("Hack cooldown for roles(int)")]
      public Dictionary<RoleTypeId, int> CustomCooldownRoles { get; set; } = new()
      {

      };

      [Description("Hack cooldown for items (int)")]
      public Dictionary<ItemType, int> CustomCooldownItems { get; set; } = new()
      {

      };
      #endregion

      #region UnlockTimely

      [Description("Is doors unlock on time")]
      public bool IsDoorsTimelyUnlock { get; set; } = false;

      [Description("Internal time for open doors")]
      public float OpensTime { get; set; } = 3f;

      [Description("Custom time for open doors")]
      public Dictionary<DoorType, double> CustomDoorsOpenTime { get; set; } = new()
      {
          {
              DoorType.Scp049Gate,
              20
          }
      };

      #endregion
      
      #region Chances System

      [Description("Is Chances is enabled")] 
      public bool ChancesEnabled { get; set; } = false;

      [Description("Internal chance for all locked doors")]
      public double HackChance { get; set; } = 5f;

      [Description("Custom chance for locked door (Enum DoorType)")]
      public Dictionary<DoorType, double> CustomChance { get; set; } = new()
      {
          {
              DoorType.Intercom,
              40f
          }
      };

      #endregion

      #region PermissionsToHack
      [Description("Roles whose can use hack system(SCP, SPECTATOR, OVERWATCH NOT WORK)")]
      public List<RoleTypeId> HackRoles { get; set; } = new()
      {
          RoleTypeId.Scientist
      };

      [Description("Items which can use hack system")]
      public List<ItemType> HackItems { get; set; } = new()
      {
          ItemType.Coin
      };
      
      #endregion

      #region TimeHack
      [Description("Internal time to hack door in seconds (float) ")]
      public double InternalHackTime { get; set; } = 5;

      [Description("Custom time for every door(Door which not contains in this list will be hack for `InternalHackTime`) (Enum DoorType)")]
      public Dictionary<DoorType, double> CustomHackTime { get; set; } = new()
      {
          {
              DoorType.Scp096,
              15
          }
      };
      
      #endregion
      
      #region Hints
      
      [Description("Text whose will show when player try to interacting locked door")]
      public string CanStartText { get; set; } = "Нажмите повторно E чтобы начать взламывать дверь";

      [Description("Text whose will show when player will be start hacking")]
      public string CurrentHackingText { get; set; } = "Идет взламывание двери";

      [Description("Text whose will show when player try to hack, but he is in cooldown, %TIME% - current seconds of cooldown")]
      public string CooldownText { get; set; }= "Подождите еще %TIME% секунд";

      [Description("Text whose will show when player try to hack with ChancesEnabled option and it failed")]
      public string FailedHackText { get; set; } = "Неудачно!";

      #endregion
  }
}
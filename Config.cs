using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Enums;
using Exiled.API.Features.Doors;
using Exiled.API.Interfaces;
using PlayerRoles;
using YamlDotNet.Serialization;

namespace HackSystem
{
  public class Config : IConfig
  {
      
      [Description("Is plugin enabled")]
      public bool IsEnabled { get; set; } = true;
      
      [Description("Is debug enabled")]
      public bool Debug { get; set; } = false;
      
      [Description("List of doors whose will be unlock only by hacking before starting round")]
      public List<DoorType> DoorsToLock { get; set; } = new()
      {
          DoorType.Scp173Armory
      };
      
      #region HackCooldownSettings
      [Description("internal hack cooldown (int)")] 
      public int HackCooldown { get; set; } = 10;

      [Description("Hack cooldown for roles(int) ROLE MUST BE IN `hack_roles`")]
      public Dictionary<RoleTypeId, int> CustomCooldownRoles { get; set; } = new()
      {
          {
              RoleTypeId.Scientist, 5
          }
      };

      [Description("Hack cooldown for items (int) ITEM MUST BE IN `hack_items`")]
      public Dictionary<ItemType, int> CustomCooldownItems { get; set; } = new()
      {
          {
              ItemType.KeycardChaosInsurgency, 5
          }
      };
      #endregion

      #region UnlockTimely

      [Description("Is doors unlock temporarily")]
      public bool IsDoorsTimelyUnlock { get; set; } = false;

      [Description("Internal time during which the door will be open and then closed")]
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

      [Description("Custom chance for locked door (Enum DoorType) This chance is taken into account last after the item and role!")]
      public Dictionary<DoorType, double> CustomChanceDoors { get; set; } = new()
      {
          {
              DoorType.Intercom,
              40f
          }
      };

      [Description("Custom chance for hacking role (Enum RoleTypeId) (ROLE MUST BE IN `hack_roles`)")]
      public Dictionary<RoleTypeId, double> CustomChanceRoles { get; set; } = new()
      {
          {
              RoleTypeId.Scientist, 50
          }
      };
      
      [Description("Custom chance for hacking item (Enum ItemType) (ITEM MUST BE IN `hack_items`) \u26a0\ufe0f" +
                   "IF A PLAYER WITH A HACKING ROLE WITH A CUSTOM CHANCE STARTS HACKING AT THE SAME TIME HOLDING A HACKING ITEMS WITH A UNIQUE CHANCE, THEN THE CHANCE WILL BE CONSIDERED ONLY WHICH IS SPECIFIED IN THE ROLE\u26a0\ufe0f")]
      public Dictionary<ItemType, double> CustomChanceItems { get; set; } = new()
      {
          {
              ItemType.Coin, 20
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

      #region TimeDuration
      [Description("Internal time to hack door in seconds (float) ")]
      public double InternalHackDuration { get; set; } = 5;

      [Description("Custom time for every door(Door which not contains in this list will be hack for `InternalHackTime`) (Enum DoorType)")]
      public Dictionary<DoorType, double> CustomHackDuration { get; set; } = new()
      {
          {
              DoorType.Scp096,
              15
          }
      };
      
      #endregion
      
      #region Hints
      
      [Description("Text whose will show when player try to interacting locked door")]
      public string CanStartText { get; set; } = "Press E again to start hacking the door";

      [Description("Text whose will show when player will be start hacking")]
      public string CurrentHackingText { get; set; } = "Hacking door";

      [Description("Text whose will show when player try to hack, but he is in cooldown, %TIME% - current seconds of cooldown")]
      public string CooldownText { get; set; } = "Wait for %TIME% seconds";

      [Description("Text whose will show when player try to hack with `ChancesEnabled` option and it failed")]
      public string FailedHackText { get; set; } = "Unsuccessful!";

      #endregion
  }
}
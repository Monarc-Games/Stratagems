using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Oxide.Plugins
{
    [Info("Stratagems", "Monarc Games", "1.0.0")]
    [Description("Call in a wide variety of powerful ordnance and war materials")]
    public class Stratagems : RustPlugin
    {
        #region Fields

        private static Stratagems _instance;

        // Prefabs
        private readonly string _horizontalWeaponRackPrefab = "assets/prefabs/deployable/weaponracks/weaponrack_horizontal.deployed.prefab";

        
        #endregion
        
        #region Initialisation

        

        #endregion
        
        #region Hooks

        

        #endregion
        
        #region Commands

        

        #endregion

        #region Helpers

        private void SpawnPodEntity(Vector3 targetPosition, Quaternion targetRotation)
        {
            var parent = GameManager.server.CreateEntity(_horizontalWeaponRackPrefab, targetPosition + new Vector3(100f, 600f, 200f), Quaternion.Euler(90f, 180f, 0f));
            if (parent == null)
                return;
                
            parent.Spawn();

            var podEntity = parent.gameObject.AddComponent<PodEntity>();
            podEntity.Init(parent, parent.transform.position + new Vector3(0f, 1f, 0f), targetPosition, parent.transform.rotation, targetRotation);
        }

        #endregion

        #region Stratagems Pod Entity

        // This class is where all the main functionality of how the Stratagems Pod (the thing that falls out of the sky) works.
        private class PodEntity : BaseEntity
        {
            private BaseEntity _parent;
            private readonly string _horizontalWeaponRackPrefab = "assets/prefabs/deployable/weaponracks/weaponrack_horizontal.deployed.prefab";
            private readonly string _tallWeaponRackPrefab = "assets/prefabs/deployable/weaponracks/weaponrack_tall.deployed.prefab";
            private readonly string _flamePrefab = "assets/prefabs/weapons/flamethrower/flamethrower.entity.prefab";
            private readonly string _sheetmetalShortname = "sheetmetal";
            private readonly string _mlrsRocketShortname = "ammo.rocket.mlrs";
            private readonly string _roadsignsShortname = "roadsigns";
            
            // This is where the pod entity is called.
            public void Init(BaseEntity parentEnt, Vector3 startPos, Vector3 targetPos, Quaternion startRot, Quaternion targetRot)
            {
                _parent = parentEnt;

                BaseEntity bottomEntity;
                BaseEntity entity;
                List<BaseEntity> entitiesToDelete;

                SpawnPodEntities(startPos, out entity, out entitiesToDelete, out bottomEntity);
            }
            
            // Spawns all the entities of the pod.
            private void SpawnPodEntities(Vector3 startPos, out BaseEntity ent, out List<BaseEntity> entities, out BaseEntity bottomEnt)
            {
                var tallWeaponRackBack = SpawnPart(_tallWeaponRackPrefab, startPos + new Vector3(-0.013f, -0.171f, -0.779f), Quaternion.Euler(0f, 0f, 180f));
                var tallWeaponRackRight = SpawnPart(_tallWeaponRackPrefab, startPos + new Vector3(0.737f, -0.172f, -0.029f), Quaternion.Euler(0f, 270f, 180f));
                var tallWeaponRackFront = SpawnPart(_tallWeaponRackPrefab, startPos + new Vector3(-0.013f, -0.172f, 0.721f), Quaternion.Euler(0f, 180f, 180f));
                var tallWeaponRackLeft = SpawnPart(_tallWeaponRackPrefab, startPos + new Vector3(-0.763f, -0.172f, -0.029f), Quaternion.Euler(0f, 90f, 180f));
                var horizontalWeaponRackBottom = SpawnPart(_horizontalWeaponRackPrefab, startPos + new Vector3(0f, 0f, 0f), Quaternion.Euler(90f, 90f, 0f));
                var horizontalWeaponRackBack = SpawnPart(_horizontalWeaponRackPrefab, startPos + new Vector3(0f, 0.865f, -1.500f), Quaternion.Euler(275f, 180f, 0f));
                var horizontalWeaponRackRight = SpawnPart(_horizontalWeaponRackPrefab, startPos + new Vector3(1.500f, 0.865f, 0f), Quaternion.Euler(275f, 90f, 0f));
                var horizontalWeaponRackFront = SpawnPart(_horizontalWeaponRackPrefab, startPos + new Vector3(0f, 0.865f, 1.500f), Quaternion.Euler(275f, 0f, 0f));
                var horizontalWeaponRackLeft = SpawnPart(_horizontalWeaponRackPrefab, startPos + new Vector3(-1.500f, 0.865f, 0f), Quaternion.Euler(275f, 270f, 0f));
                var horizontalWeaponRackInside = SpawnPart(_horizontalWeaponRackPrefab, startPos + new Vector3(0f, 0.930f, 0f), Quaternion.Euler(90f, 90f, 0f));
                var mlrsRocketBack = SpawnWorldModel(_mlrsRocketShortname, startPos + new Vector3(0f, -0.300f, -0.900f), Quaternion.Euler(280f, 0f, 0f));
                var mlrsRocketRight = SpawnWorldModel(_mlrsRocketShortname, startPos + new Vector3(0.900f, -0.300f, 0f), Quaternion.Euler(280f, 270f, 0f));
                var mlrsRocketFront = SpawnWorldModel(_mlrsRocketShortname, startPos + new Vector3(0f, -0.300f, 0.900f), Quaternion.Euler(280f, 180f, 0f));
                var mlrsRocketLeft = SpawnWorldModel(_mlrsRocketShortname, startPos + new Vector3(-0.900f, -0.300f, 0f), Quaternion.Euler(280f, 90f, 0f));
                var flameBack = SpawnPart(_flamePrefab, startPos, Quaternion.Euler(180f, 0f, 180f));
                var flameRight = SpawnPart(_flamePrefab, startPos, Quaternion.Euler(180f, 270f, 180f));
                var flameFront = SpawnPart(_flamePrefab, startPos, Quaternion.Euler(180f, 180f, 180f));
                var flameLeft = SpawnPart(_flamePrefab, startPos, Quaternion.Euler(180f, 90f, 180f));
                
                _parent.SendChildrenNetworkUpdateImmediate();
         
                bottomEnt = horizontalWeaponRackBottom;
                ent = horizontalWeaponRackInside;
                entities = new List<BaseEntity>()
                {
                    flameBack,
                    flameRight,
                    flameFront,
                    flameLeft
                };
            }
            
            // This function spawns only .prefab things, helper function to help spawning crap.
            private BaseEntity SpawnPart(string prefab, Vector3 position = default, Quaternion rotation = default)
            {
                var entity = GameManager.server.CreateEntity(prefab, position, rotation);
                if (entity == null)
                    return null;
                
                entity.Spawn();
                entity.SetParent(_parent, true, true);
                return entity;
            }
            
            // Spawns WORLD MODELS. We cannot spawn world models with the regular SpawnPart() function, as world models are special, and require special treatment, such as making sure the world model is fixed in position.
            private DroppedItem SpawnWorldModel(string prefab, Vector3 position = default, Quaternion rotation = default)
            {
                DroppedItem worldModel = ItemManager.CreateByName(prefab, 1)?.Drop(position, Vector3.zero, rotation)?.GetComponent<DroppedItem>();
                if (worldModel == null)
                    return null;
                
                // We need to get the rigidbody of the world model so we can make sure it does not fall to the ground. isKinematic = true is required to make it fixed at a position.
                Rigidbody rigidbody = worldModel.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    rigidbody.useGravity = false;
                    rigidbody.isKinematic = true;
                }

                worldModel.syncPosition = false;
                worldModel.enableSaving = true;
                worldModel.allowPickup = false;
                worldModel.CancelInvoke(worldModel.IdleDestroy);
                worldModel.SetParent(_parent, true, true);

                return worldModel;
            }
        }

        #endregion

        #region Animator / Curve

        

        #endregion
        
        #region User Interface

        

        #endregion
        
        #region Data Management

        

        #endregion
        
        #region Localisation

        

        #endregion
        
        #region Configuration

        private ConfigData config;

        private class ConfigData
        {
            [JsonProperty(PropertyName = "Debug")]
            public bool debug { get; set; }
            
            [JsonProperty(PropertyName = "General Settings")]
            public GeneralSettings generalSettings { get; set; }
            
            [JsonProperty(PropertyName = "Chat Settings")]
            public ChatSettings chatSettings { get; set; }
        }

        private class GeneralSettings
        {
            [JsonProperty(PropertyName = "Main command")]
            public string mainCommand { get; set; }
        }

        private class ChatSettings
        {
            [JsonProperty(PropertyName = "Chat icon (Steam ID)")]
            public ulong iconID { get; set; }

            [JsonProperty(PropertyName = "Chat prefix")]
            public string chatPrefix { get; set; }

            [JsonProperty(PropertyName = "Chat prefix colour")]
            public string chatPrefixColour { get; set; }
        }

        private class UISettings
        {
            
        }

        private class PodSettings
        {
            [JsonProperty(PropertyName = "Allow stratagem pod to take damage")]
            public bool canTakeDamage { get; set; }
            
            [JsonProperty(PropertyName = "Stratagem pod health")]
            public float podHealth { get; set; }
        }
        
        protected override void LoadConfig()
        {
            base.LoadConfig();
            try
            {
                config = Config.ReadObject<ConfigData>();
                if (config == null)
                {
                    LoadDefaultConfig();
                }
            }
            catch
            {
                PrintError($"{Name}.json is corrupted! Recreating a new configuration");
                LoadDefaultConfig();
                return;
            }
            SaveConfig();
        }

        protected override void LoadDefaultConfig()
        {
            config = new ConfigData
            {
                debug = false,
                generalSettings = new GeneralSettings()
                {
                    mainCommand = "stratagems"
                },
                
                chatSettings = new ChatSettings()
                {
                    iconID = 0,
                    chatPrefix = "STRATAGEMS: ",
                    chatPrefixColour = "#8B4343"
                },
            };
        }


        protected override void SaveConfig() => Config.WriteObject(config);

        #endregion

    }
}
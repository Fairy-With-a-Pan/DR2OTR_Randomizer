using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DR2OTR_Randomizer.Resources
{
    public class AllItemStatData
    {
        /// <summary>
        /// Use this to add all the data for the item stats
        /// </summary>
        public List<ItemStatsData> GetVheicleStats()
        {
            var list = new List<ItemStatsData>();
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Vehicle Air Density:",
                StatDescription = "Controls how much air resistance there is for the vehicle. The lower the less air resistance.",
                StatMin = 0,
                StatMax = 10,
                StatInGameName = "\tAirDensity"

            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Vehicle Max RPM:",
                StatDescription = "One of the stats that controls the vehicles speed. May start to auto acelerate at higher values.",
                StatMin = 3550,
                StatMax = 10000,
                StatInGameName = "\tEngine_MaxRPM"
            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Vehicle Min RPM:",
                StatDescription = "Gives a burst of speed when first acelerating. Setting higher can cause it to auto acelerate.",
                StatMin = 885,
                StatMax = 5000,
                StatInGameName = "\tEngine_MinRPM"

            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Vehicle Max Torque:",
                StatDescription = "Similar to max rpm but with a little less control and bit more kick.",
                StatMin = 100,
                StatMax = 1000,
                StatInGameName = "\tEngine_MaxTorque"

            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Vehicle Opt RPM:",
                StatDescription = "Give increased speed while keeping some control when set high.",
                StatMin = 3473,
                StatMax = 10000,
                StatInGameName = "\tEngine_OptRPM"

            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Torque Factor At Max ",
                StatDescription = "Will greatly increase the vehicles speed the higher you set.",
                StatMin = 1,
                StatMax = 1000,
                StatInGameName = "\tEngine_TorqueFactorAtMaxRPM"

            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Torque Factor At Min",
                StatDescription = "If this gets set to high the vehicle will become very slow and hard to steer.",
                StatMin = 1,
                StatMax = 100,
                StatInGameName = "\tEngine_TorqueFactorAtMinRPM"

            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Min Speed For Wheelies",
                StatDescription = "Recomended to give this a higher value having it to low makes controling bike very hard.",
                StatMin = 10000,
                StatMax = 100000,
                StatInGameName = "\tMinVehicleSpeedForWheeliesAndEndos"

            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Min Speed To Eject Player",
                StatDescription = "Recomended to set fairly high unless you like getting thrown off bikes",
                StatMin = 10000,
                StatMax = 100000,
                StatInGameName = "\tMinVehicleSpeedToEjectPlayer"

            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Vehicle Top Speed",
                StatDescription = "Setting high will make the vehicle go very slow but gradually get faster. Setting very low makes it freak out",
                StatMin = 1,
                StatMax = 1000,
                StatInGameName = "\tVehicleTopSpeed"

            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Extra Torque Factor",
                StatDescription = "Setting this very high will cause the vehicle to spin the higher the faster it spins.",
                StatMin = 1,
                StatMax = 10,
                StatInGameName = "\tExtraTorqueFactor"

            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Skateboard Initial Speed",
                StatDescription = "The launch speed for the skate board setting this very high will teleport",
                StatMin = 1,
                StatMax = 100,
                StatInGameName = "\tInitialSpeed"

            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Skateboard Ride Speed",
                StatDescription = "The maxim speed the skateboard can go you will keep going faster as kick of the floor",
                StatMin = 1,
                StatMax = 100,
                StatInGameName = "\tMaxRideSpeed"

            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Max Rotate Speed",
                StatDescription = "The max speed the skateboard will rotate at",
                StatMin = 1,
                StatMax = 100,
                StatInGameName = "\tMaxRotationSpeed"

            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Skateboard Rotate Speed",
                StatDescription = "The speed the Skateboard will rotate\r\n(will not rotate faster then what max rotate speed is set to)",
                StatMin = 1,
                StatMax = 100,
                StatInGameName = "\tRotationSpeed"

            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Skateboard Eject Speed",
                StatDescription = "The requied speed to be knocked off when hitting a wall or object.",
                StatMin = 1,
                StatMax = 1000,
                StatInGameName = "EjectSpeed"

            });

            return list;
        }
        public List<ItemStatsData> GetNPCStats()
        {
            var list = new List<ItemStatsData>();

            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Damage to Enemys\r\n(vehicle)",
                StatDescription = "The base damage done when hitting an enemy npc with a vehicle",
                StatMin = 1,
                StatMax = 100,
                StatInGameName = "\tDamageBOSS_Flat"
            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Speed need to damage \r\nenemys",
                StatDescription = "The minimum speed needed to deal damage to a enemy",
                StatMin = 1,
                StatMax = 100,
                StatInGameName = "\tDamageBOSS_MinSpeed"
            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Damage done per KMH\r\n(Enemys)",
                StatDescription = "The faster you go the more damage you do to an enemy",
                StatMin = 1,
                StatMax = 100,
                StatInGameName = "\tDamageBOSS_PerKMH"
            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Speed need to damage \r\nSurvivors\r\n",
                StatDescription = "The minimum speed needed to deal damage to a survivors",
                StatMin = 1,
                StatMax = 100,
                StatInGameName = "\tDamageSURV_MinSpeed"
            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Damage done per KMH\r\n(Survivors)\r\n",
                StatDescription = "The fast you go the damage is done to survivor",
                StatMin = 1,
                StatMax = 100,
                StatInGameName = "\tDamageSURV_PerKMH"
            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "NPC Health Amount",
                StatDescription = "This will efect all npc both enemy and frendly npcs",
                StatMin = 1,
                StatMax = 100,
                StatInGameName = "\tHealthAmount"
            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "NPC Photo PP amount",
                StatDescription = "The max amount of PP you get from photos of NPC",
                StatMin = 1,
                StatMax = 100,
                StatInGameName = "\tMax_pp"
            });

            return list;
        }
        public List<ItemStatsData> GetFireArmsStats()
        {
            var list = new List<ItemStatsData>();

            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Fire Rate",
                StatDescription = "Speed the weapon will fire at",
                StatMin = 1,
                StatMax = 10000,
                StatInGameName = "\tFiringRate"
            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Bullet Damage",
                StatDescription = "Damage for for fire arms.",
                StatMin = 1,
                StatMax = 1000,
                StatInGameName = "\tBaseBulletDamage"
            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Max Bullet Distance",
                StatDescription = "Max range for fire arms.",
                StatMin = 1,
                StatMax = 1000,
                StatInGameName = "\tMaxBulletDistance"
            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Number of Bullets Per Shot",
                StatDescription = "This can cause some weapons to bug just drop and pick them back up to fix it.",
                StatMin = 1,
                StatMax = 50,
                StatInGameName = "\tNumPelletsPerShot"
            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Burst Bullet Count",
                StatDescription = "Number of bullest fired from a burst shot.",
                StatMin = 1,
                StatMax = 25,
                StatInGameName = "\tMaxBurstBulletCount"
            });

            return list;
        }
        public List<ItemStatsData> GetWorldStats()
        {
            var list = new List<ItemStatsData>();

            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Durability",
                StatDescription = "The health for items and the amount of ammo fire arms have.",
                StatMin = 1,
                StatMax = 10000,
                StatInGameName = "\tDurability"
            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Money PickUp Amount",
                StatDescription = "The amount of money the player is given when pick up cash.\r\n(There is 9 types of pick up)",
                StatMin = 1,
                StatMax = 50000,
                StatInGameName = "\tAmount"
            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Payout Amount",
                StatDescription = "The payout you get for winning a slot machine",
                StatMin = 1,
                StatMax = 10000,
                StatInGameName = "\tPayoutAmount "
            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Payout Count",
                StatDescription = "How many times you can win on a slot machine.",
                StatMin = 1,
                StatMax = 100,
                StatInGameName = "\tPayoutCount "
            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "PP Awarded",
                StatDescription = "The amount of PP awarded for completeing a minigame.",
                StatMin = 1,
                StatMax = 2500,
                StatInGameName = "\tPPAward "
            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "PP For Photos",
                StatDescription = "The amount of PP awarded for taking pictures of genres with your camera.",
                StatMin = 1,
                StatMax = 2000,
                StatInGameName = "\tPPAmount "
            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "PP For Weapon Kills",
                StatDescription = "The amount of PP awarded for killing zombies with weapons.",
                StatMin = 1,
                StatMax = 5000,
                StatInGameName = "\tPrestigePointsAwarded"
            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "PP Multiplier",
                StatDescription = "How much PP is mulitiplied when you own that weapns combo card",
                StatMin = 1,
                StatMax = 10,
                StatInGameName = "\tPP_Multiplier"
            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Price (low)",
                StatDescription = "The low price for interactables.",
                StatMin = 1,
                StatMax = 5000,
                StatInGameName = "\tCost "
            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Price (mid)",
                StatDescription = "The medium price for interactables.",
                StatMin = 1,
                StatMax = 7500,
                StatInGameName = "\tCost2"
            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Price (high)",
                StatDescription = "The high price for interactables.",
                StatMin = 1,
                StatMax = 10000,
                StatInGameName = "\tCost3"
            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Throw Force",
                StatDescription = "How far you can throw an item you have picked up. This is diffrent for each item",
                StatMin = 1,
                StatMax = 1000,
                StatInGameName = "\tThrowForce"
            });

            return list;
        }
        public List<ItemStatsData> GetExplosivesStats()
        {
            var list = new List<ItemStatsData>();

            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Spray Item Ammo",
                StatDescription = "The ammount of ammo spray items have. This shows up in came as a precentage",
                StatMin = 1,
                StatMax = 500,
                StatInGameName = "\tSpraySupply"
            });
            
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Explosive Radius",
                StatDescription = "This will affect everything that explodes including explosive firearms like the rocket launcher.",
                StatMin = 1,
                StatMax = 250,
                StatInGameName = "\tExplodeRadius"
            });
            
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Explosive Damage",
                StatDescription = "As long as you have more than 1 bar of HP you will not die instantly to explosions. (Most of the time)",
                StatMin = 1,
                StatMax = 250,
                StatInGameName = "\tMaxDamage"
            });
            
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Detonation Delay",
                StatDescription = "Time till explosives detonate this includes projectile explosives.",
                StatMin = 0,
                StatMax = 5,
                StatInGameName = "\tDetonationDelay "
            });
            
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Projectile Spin",
                StatDescription = "How much spin a projectile as once its been shot",
                StatMin = 1,
                StatMax = 10,
                StatInGameName = "\tPropelledSpinImpulse"
            });
            
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Throwables Ammo",
                StatDescription = "Ammo for all throwables",
                StatMin = 1,
                StatMax = 500,
                StatInGameName = "\tNumberOfThrowables"
            });

            return list;
        }
        public List<ItemStatsData> GetFoodAndDamageStats()
        {
            var list = new List<ItemStatsData>();

            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "All Melee Damage",
                StatDescription = "The damage amount for all melee attacks including zombies and NPC's.",
                StatMin = 1,
                StatMax = 500,
                StatInGameName = "\tAttackDamage"
            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Health Restore Amount",
                StatDescription = "The amount of health restored by eating food items (every 100 equals one health bar).",
                StatMin = 1,
                StatMax = 1000,
                StatInGameName = "\tHealthBoost"
            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Thrown Item Damage",
                StatDescription = "Amount of damage done when an item as be thrown",
                StatMin = 1,
                StatMax = 500,
                StatInGameName = "\tMaxProjectileDamage"
            });
            list.Add(new ItemStatsData()
            {
                StatState = false,
                StatName = "Max Damage Attack",
                StatDescription = "The maximum damage for each hit this affects everything apart from firearms and explosives.",
                StatMin = 1,
                StatMax = 100,
                StatInGameName = "\tMaxDamageDealtPerAttack"
            });

            return list;
        }
    }
}

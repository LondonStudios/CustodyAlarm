using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpConfig;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;

namespace CustodyAlam
{
    public class Main : BaseScript
    {
        public Dictionary<string, Vector3> CustodyLocations { get; set; }
        public float SoundRadius { get; set; }
        public float SoundVolume { get; set; }
        public bool AlarmActivated { get; set; }
        public string PlayingAlarm { get; set; }

        public Main()
        {
            ConfigReader();

            // Stop = true/false
            EventHandlers["Client:CustodyAlarm"] += new Action<Vector3, string, bool>((alarmLocation, alarmName, stop) =>
            {
                if (!stop)
                {
                    Vector3 playerCoords = Game.Player.Character.Position;
                    var distance = Vdist(playerCoords.X, playerCoords.Y, playerCoords.Z, alarmLocation.X, alarmLocation.Y, alarmLocation.Z);
                    float distanceVolumeMultiplier = (SoundVolume / SoundRadius);
                    float distanceVolume = SoundVolume - (distance * distanceVolumeMultiplier);
                    if (distance <= SoundRadius)
                    {
                        if (PlayingAlarm != alarmName)
                        {
                            PlayingAlarm = alarmName;
                        }
                        SendNuiMessage(string.Format("{{\"submissionType\":\"custodyAlarm\", \"submissionVolume\":{0}, \"submissionFile\":\"{1}\"}}", (object)distanceVolume, "custodyalarm.ogg"));
                    }
                }
                else
                {
                    if (PlayingAlarm == alarmName)
                    {
                        AlarmActivated = false;
                        PlayingAlarm = "";
                        SendNuiMessage(string.Format("{{\"submissionType\":\"custodyAlarm\", \"submissionVolume\":{0}, \"submissionFile\":\"{1}\"}}", 0f, ""));
                        Screen.ShowNotification($"~r~{alarmName} ~w~custody alarm has been ~g~deactivated.");
                    }
                }
                
            });

            EventHandlers["Client:CustodyAlert"] += new Action<Vector3, string>((alarmLocation, alarmName) =>
            {
                Screen.ShowNotification($"~r~Alert~w~: {alarmName} custody alarm activated.");
                ClearGpsPlayerWaypoint();
                SetWaypointOff();
                SetNewWaypoint(alarmLocation.X, alarmLocation.Y);
            });
        }

        public void ConfigReader()
        {
            var data = LoadResourceFile(GetCurrentResourceName(), "config.ini");
            if (Configuration.LoadFromString(data).Contains("CustodyAlarm", "CustodyNames") == true)
            {
                Configuration loaded = Configuration.LoadFromString(data);
                CustodyLocations = new Dictionary<string, Vector3>();
                SoundRadius = loaded["CustodyAlarm"]["AlarmRadius"].FloatValue;
                SoundVolume = loaded["CustodyAlarm"]["AlarmVolume"].FloatValue;
                foreach (string s in loaded["CustodyAlarm"]["CustodyNames"].StringValueArray)
                {
                    if (loaded.Contains(s))
                    {
                        float[] importLocation = loaded[s]["Location"].FloatValueArray;
                        Vector3 location = new Vector3(importLocation[0], importLocation[1], importLocation[2]);
                        CustodyLocations.Add(s, location);
                    }
                    else
                    {
                        ProcessError(true, $"The {s} custody location is unconfigured.");
                    }
                }
                Debug.WriteLine("Custody Locations Loaded. Made by London Studios");
                TriggerEvent("chat:addSuggestion", "/" + loaded["CustodyAlarm"]["Command"].StringValue, "Activate a custody alarm");
                TriggerEvent("chat:addSuggestion", "/" + loaded["CustodyAlarm"]["StopCommand"].StringValue, "Deactivate a custody alarm");
                CustodyCommand(loaded["CustodyAlarm"]["Command"].StringValue);
                StopCommand(loaded["CustodyAlarm"]["StopCommand"].StringValue);
            }
            else
            {
                ProcessError(false);
            }
        }

        private void ProcessError(bool custom, string name = "")
        {
            if (custom == true)
            {
                PlaySoundFrontend(-1, "ERROR", "HUD_AMMO_SHOP_SOUNDSET", true);
                TriggerEvent("chat:addMessage", new
                {
                    color = new[] { 255, 0, 0 },
                    multiline = true,
                    args = new[] { "CustodyAlarm", $"{name}" }
                });
            }
            else
            {
                PlaySoundFrontend(-1, "ERROR", "HUD_AMMO_SHOP_SOUNDSET", true);
                TriggerEvent("chat:addMessage", new
                {
                    color = new[] { 255, 0, 0 },
                    multiline = true,
                    args = new[] { "CustodyAlarm", $"The config has not been configured correctly." }
                });
            }
        }

        private void StopCommand(string name)
        {
            RegisterCommand(name, new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (!AlarmActivated || IsStringNullOrEmpty(PlayingAlarm))
                {
                    ProcessError(true, "There is no alarm active in this area.");
                }
                else
                {
                    TriggerServerEvent("Server:CustodyAlarm", GetEntityCoords(PlayerPedId(), true), PlayingAlarm, true);
                }
            }), false);
        }

        private void CustodyCommand(string name)
        {

            RegisterCommand(name, new Action<int, List<object>, string>((source, args, raw) =>
            {
                Vector3 location = GetEntityCoords(PlayerPedId(), true);
                foreach (var item in CustodyLocations)
                {
                    var key = item.Key;
                    var area = item.Value;
                    if (Vdist(location.X, location.Y, location.Z, area.X, area.Y, area.Z) < SoundRadius)
                    {
                        if (PlayingAlarm == key)
                        {
                            ProcessError(true, "There is already an alarm playing in this area.");
                        }
                        else
                        {
                            if (AlarmActivated)
                            {
                                ProcessError(true, "You have an active custody alarm playing.");
                            }
                            else
                            {
                                TriggerAlarm(key, area);
                            }
                        }
                        break;
                    }
                    else
                    {
                        ProcessError(true, "No custody alarm found to activate.");
                        break;
                    }
                }
            }), false);
        }

        private void TriggerAlarm(string name, Vector3 location)
        {
            Screen.ShowNotification($"You have ~r~activated ~w~the {name} custody alarm.");
            AlarmActivated = true;
            TriggerServerEvent("Server:CustodyAlert", location, name);
            SoundAlarm(location, name);
        }

        private async void SoundAlarm(Vector3 position, string name)
        {
            while (AlarmActivated == true)
            {
                TriggerServerEvent("Server:CustodyAlarm", position, name, false);
                await Delay(7900);
            }
        }
    }
}

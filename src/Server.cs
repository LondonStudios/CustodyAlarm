using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace Server
{
    public class Main : BaseScript
    {
        public Main()
        {
            // Stop = true/false
            EventHandlers["Server:CustodyAlarm"] += new Action<Vector3, string, bool>((alarmLocation, alarmName, stop) =>
            {
                TriggerClientEvent("Client:CustodyAlarm", alarmLocation, alarmName, stop);
            });

            EventHandlers["Server:CustodyAlert"] += new Action<Vector3, string>((alarmLocation, alarmName) =>
            {
                TriggerClientEvent("Client:CustodyAlert", alarmLocation, alarmName);
            });
        }
    }
}

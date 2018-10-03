﻿using System;
using Harmony;
using LmpClient.Extensions;
using LmpClient.VesselUtilities;

namespace LmpClient.Systems.VesselPartSyncCallSys
{
    /// <summary>
    /// Class that maps a message class to a system class. This way we avoid the message caching issues
    /// </summary>
    public class VesselPartSyncCall
    {
        #region Fields and Properties

        public double GameTime;
        public Guid VesselId;

        public uint PartFlightId;
        public string ModuleName;
        public string MethodName;
        
        #endregion

        public void ProcessPartMethodCallSync()
        {
            var vessel = FlightGlobals.fetch.LmpFindVessel(VesselId);
            if (vessel == null || !vessel.loaded) return;

            if (!VesselCommon.DoVesselChecks(VesselId))
                return;

            var part = VesselCommon.FindProtoPartInProtovessel(vessel.protoVessel, PartFlightId);
            if (part != null)
            {
                var module = VesselCommon.FindProtoPartModuleInProtoPart(part, ModuleName);
                if (module != null)
                {
                    if (module.moduleRef != null)
                    {
                        module.moduleRef.GetType().GetMethod(MethodName, AccessTools.all)?.Invoke(module.moduleRef, null);
                    }
                }
            }
        }
    }
}

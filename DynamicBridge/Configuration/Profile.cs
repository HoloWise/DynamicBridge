﻿using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicBridge.Configuration
{
    public class Profile
    {
        [NonSerialized] internal string GUID = Guid.NewGuid().ToString();
        public string Name = "";
        public List<ApplyRule> Rules = [];
        public List<Preset> Presets = [];
        public List<PresetFolder> PresetsFolders = [];
        public string ForcedPreset = null;
        public List<Profile> Subprofiles = [];
        public HashSet<ulong> Characters = [];
        public List<string> Pathes = [];
        public Preset FallbackPreset = new();

        public bool IsGlobal => C.GlobalProfile == this;

        public IEnumerable<Preset> GetPresetsUnion(bool includeGlobal = true)
        {
            foreach(var x in GetPresetsListUnion(includeGlobal))
            {
                foreach(var z in x) yield return z;
            }
        }

        public IEnumerable<List<Preset>> GetPresetsListUnion(bool includeGlobal = true)
        {
            yield return Presets;
            foreach (var x in PresetsFolders) yield return x.Presets;
            if (!IsGlobal && includeGlobal)
            {
                yield return C.GlobalProfile.Presets;
                foreach (var x in C.GlobalProfile.PresetsFolders) yield return x.Presets;
            }
        }
    }
}

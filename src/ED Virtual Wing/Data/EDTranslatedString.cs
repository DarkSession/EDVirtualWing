using ED_Virtual_Wing.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ED_Virtual_Wing.Data
{
    public static class EDTranslatedString
    {
        private static Regex LocalisationStrRegex { get; } = new(@"\$(.*?);");
        private static Regex LocalisationVariableRegex { get; } = new(@"^#(.*?)=(.*?)$");

        public static async ValueTask<string?> Translate(string input, string? returnIfNotAvailable, ApplicationDbContext applicationDbContext)
        {
            if (IsLocalizedString(input))
            {
                string? translatedString = ProcessLocalisationString(input);
                if (!string.IsNullOrEmpty(translatedString))
                {
                    return translatedString;
                }
                else if (!await applicationDbContext.TranslationsPendings.AnyAsync(t => t.NonLocalized == input))
                {
                    applicationDbContext.TranslationsPendings.Add(new TranslationsPending()
                    {
                        NonLocalized = input,
                        LocalizedExample = returnIfNotAvailable ?? string.Empty,
                    });
                    await applicationDbContext.SaveChangesAsync();
                }
            }
            return returnIfNotAvailable;
        }

        public static string? ProcessLocalisationString(string input)
        {
            MatchCollection matches = LocalisationStrRegex.Matches(input);
            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    EDLocalisationString localisationString;
                    string localisationStringValue = match.Groups[1].Value;
                    if (localisationStringValue.Contains(':'))
                    {
                        string[] localosationStringParts = localisationStringValue.Split(':');
                        localisationString = new(localosationStringParts[0].Trim());
                        string variable = localosationStringParts[1].Trim();
                        Match variableMatch = LocalisationVariableRegex.Match(variable);
                        if (variableMatch.Success)
                        {
                            localisationString.AddVariable(variableMatch.Groups[1].Value, variableMatch.Groups[2].Value);
                        }
                    }
                    else
                    {
                        localisationString = new(localisationStringValue);
                    }
                    string? translation = localisationString.TranslatedString;
                    if (!string.IsNullOrEmpty(translation))
                    {
                        input = input.Replace(match.Groups[0].Value, translation);
                        continue;
                    }
                }
                return null;
            }
            if (matches.Count > 0)
            {
                return input;
            }
            return null;
        }

        public static bool IsLocalizedString(string input)
        {
            return !string.IsNullOrEmpty(input) && LocalisationStrRegex.IsMatch(input);
        }
    }

    public class EDLocalisationString
    {
        private static Dictionary<string, string> Localisation { get; } = new()
        {
            { "cmdr_decorate", "CMDR {name}" },

            { "MULTIPLAYER_SCENARIO14_TITLE", "Resource Extraction Site" },
            { "MULTIPLAYER_SCENARIO42_TITLE", "Nav Beacon" },
            { "MULTIPLAYER_SCENARIO77_TITLE", "Resource Extraction Site [Low]" },
            { "MULTIPLAYER_SCENARIO78_TITLE", "Resource Extraction Site [High]" },
            { "MULTIPLAYER_SCENARIO79_TITLE", "Resource Extraction Site [Hazardous]" },
            { "MULTIPLAYER_SCENARIO80_TITLE", "Compromised Navigation Beacon" },

            { "LUASC_Scenario_Warzone_NPC_WarzoneGeneral_Fed", "Federal Captain" },
            { "LUASC_Scenario_Warzone_NPC_WarzoneGeneral_Ind", "Independent Captain" },
            { "LUASC_Scenario_Warzone_NPC_WarzoneCorrespondent", "Warzone Correspondent" },
            { "LUASC_Scenario_Warzone_NPC_SpecOps_A", "Spec Ops Wing Alpha" },
            { "LUASC_Scenario_Warzone_NPC_SpecOps_B", "Spec Ops Wing Beta" },
            { "LUASC_Scenario_Warzone_NPC_SpecOps_D", "Spec Ops Wing Delta" },
            { "LUASC_Scenario_Warzone_NPC_SpecOps_G", "Spec Ops Wing Gamma" },

            { "npc_name_decorate", "{name}" },

            { "POIScenario_Watson_Damaged_Eagle_01_Salvage_Medium", "Distress Beacon" },
            { "POIScenario_Watson_Smugglers_Cache_02_Heist_Medium", "Irregular Markers" },

            { "ShipName_Military_Independent", "System Defence Force" },
            { "ShipName_Military_Federation", "Federal Navy Ship" },
            { "ShipName_PassengerLiner_Cruise", "Cruise Ship" },
            { "ShipName_Police_Alliance", "Allied Police Forces" },
            { "ShipName_Police_Federation", "Federal Security Service" },
            { "ShipName_Police_Independent", "System Authority Vessel" },
            { "ShipName_SearchAndRescue", "Search And Rescue Patrol" },

            { "RolePanel2_unmanned", "Unmanned" },
            { "RolePanel2_crew", "Crew" },

            { "USS", "Unidentified signal source" },
            { "USS_DegradedEmissions", "Degraded Emissions" },
            { "USS_HighGradeEmissions", "High Grade Emissions" },
            { "USS_ThreatLevel", "[Threat {threatLevel}]" },
            { "USS_TradingBeacon", "Trading Beacon" },

            { "Warzone_PointRace_Low", "Conflict Zone [Low Intensity]" },
            { "Warzone_PointRace_Med", "Conflict Zone [Medium Intensity]" },
            { "Warzone_PointRace_High", "Conflict Zone [High Intensity]" },
        };

        public string LocalisationKey { get; }
        public Dictionary<string, string> Variables { get; } = new();
        public EDLocalisationString(string localisationKey)
        {
            LocalisationKey = localisationKey;
        }

        public void AddVariable(string key, string value)
        {
            Variables[key] = value;
        }

        public string? TranslatedString
        {
            get
            {
                if (Localisation.TryGetValue(LocalisationKey, out string? translation))
                {
                    foreach (KeyValuePair<string, string> variable in Variables)
                    {
                        translation = translation.Replace("{" + variable.Key + "}", variable.Value);
                    }
                    return translation;
                }
                return null;
            }
        }
    }
}

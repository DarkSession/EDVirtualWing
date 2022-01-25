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
            { "MULTIPLAYER_SCENARIO14_TITLE", "Resource Extraction Site" },
            { "MULTIPLAYER_SCENARIO42_TITLE", "Nav Beacon" },
            { "MULTIPLAYER_SCENARIO77_TITLE", "Resource Extraction Site [Low]" },
            { "MULTIPLAYER_SCENARIO78_TITLE", "Resource Extraction Site [High]" },
            { "MULTIPLAYER_SCENARIO79_TITLE", "Resource Extraction Site [Hazardous]" },
            { "MULTIPLAYER_SCENARIO80_TITLE", "Compromised Navigation Beacon" },
            { "ShipName_Military_Independent", "System Defence Force" },
            { "ShipName_Police_Independent", "System Authority Vessel" },
            { "ShipName_PassengerLiner_Cruise", "Cruise Ship" },
            { "ShipName_Police_Alliance", "Allied Police Forces" },
            { "cmdr_decorate", "CMDR {name}" },
            { "USS", "Unidentified signal source" },
            { "USS_HighGradeEmissions", "High Grade Emissions" },
            { "POIScenario_Watson_Smugglers_Cache_02_Heist_Medium", "Irregular Markers" },
            { "POIScenario_Watson_Damaged_Eagle_01_Salvage_Medium", "Distress Beacon" },
            { "USS_ThreatLevel", "[Threat {threatLevel}]" },
            { "USS_TradingBeacon", "Trading Beacon" },
            { "RolePanel2_unmanned", "Unmanned" },
            { "USS_DegradedEmissions", "Degraded Emissions" },
            { "RolePanel2_crew", "Crew" },
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

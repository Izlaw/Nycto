namespace Nycto.Helper;

public static class EmojiMapper
{
    public static readonly Dictionary<string, string> Elements = new(StringComparer.OrdinalIgnoreCase)
    {
        { "impact", "<:impact:1526875490319728731> Impact" },
        { "puncture", "<:puncture:1526875497386999888> Puncture" },
        { "slash", "<:slash:1526875501501616158> Slash" },
        { "heat", "<:heat:1526875487945752616> Heat" },
        { "cold", "<:cold:1526875479041376256> Cold" },
        { "electricity", "<:electricity:1526875482966986782> Electricity" },
        { "toxin", "<:toxin:1526875505066770462> Toxin" },
        { "blast", "<:blast:1526875477338492959> Blast" },
        { "corrosive", "<:corrosive:1526875480706515036> Corrosive" },
        { "gas", "<:gas:1526875485621981214> Gas" },
        { "magnetic", "<:magnetic:1526875493842948227> Magnetic" },
        { "radiation", "<:radiation:1526875499228430376> Radiation" },
        { "viral", "<:viral:1526875507008999434> Viral" },
        { "true", "<:true_damage:1526880531927535756> True" },
        { "void", "<:void:1526875508816740464> Void" },
        { "tau", "<:tau:1526875503342915664> Tau" }
    };

    public static readonly Dictionary<string, string> Polarities = new(StringComparer.OrdinalIgnoreCase)
    {
        { "madurai", "<:MaduraiPol:1526946524142112838> Madurai" },
        { "vazarin", "<:VazarinPol:1526946534992642059> Vazarin" },
        { "naramon", "<:NaramonPol:1526946526738518167> Naramon" },
        { "zenurik", "<:ZenurikPol:1526946539774283957> Zenurik" },
        { "unairu", "<:UnairuPol:1526946532673327174> Unairu" },
        { "precept", "<:PenjagaPol:1526946529141588068> Precept" },
        { "umbra", "<:UmbraPol:1526946530937012274> Umbra" },
        { "any", "<:AnyPol:1526946519603740814> Any" },
        { "fusion", "<:KoneksiPol:1526946522154139678> Fusion" },
        { "penjaga", "<:PenjagaPol:1526946529141588068> Penjaga" },
        { "koneksi", "<:KoneksiPol:1526946522154139678> Koneksi" }
    };

    public static string GetPolarityString(string apiPolarity)
    {
        if (string.IsNullOrWhiteSpace(apiPolarity)) return string.Empty;

        if (Polarities.TryGetValue(apiPolarity, out var emojiString))
        {
            return emojiString;
        }

        return char.ToUpperInvariant(apiPolarity[0]) + apiPolarity.Substring(1);
    }

    public static string GetElementString(string apiDamageType)
    {
        if (Elements.TryGetValue(apiDamageType, out var emojiString))
        {
            return emojiString;
        }

        return char.ToUpperInvariant(apiDamageType[0]) + apiDamageType.Substring(1);
    }

    public static string ReplaceEmojiTags(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return text;

        return Regex.Replace(text, @"<(DT|POLARITY)_([A-Z_]+)>", match =>
        {
            var prefix = match.Groups[1].Value;
            var tag = match.Groups[2].Value;

            if (prefix == "DT")
            {
                var elementTag = tag
                    .Replace("_COLOR", "")
                    .Replace("_OUTLINE", "")
                    .Replace("_NO_ADV", "")
                    .ToLowerInvariant();
                
                elementTag = elementTag switch
                {
                    "poison" => "toxin",
                    "fire" => "heat",
                    "freeze" => "cold",
                    "explosion" => "blast",
                    _ => elementTag
                };

                if (Elements.TryGetValue(elementTag, out var fullEmojiString))
                {
                    return fullEmojiString.Split(' ')[0];
                }
            }
            else if (prefix == "POLARITY")
            {
                var polarityTag = tag.ToLowerInvariant() switch
                {
                    "attack" => "madurai",
                    "defense" => "vazarin",
                    "tactic" => "naramon",
                    "power" => "zenurik",
                    "ward" => "unairu",
                    "precept" => "precept",
                    "umbra" => "umbra",
                    "any" => "any",
                    "fusion" => "fusion",
                    _ => tag.ToLowerInvariant()
                };

                if (Polarities.TryGetValue(polarityTag, out var fullEmojiString))
                {
                    return fullEmojiString.Split(' ')[0];
                }
            }

            return "";
        });
    }
}

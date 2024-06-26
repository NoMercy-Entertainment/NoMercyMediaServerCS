﻿using Newtonsoft.Json;

namespace NoMercy.Providers.TMDB.Models.Shared;

public class Certification
{
    [JsonProperty("certifications")] public TmdbCertificationList TmdbCertifications { get; set; } = new();

    public List<TmdbCertificationItem> ToArray()
    {
        List<TmdbCertificationItem> certifications = new();

        var index = 0;
        while (index < TmdbCertifications.Au.Length)
        {
            var y = TmdbCertifications.Au[index];
            certifications.Add(new TmdbCertificationItem
            {
                Certification = y.Certification,
                Meaning = y.Meaning,
                Order = y.Order,
                Iso31661 = "AU"
            });
            index += 1;
        }

        index = 0;
        while (index < TmdbCertifications.Bg.Length)
        {
            var y = TmdbCertifications.Bg[index];
            certifications.Add(new TmdbCertificationItem
            {
                Certification = y.Certification,
                Meaning = y.Meaning,
                Order = y.Order,
                Iso31661 = "BG"
            });
            index += 1;
        }

        index = 0;
        while (index < TmdbCertifications.Br.Length)
        {
            var y = TmdbCertifications.Br[index];
            certifications.Add(new TmdbCertificationItem
            {
                Certification = y.Certification,
                Meaning = y.Meaning,
                Order = y.Order,
                Iso31661 = "BR"
            });
            index += 1;
        }

        index = 0;
        while (index < TmdbCertifications.Caqc.Length)
        {
            var y = TmdbCertifications.Caqc[index];
            certifications.Add(new TmdbCertificationItem
            {
                Certification = y.Certification,
                Meaning = y.Meaning,
                Order = y.Order,
                Iso31661 = "CAQC"
            });
            index += 1;
        }

        index = 0;
        while (index < TmdbCertifications.Ca.Length)
        {
            var y = TmdbCertifications.Ca[index];
            certifications.Add(new TmdbCertificationItem
            {
                Certification = y.Certification,
                Meaning = y.Meaning,
                Order = y.Order,
                Iso31661 = "CA"
            });
            index += 1;
        }

        index = 0;
        while (index < TmdbCertifications.De.Length)
        {
            var y = TmdbCertifications.De[index];
            certifications.Add(new TmdbCertificationItem
            {
                Certification = y.Certification,
                Meaning = y.Meaning,
                Order = y.Order,
                Iso31661 = "DE"
            });
            index += 1;
        }

        index = 0;
        while (index < TmdbCertifications.Dk.Length)
        {
            var y = TmdbCertifications.Dk[index];
            certifications.Add(new TmdbCertificationItem
            {
                Certification = y.Certification,
                Meaning = y.Meaning,
                Order = y.Order,
                Iso31661 = "DK"
            });
            index += 1;
        }

        index = 0;
        while (index < TmdbCertifications.Es.Length)
        {
            var y = TmdbCertifications.Es[index];
            certifications.Add(new TmdbCertificationItem
            {
                Certification = y.Certification,
                Meaning = y.Meaning,
                Order = y.Order,
                Iso31661 = "ES"
            });
            index += 1;
        }

        index = 0;
        while (index < TmdbCertifications.Fi.Length)
        {
            var y = TmdbCertifications.Fi[index];
            certifications.Add(new TmdbCertificationItem
            {
                Certification = y.Certification,
                Meaning = y.Meaning,
                Order = y.Order,
                Iso31661 = "FI"
            });
            index += 1;
        }

        index = 0;
        while (index < TmdbCertifications.Fr.Length)
        {
            var y = TmdbCertifications.Fr[index];
            certifications.Add(new TmdbCertificationItem
            {
                Certification = y.Certification,
                Meaning = y.Meaning,
                Order = y.Order,
                Iso31661 = "FR"
            });
            index += 1;
        }

        index = 0;
        while (index < TmdbCertifications.Gb.Length)
        {
            var y = TmdbCertifications.Gb[index];
            certifications.Add(new TmdbCertificationItem
            {
                Certification = y.Certification,
                Meaning = y.Meaning,
                Order = y.Order,
                Iso31661 = "GB"
            });
            index += 1;
        }

        index = 0;
        while (index < TmdbCertifications.Hu.Length)
        {
            var y = TmdbCertifications.Hu[index];
            certifications.Add(new TmdbCertificationItem
            {
                Certification = y.Certification,
                Meaning = y.Meaning,
                Order = y.Order,
                Iso31661 = "HU"
            });
            index += 1;
        }

        index = 0;
        while (index < TmdbCertifications.In.Length)
        {
            var y = TmdbCertifications.In[index];
            certifications.Add(new TmdbCertificationItem
            {
                Certification = y.Certification,
                Meaning = y.Meaning,
                Order = y.Order,
                Iso31661 = "IN"
            });
            index += 1;
        }

        index = 0;
        while (index < TmdbCertifications.It.Length)
        {
            var y = TmdbCertifications.It[index];
            certifications.Add(new TmdbCertificationItem
            {
                Certification = y.Certification,
                Meaning = y.Meaning,
                Order = y.Order,
                Iso31661 = "IT"
            });
            index += 1;
        }

        index = 0;
        while (index < TmdbCertifications.Kr.Length)
        {
            var y = TmdbCertifications.Kr[index];
            certifications.Add(new TmdbCertificationItem
            {
                Certification = y.Certification,
                Meaning = y.Meaning,
                Order = y.Order,
                Iso31661 = "KR"
            });
            index += 1;
        }

        index = 0;
        while (index < TmdbCertifications.Lt.Length)
        {
            var y = TmdbCertifications.Lt[index];
            certifications.Add(new TmdbCertificationItem
            {
                Certification = y.Certification,
                Meaning = y.Meaning,
                Order = y.Order,
                Iso31661 = "LT"
            });
            index += 1;
        }

        index = 0;
        while (index < TmdbCertifications.My.Length)
        {
            var y = TmdbCertifications.My[index];
            certifications.Add(new TmdbCertificationItem
            {
                Certification = y.Certification,
                Meaning = y.Meaning,
                Order = y.Order,
                Iso31661 = "MY"
            });
            index += 1;
        }

        index = 0;
        while (index < TmdbCertifications.Nl.Length)
        {
            var y = TmdbCertifications.Nl[index];
            certifications.Add(new TmdbCertificationItem
            {
                Certification = y.Certification,
                Meaning = y.Meaning,
                Order = y.Order,
                Iso31661 = "NL"
            });
            index += 1;
        }

        index = 0;
        while (index < TmdbCertifications.No.Length)
        {
            var y = TmdbCertifications.No[index];
            certifications.Add(new TmdbCertificationItem
            {
                Certification = y.Certification,
                Meaning = y.Meaning,
                Order = y.Order,
                Iso31661 = "NO"
            });
            index += 1;
        }

        index = 0;
        while (index < TmdbCertifications.Nz.Length)
        {
            var y = TmdbCertifications.Nz[index];
            certifications.Add(new TmdbCertificationItem
            {
                Certification = y.Certification,
                Meaning = y.Meaning,
                Order = y.Order,
                Iso31661 = "NZ"
            });
            index += 1;
        }

        index = 0;
        while (index < TmdbCertifications.Ph.Length)
        {
            var y = TmdbCertifications.Ph[index];
            certifications.Add(new TmdbCertificationItem
            {
                Certification = y.Certification,
                Meaning = y.Meaning,
                Order = y.Order,
                Iso31661 = "PH"
            });
            index += 1;
        }

        index = 0;
        while (index < TmdbCertifications.Pt.Length)
        {
            var y = TmdbCertifications.Pt[index];
            certifications.Add(new TmdbCertificationItem
            {
                Certification = y.Certification,
                Meaning = y.Meaning,
                Order = y.Order,
                Iso31661 = "PT"
            });
            index += 1;
        }

        index = 0;
        while (index < TmdbCertifications.Ru.Length)
        {
            var y = TmdbCertifications.Ru[index];
            certifications.Add(new TmdbCertificationItem
            {
                Certification = y.Certification,
                Meaning = y.Meaning,
                Order = y.Order,
                Iso31661 = "RU"
            });
            index += 1;
        }

        index = 0;
        while (index < TmdbCertifications.Se.Length)
        {
            var y = TmdbCertifications.Se[index];
            certifications.Add(new TmdbCertificationItem
            {
                Certification = y.Certification,
                Meaning = y.Meaning,
                Order = y.Order,
                Iso31661 = "SE"
            });
            index += 1;
        }

        index = 0;
        while (index < TmdbCertifications.Sk.Length)
        {
            var y = TmdbCertifications.Sk[index];
            certifications.Add(new TmdbCertificationItem
            {
                Certification = y.Certification,
                Meaning = y.Meaning,
                Order = y.Order,
                Iso31661 = "SK"
            });
            index += 1;
        }

        index = 0;
        while (index < TmdbCertifications.Th.Length)
        {
            var y = TmdbCertifications.Th[index];
            certifications.Add(new TmdbCertificationItem
            {
                Certification = y.Certification,
                Meaning = y.Meaning,
                Order = y.Order,
                Iso31661 = "TH"
            });
            index += 1;
        }

        index = 0;
        while (index < TmdbCertifications.Us.Length)
        {
            var y = TmdbCertifications.Us[index];
            certifications.Add(new TmdbCertificationItem
            {
                Certification = y.Certification,
                Meaning = y.Meaning,
                Order = y.Order,
                Iso31661 = "US"
            });
            index += 1;
        }

        return certifications;
    }
}

public class TmdbCertificationList
{
    [JsonProperty("AU")] public TmdbCertificationItem[] Au { get; set; } = [];
    [JsonProperty("BG")] public TmdbCertificationItem[] Bg { get; set; } = [];
    [JsonProperty("BR")] public TmdbCertificationItem[] Br { get; set; } = [];
    [JsonProperty("CA-QC")] public TmdbCertificationItem[] Caqc { get; set; } = [];
    [JsonProperty("CA")] public TmdbCertificationItem[] Ca { get; set; } = [];
    [JsonProperty("DE")] public TmdbCertificationItem[] De { get; set; } = [];
    [JsonProperty("ES")] public TmdbCertificationItem[] Es { get; set; } = [];
    [JsonProperty("FI")] public TmdbCertificationItem[] Fi { get; set; } = [];
    [JsonProperty("FR")] public TmdbCertificationItem[] Fr { get; set; } = [];
    [JsonProperty("GB")] public TmdbCertificationItem[] Gb { get; set; } = [];
    [JsonProperty("HU")] public TmdbCertificationItem[] Hu { get; set; } = [];
    [JsonProperty("IN")] public TmdbCertificationItem[] In { get; set; } = [];
    [JsonProperty("KR")] public TmdbCertificationItem[] Kr { get; set; } = [];
    [JsonProperty("LT")] public TmdbCertificationItem[] Lt { get; set; } = [];
    [JsonProperty("NL")] public TmdbCertificationItem[] Nl { get; set; } = [];
    [JsonProperty("NZ")] public TmdbCertificationItem[] Nz { get; set; } = [];
    [JsonProperty("PH")] public TmdbCertificationItem[] Ph { get; set; } = [];
    [JsonProperty("RU")] public TmdbCertificationItem[] Ru { get; set; } = [];
    [JsonProperty("SK")] public TmdbCertificationItem[] Sk { get; set; } = [];
    [JsonProperty("US")] public TmdbCertificationItem[] Us { get; set; } = [];
    [JsonProperty("DK")] public TmdbCertificationItem[] Dk { get; set; } = [];
    [JsonProperty("IT")] public TmdbCertificationItem[] It { get; set; } = [];
    [JsonProperty("MY")] public TmdbCertificationItem[] My { get; set; } = [];
    [JsonProperty("NO")] public TmdbCertificationItem[] No { get; set; } = [];
    [JsonProperty("SE")] public TmdbCertificationItem[] Se { get; set; } = [];
    [JsonProperty("TH")] public TmdbCertificationItem[] Th { get; set; } = [];
    [JsonProperty("PT")] public TmdbCertificationItem[] Pt { get; set; } = [];
}

public class TmdbCertificationItem
{
    [JsonProperty("certification")] public string Certification { get; set; } = string.Empty;
    [JsonProperty("meaning")] public string Meaning { get; set; } = string.Empty;
    [JsonProperty("order")] public int Order { get; set; }
    public string Iso31661 { get; set; } = string.Empty;
}
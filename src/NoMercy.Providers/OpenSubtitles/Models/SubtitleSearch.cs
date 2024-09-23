using System.Xml.Serialization;
using Newtonsoft.Json;

namespace NoMercy.Providers.OpenSubtitles.Models;

public class SubtitleSearch
{
    [XmlElement("methodCall")]
    public MethodCall MethodCall { get; set; }
}

public class MethodCall
{
    [XmlElement("methodName")]
    public string MethodName { get; set; }

    [XmlElement("params")]
    public SubtitleSearchParams Params { get; set; }
}

public class SubtitleSearchParams
{
    [XmlElement("param")]
    public SubtitleSearchParam[] Param { get; set; }
}

public class SubtitleSearchParam
{
    [XmlElement("value")]
    public SubtitleSearchParamValue Value { get; set; }
}

public class SubtitleSearchParamValue
{
    [XmlElement("string", IsNullable = true)]
    public string String { get; set; }

    [XmlElement("array", IsNullable = true)]
    public SubtitleSearchArray Array { get; set; }
}

public class SubtitleSearchArray
{
    [XmlElement("data")]
    public SubtitleSearchData Data { get; set; }
}

public class SubtitleSearchData
{
    [XmlElement("value")]
    public SubtitleSearchDataValue Value { get; set; }
}

public class SubtitleSearchDataValue
{
    [XmlElement("struct")]
    public SubtitleSearchStruct Struct { get; set; }
}

public class SubtitleSearchStruct
{
    [XmlElement("member")]
    public SubtitleSearchMember[] Member { get; set; }
}

public class SubtitleSearchMember
{
    public SubtitleSearchMember()
    {
    }

    public SubtitleSearchMember(string name, SubtitleSearchMemberValue value)
    {
        Name = name;
        Value = value;
    }

    [XmlElement("name")]
    public string Name { get; set; }

    [XmlElement("value")]
    public SubtitleSearchMemberValue Value { get; set; }
}

public class SubtitleSearchMemberValue
{
    [XmlElement("string")]
    public string String { get; set; }
}
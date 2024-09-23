using NoMercy.Providers.OpenSubtitles.Models;

namespace NoMercy.Providers.OpenSubtitles.Client;

public class OpenSubtitlesClient : OpenSubtitlesBaseClient
{
    public async Task<OpenSubtitlesClient> Login()
    {
        Login login = new()
        {
            MethodName = "LogIn",
            Params =
            [
                new LoginParam
                {
                    Value = new LoginValue
                    {
                        String = ""
                    }
                },
                new LoginParam
                {
                    Value = new LoginValue
                    {
                        String = ""
                    }
                },
                new LoginParam
                {
                    Value = new LoginValue
                    {
                        String = "dut"
                    }
                },
                new LoginParam
                {
                    Value = new LoginValue
                    {
                        // String = ApiInfo.UserAgent
                        String = "VLSub"
                    }
                }
            ]
        };

        var x = await  Post<Login, LoginResponse>("", login);
        AccessToken = x?.Params?.Param?.Value?.Struct?.Member.FirstOrDefault(member => member.Name == "token")?.Value?.String;
        
        return this;
    }
    
    public async Task<SubtitleSearchResponse?> SearchSubtitles(string query, string language)
    {
        SubtitleSearch searchResponse = new()
        {
            MethodCall = new MethodCall
            {
                MethodName = "SearchSubtitles",
                Params = new SubtitleSearchParams
                {
                    Param =
                    [
                        new SubtitleSearchParam
                        {
                            Value = new SubtitleSearchParamValue
                            {
                                String = AccessToken!
                            }
                        },
                        new SubtitleSearchParam
                        {
                            Value = new SubtitleSearchParamValue
                            {
                                Array = new SubtitleSearchArray
                                {
                                    Data = new SubtitleSearchData
                                    {
                                        Value = new SubtitleSearchDataValue
                                        {
                                            Struct = new SubtitleSearchStruct
                                            {
                                                Member =
                                                [
                                                    new SubtitleSearchMember(name: "sublanguageid", value: new SubtitleSearchMemberValue
                                                    {
                                                        String = language
                                                    }),
                                                    new SubtitleSearchMember(name: "query", value: new SubtitleSearchMemberValue
                                                    {
                                                        String = query
                                                    }),
                                                ]
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    ]
                }
            }
        };

        return await Post<SubtitleSearch, SubtitleSearchResponse>("", searchResponse);
    }
}
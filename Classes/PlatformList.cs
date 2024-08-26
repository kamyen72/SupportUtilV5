using DupRecRemoval.Classes;

namespace SupportUtil.Classes
{
    public class PlatformList
    {
        public List<NewPlatform> platforms {  get; set; }

        public void LoadPlatforms()
        {
            platforms = new List<NewPlatform>();

            // create platform page for HL
            NewPlatform pf1 = new NewPlatform();
            pf1.Platform = "HL";
            pf1.APIDomain = "https://hkgp.ug396-api.com";
            pf1.PlatformText = "Under HL Series";
            pf1.APID = "";
            pf1.AgentName = "";
            pf1.CompanyCode = "";

            platforms.Add(pf1);

            // create platform page for TM
            NewPlatform pf2 = new NewPlatform();
            pf2.Platform = "TM";
            pf2.APIDomain = "https://hkgp.3mplay.net";
            pf2.PlatformText = "Under TM Series";
            pf2.APID = "";
            pf2.AgentName = "";
            pf2.CompanyCode = "";

            platforms.Add(pf2);

            // create platform page for King
            NewPlatform pf3 = new NewPlatform();
            pf3.Platform = "King";
            pf3.APIDomain = "https://hkgp.ace-api.com";
            pf3.PlatformText = "Under King Series";
            pf3.APID = "";
            pf3.AgentName = "";
            pf3.CompanyCode = "";

            platforms.Add(pf3);

            // create platform page for TM2
            NewPlatform pf4 = new NewPlatform();
            pf4.Platform = "TM2";
            pf4.APIDomain = "https://hkgp.3mplay.net";
            pf4.PlatformText = "Under TM Series";
            pf4.APID = "";
            pf4.AgentName = "";
            pf4.CompanyCode = "";

            platforms.Add(pf4);

        }
    }
}

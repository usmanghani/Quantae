namespace Quantae.DataModel
{
    public class GrammarRole : QuantaeObject
    {
        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>
        /// The name of the role.
        /// </value>
        public string RoleName { get; set; }

        /// <summary>
        /// Used as a hint.
        /// </summary>
        /// <value>
        /// The role definition.
        /// </value>
        public string RoleDefinition { get; set; }
    }

    public class GrammarRoleHandle : QuantaeObjectHandle<GrammarRole>
    {
        public GrammarRoleHandle(GrammarRole role) : base(role)
        {
        }
    }

    public static class RoleNames
    {
        public const string Unknown = "Unknown";
        public const string Faail = "Faail"; // doer
        public const string Mubtada = "Mubtada"; // subject
        public const string Khabar = "Khabar"; // predicate
        public const string SifaMarfoo = "SifaMarfoo"; // adjective
        public const string SifaMansoob = "SifaMansoob";
        public const string SifaMajroor = "SifaMajroor";
        public const string TawkeedMarfoo = "TawkeedMarfoo";// emphasis
        public const string TawkeedMansoob = "TawkeedMansoob"; // emphasis
        public const string TawkeedMajroor = "TawkeedMajroor"; // emphasis
        public const string BadalMarfoo = "BadalMarfoo"; // Appositive
        public const string BadalMansoob = "BadalMansoob"; // Appositive
        public const string BadalMajroor = "BadalMajroor"; // Appositive
        public const string MaatoofMarfoo = "MaatoofMarfoo";
        public const string MaatoofMansoob = "MaatoofMansoob";
        public const string MaatoofMajroor = "MaatoofMajroor";
        public const string MafoolBihi = "MafoolBihi"; // Direct Object
        public const string NaaibFaail = "NaaibFaail"; // passive doer
        public const string IsmKana = "IsmKana"; // subject of kana
        public const string KhabarKana = "KhabarKana"; // predicate of kana
        public const string IsmInna = "IsmInna"; // subject of inna
        public const string KhabarInna = "KhabarInna"; // predicate of inna
        public const string MafoolMutlaq = "MafoolMutlaq";
        public const string MafoolLiajlihi = "MafoolLiajlihi";
        public const string MafoolMaahoo = "MafoolMaahoo";
        public const string DharfZaman = "DharfZaman";
        public const string DharfMakan = "DharfMakan";
        public const string Haal = "Haal";
        public const string Tamyeez = "Tamyeez";
        public const string Mustasna = "Mustasna";
        public const string Munada = "Munada";
        public const string Mudhaf = "Mudhaf";
        public const string MudhafIlaih = "MudhafIlaih";
        public const string IsmMosool = "IsmMosool";
        public const string JumlaSila = "JumlaSila";
        public const string Mosoof = "Mosoof";
        public const string Muakkad = "Muakkad";
        public const string MubdalBihi = "MubdalBihi";
        public const string MaatoofAlaihi = "MaatoofAlaihi";
    }
}
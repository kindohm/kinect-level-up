using System.Collections.Generic;

namespace KinectLevelUp.Speech.Services
{
    public class NuiGrammar
    {
        public enum NuiGrammarType
        {
            Basic,
            Srgs
        }

        public NuiGrammar()
        {
            this.GrammarType = NuiGrammarType.Basic;
            this.Items = new List<NuiGrammarItem>();
        }

        public NuiGrammarType GrammarType { get; set; }
        public string Name { get; set; }
        public List<NuiGrammarItem> Items { get; set; }
    }
}

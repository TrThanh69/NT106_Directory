using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignInLogIn
{
    public class translationsResponse
    {
        public string normalizedSource { get; set; }
        public string displaySource { get; set; }
        public List<translations> translations { get; set; }
    }
    public class translations
    {
        public string normalizedTarget { get; set; }
        public string displayTarget { get; set; }
        public string posTag { get; set; }
        public double confidence { get; set; }
        public string prefixWord { get; set; }
        public List<backTranslation> backTranslations { get; set; }
    }
    public class backTranslation
    {
        public string normalizedText { get; set; }
        public string displayText { get; set; }
        public double numExamples { get; set; }
        public double frequencyCount { get; set; }
    }

    // Dictionary examples
    public class examplesresponse
    {
        public List<example> examples { get; set; }
        public string normalizedSource { get; set; }
        public string normalizedTarget { get; set; }
    }
    public class example
    {
        public string sourcePrefix { get; set; }
        public string sourceSuffix { get; set; }
        public string sourceTerm { get; set; }
        public string targetPrefix { get; set; }
        public string targetSuffix { get; set; }
        public string targetTerm { get; set; }
    }
}

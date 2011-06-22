using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae
{
    public class GrammarRole
    {
        public TopicComponent Component { get; set; }

        /// <summary>
        /// Used as a hint.
        /// </summary>
        public string RoleDefinition { get; set; }
    }
}

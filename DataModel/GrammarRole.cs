using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel
{
    public class GrammarRole : QuantaeObject<ulong>
    {
        public Role Component { get; set; }

        /// <summary>
        /// Used as a hint.
        /// </summary>
        public string RoleDefinition { get; set; }
    }

    public class GrammarRoleHandle : QuantaeObjectHandle<ulong> { }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Engine
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISentenceFilter
    {
        /// <summary>
        /// Determines whether [is sentence valid] [the specified profile].
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <param name="sentence">The sentence.</param>
        /// <returns>
        ///   <c>true</c> if [is sentence valid] [the specified profile]; otherwise, <c>false</c>.
        /// </returns>
        bool IsSentenceValid(UserProfile profile, Sentence sentence);
    }
}

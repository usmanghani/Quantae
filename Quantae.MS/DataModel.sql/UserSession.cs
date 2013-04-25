using System;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Quantae.DataModel.Sql
{
    /// <summary>
    /// Doesn't derive from QuantaeObject since it uses its own ID type.
    /// </summary>
    public class UserSession
    {
        /// <summary>
        /// Gets or sets the token. This acts as a key. Supposed to be unique. Let's see :)
        /// If two users get created at exactly the same time 
        /// (well within the delta e.g if DateTime.UtcNow returns the same value for both users)
        /// and they have exactly the same password then their tokens will be the same.
        /// What does that entail?
        /// Every time we create a user we check to see if another use has the same salt (pun intended)
        /// If yes, then we just recreate the salt. Hopefull in the mean time DateTime.UtcNow has
        /// changed its value.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        [BsonId]
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the creation timestamp. This is the timestamp
        /// when this session was created.
        /// Can be used for expiry and a lot of other things in the future.
        /// </summary>
        /// <value>
        /// The timestamp.
        /// </value>
        public DateTime CreationTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the expiration timestamp.
        /// This is set at a max of two weeks if the user asks us to remember him/her.
        /// </summary>
        /// <value>
        /// The expiration timestamp.
        /// </value>
        public DateTime ExpirationTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        /// <value>
        /// The user ID.
        /// </value>
        public string UserID { get; set; }

        /// <summary>
        /// Gets or sets the user profile.
        /// </summary>
        /// <value>
        /// The user profile.
        /// </value>
        public UserProfileHandle UserProfile { get; set; }

        /// <summary>
        /// Gets or sets the sentence batch.
        /// This is in-memory only. Doesn't go to the db.
        /// </summary>
        /// <value>
        /// The sentence batch.
        /// </value>
        [BsonIgnore]
        public List<Sentence> SentenceBatch { get; set; }

        public UserSession()
        {
            this.SentenceBatch = new List<Sentence>();
        }
    }
}
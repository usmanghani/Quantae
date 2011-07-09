using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;
using Quantae.Repositories;

namespace Quantae.Engine
{
    public class SessionManager
    {
        private object SessionStoreLock = new object();

        private Dictionary<string, UserSession> SessionStore = new Dictionary<string, UserSession>();

        public static SessionManager Current = new SessionManager();

        public static void Load()
        {
            var sessions = Repositories.Repositories.Sessions.GetAllItems();
            foreach (var session in sessions)
            {
                session.SentenceBatch = SentenceOperations.LoadBatch(session.UserProfile);
                Current.SessionStore.Add(session.Token, session);
            }
        }

        public UserSession CreateOrReturnSession(UserSession session)
        {
            lock (SessionStoreLock)
            {
                if (this.SessionStore.ContainsKey(session.Token))
                {
                    return this.SessionStore[session.Token];
                }

                CreateSession(session);
                return session;
            }
        }

        public bool SessionExists(string token)
        {
            lock (SessionStoreLock)
            {
                return this.SessionStore.ContainsKey(token);
            }
        }

        public UserSession GetSession(string token)
        {
            lock (SessionStoreLock)
            {
                EnsureSessionExists(token);
                return this.SessionStore[token];
            }
        }

        public UserSession GetSessionById(UserProfile profile)
        {
            lock (SessionStoreLock)
            {
                return this.SessionStore.Where(kvp => kvp.Value.UserID.Equals(profile.UserID)).FirstOrDefault().Value;
            }
        }

        public void UpdateSession(string token, UserSession session)
        {
            lock (SessionStoreLock)
            {
                EnsureSessionExists(token);

                Repositories.Repositories.Sessions.Save(session);

                this.SessionStore[token] = session;
            }
        }

        public bool RemoveSession(string token)
        {
            lock (SessionStoreLock)
            {
                EnsureSessionExists(token);

                Repositories.Repositories.Sessions.Remove(SessionQueries.GetSessionByToken(token));

                return this.SessionStore.Remove(token);
            }
        }

        public void ReloadSentenceBatch(string token, List<Sentence> sentences)
        {
            lock (SessionStoreLock)
            {
                EnsureSessionExists(token);
                this.SessionStore[token].SentenceBatch = sentences;
                
            }
        }

        public void AddToSentenceBatch(string token, params Sentence[] sentences)
        {
            lock (SessionStoreLock)
            {
                EnsureSessionExists(token);
                if (this.SessionStore[token].SentenceBatch == null)
                {
                    this.SessionStore[token].SentenceBatch = new List<Sentence>();
                }

                this.SessionStore[token].SentenceBatch.AddRange(sentences);

            }
        }

        private void EnsureSessionExists(string token)
        {
            if (!SessionStore.ContainsKey(token))
            {
                throw new SessionNotFoundException(token);
            }
        }

        private void CreateSession(UserSession session)
        {
            this.SessionStore.Add(session.Token, session);
            Repositories.Repositories.Sessions.Save(session);
        }
    }
}

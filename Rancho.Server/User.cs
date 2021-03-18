using Rancho.Protocol;

namespace Rancho.Server
{
    internal sealed class User
    {
        private readonly Session _session;

        public User(Session session)
        {
            _session = session;
        }

        public User(string username, Session session)
        {
            Username = username;
            _session = session;
        }

        public string Username { get; private set; }

        public bool IsAuth => Program.Server.Users.ContainsKey(_session.Id);

        public void SendAsync(Message message)
        {
            _session.SendAsync(message.Write());
        }

        public bool Authenticate(string username)
        {
            if (Program.Server.Users.ContainsKey(_session.Id))
            {
                return false;
            }

            Username = username;
            Program.Server.Users.Add(_session.Id, this);

            return true;
        }
    }
}
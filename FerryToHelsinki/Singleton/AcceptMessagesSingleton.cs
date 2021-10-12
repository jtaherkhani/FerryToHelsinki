namespace FerryToHelsinki.Singleton
{
    public class AcceptMessagesSingleton
    {
        private volatile bool acceptMessages;

        public bool AcceptMessages { get => acceptMessages; set => acceptMessages = value; }
    }
}

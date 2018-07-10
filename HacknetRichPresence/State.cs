namespace HacknetRichPresence {
    public class State {
        public readonly string Description;

        public readonly string ImageName;

        public readonly string SmallImageName;

        public State(string description, string imageName, string smallImageName = null) {
            Description = description;
            ImageName = imageName;
            SmallImageName = smallImageName;
        }
    }
}

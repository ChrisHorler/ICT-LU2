namespace BackendAPI.WebApi.Models {
    public class User {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }

    public class World {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int OwnerId { get; set; }
        public int SizeX { get; set; }
        public int SizeY { get; set; }
    }

    public class WorldObject {
        public int Id { get; set; }
        public int WorldId { get; set; }
        public string Type { get; set; } = string.Empty;
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; set; }
    }

    public class SharedWorld {
        public int Id { get; set; }
        public int WorldId { get; set; }
        public int SharedWithUserId { get; set; }
    }
}


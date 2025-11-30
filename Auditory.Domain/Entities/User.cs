using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Auditory.Domain.Entities;

public class User
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("email")]
    public string Email { get; set; } = null!;

    [BsonElement("username")]
    public string Username { get; set; } = null!;

    [BsonElement("passwordHash")]
    public byte[] PasswordHash { get; set; } = null!;

    [BsonElement("passwordSalt")]
    public byte[] PasswordSalt { get; set; } = null!;

    [BsonElement("role")]
    public string Role { get; set; } = "User";

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}

public class RefreshToken
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("userId")]
    public ObjectId UserId { get; set; }

    [BsonElement("token")]
    public string Token { get; set; } = null!;

    [BsonElement("expiresAt")]
    public DateTime ExpiresAt { get; set; }

    [BsonElement("revokedAt")]
    public DateTime? RevokedAt { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("createdByIp")]
    public string? CreatedByIp { get; set; }
}
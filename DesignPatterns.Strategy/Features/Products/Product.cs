using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DesignPatterns.Strategy.Features.Products;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public int Id { get; set; }
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    [DataType("decimal(18,2)")]
    public decimal Price { get; set; }
    
}
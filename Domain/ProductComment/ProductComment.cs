namespace Domain.ProductComment;

public class ProductComment
{
    public Guid Id { get; private set; }
    public Guid WeaponId { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public static ProductComment Create(Guid weaponId, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Comment content cannot be null or whitespace.", nameof(content));
        }

        return new ProductComment
        {
            Id = Guid.NewGuid(),
            WeaponId = weaponId,
            Content = content.Trim(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };
    }

    public void Update(string newContent)
    {
        if (string.IsNullOrWhiteSpace(newContent))
        {
            throw new ArgumentException("Comment content cannot be null or whitespace.", nameof(newContent));
        }
        Content = newContent.Trim();
        UpdatedAt = DateTime.UtcNow;
    }
}
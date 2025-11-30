using System;
using System.Collections.Generic;
using Domain.Weapons;

namespace Api.DTOs;

public record ProductCommentDto(
    Guid Id,
    string Content,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record CreateCommentRequest(string Content);
public record UpdateCommentRequest(string Content);

public record WeaponDetailedResponse(
    Guid Id,
    string Name,
    string Manufacturer,
    string Model,
    string Caliber,
    string SerialNumber,
    decimal Price,
    WeaponStatus Status,
    WeaponCategory Category,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    IReadOnlyCollection<ProductCommentDto> Comments 
);
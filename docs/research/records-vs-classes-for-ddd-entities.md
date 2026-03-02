# Records vs Classes for DDD Entities in C#

> Research compiled 2026-03-02. Context: evaluating whether `ChatMessage` (and future domain entities) should use `sealed record` instead of `sealed class`.

## Current Codebase Pattern

| Role | Type Used | Example |
|------|-----------|---------|
| Value Objects | `readonly record struct` | `RoomId`, `AuthorName`, `MessageText` |
| Aggregate Root / Entity | `sealed class` | `ChatMessage` |
| DTOs / Requests / Responses | `sealed record` | `SendMessageRequest`, `GetMessagesResponse` |

Persistence is via **Dapper** (not EF Core). Entities are constructed through factory methods (`Create`, `Rehydrate`) with private constructors. No mutation occurs after construction.

---

## Arguments AGAINST Records for Entities

### 1. Microsoft Official Docs — Records are not appropriate for entity types

> "Not all data models work well with value equality. For example, Entity Framework Core depends on reference equality to ensure that it uses only one instance of an entity type for what is conceptually one entity. **For this reason, record types aren't appropriate for use as entity types.**"

— [Microsoft: Records (C# fundamentals)](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/records)

Key points from the docs:
- Records synthesize **structural (value) equality** — two instances with identical field values are considered equal.
- Records synthesize `ToString()` that prints all property values.
- Records enable `with` expressions for nondestructive mutation.
- Microsoft explicitly warns against records as EF Core entity types due to the reference equality requirement of the change tracker.

### 2. Vladimir Khorikov (Enterprise Craftsmanship) — Records are for Value Objects, not Entities

> "C# records are a good fit for DDD value objects — they provide the same semantics and do all the hard lifting behind the scenes."

— [C# Records as DDD Value Objects](https://enterprisecraftsmanship.com/posts/csharp-records-value-objects/)

Khorikov's reasoning:
- **Identity equality** is the defining trait of entities. Two `ChatMessage` instances with the same data but different lifecycle histories are *not* the same thing.
- **Structural equality** is the defining trait of value objects. Two `RoomId("general")` are interchangeable.
- Records give structural equality by default, which is **semantically wrong for entities**.
- Records' conciseness advantage vanishes when you add private constructors and factory methods (which you need for encapsulation/validation).
- Records don't give fine-grained control over equality components (e.g., excluding a field, custom precision for floating-point comparison, collection-aware equality).

> "The concept of **identifier equality** refers to entities, whereas the concept of **structural equality** refers to value objects. Entities possess inherent identity while value objects don't."

— [Entity vs Value Object: The Ultimate List of Differences](https://enterprisecraftsmanship.com/posts/entity-vs-value-object-the-ultimate-list-of-differences/)

Additional points from Khorikov on entity vs value object:
- Entities **live in continuum** — they have a history and lifecycle.
- Value objects have **zero lifespan** — they are snapshots, interchangeable if field-equal.
- Value objects should always **belong to an entity**; they don't live independently.
- Entities are (usually) mutable; value objects **must be immutable**.
- Guideline: **prefer value objects over entities** — push business logic into value objects, let entities be thin wrappers.

### 3. `with` Expressions Bypass Validation

If `ChatMessage` is a record, anyone can write:

```csharp
var copy = message with { Text = MessageText.Create("bypassed validation?") };
```

This creates a new instance without going through `Create` or `Rehydrate`, potentially bypassing timestamp validation or other invariants. With a `sealed class`, the only way to construct is through the factory methods.

---

## Arguments FOR Records for Entities

### 1. Oskar Dudycz (event-driven.io) — Records as entities in event-sourced / functional architectures

Dudycz models entities as records in both [Java](https://event-driven.io/en/how_to_effectively_compose_your_business_logic/) and [F#](https://event-driven.io/en/writing_and_testing_business_logic_in_fsharp/) samples, using sealed interfaces/unions with record implementations for each state:

```fsharp
type BankAccount =
    | Initial
    | Open of {| Id: AccountId; Balance: decimal; Version: int64 |}
    | Closed of {| Id: AccountId; Version: int64 |}
```

```java
sealed public interface ShoppingCart {
    record PendingShoppingCart(UUID id, UUID clientId, ProductItems productItems)
        implements ShoppingCart {}
    record ConfirmedShoppingCart(UUID id, UUID clientId, ProductItems productItems,
        OffsetDateTime confirmedAt) implements ShoppingCart {}
    // ...
}
```

Key arguments:
- **Immutable state transitions are the point.** In event sourcing: `(oldState, event) → newState`. Records with `with` are natural here.
- **State machines modeled as sealed records** make illegal states unrepresentable at compile time.
- **Structural equality is useful for testing** — assert entire entity snapshots without custom comparers.
- **No ORM change tracker** = the main objection (EF Core reference equality) doesn't apply.

### 2. Scott Wlaschin ("Domain Modeling Made Functional", fsharpforfunandprofit.com)

The [entire book](https://pragprog.com/titles/swdddf/domain-modeling-made-functional/) and [blog series](https://fsharpforfunandprofit.com/posts/designing-with-types-intro/) model all domain types — including entities — as F# records.

Key arguments:
- **Types are documentation.** The record's shape *is* the specification.
- **"Make illegal states unrepresentable"** — records + discriminated unions enforce business rules at compile time that OOP enforces at runtime.
- In F#, *everything* is a record. Entity vs Value Object is expressed by the presence of an `Id` field, not by the language construct used.
- No hidden mutable state to reason about.

### 3. StackOverflow / Community Pragmatic Camp

From the [highly-upvoted answer on record vs class vs struct](https://stackoverflow.com/questions/64816714/when-to-use-record-vs-class-vs-struct):

- Records are fine for entities when you **don't use EF Core's change tracker**.
- You can **override `Equals` to use Id-based equality** on a record, getting concise syntax + correct identity semantics.
- Records work well for **immutable, unidirectional data flow** patterns.

### 4. Summary of Benefits

| Benefit | Explanation |
|---------|-------------|
| Immutability by default | Prevents accidental mutation; thread-safe; aligns with functional DDD |
| Concise syntax | Less boilerplate; `with` expressions for state transitions |
| Structural equality for testing | `Assert.Equal(expected, actual)` on entire entities without custom comparers |
| `ToString()` for free | Better debugging/logging out of the box |
| State machine modeling | Sealed records + pattern matching = compile-time enforcement |
| Works without EF Core | The main objection only applies to ORM change tracking |

---

## Analysis: What Applies to This Codebase?

| Factor | Our situation | Implication |
|--------|---------------|-------------|
| ORM | Dapper (no change tracker) | EF Core objection **does not apply** |
| Mutation | None — entities are immutable after construction | Records are a natural fit |
| Equality usage | `ChatMessage` is never compared for equality | Structural vs reference equality is moot |
| Construction | Private ctor + factory methods | Works identically on records and classes |
| Testing | Tests assert individual properties | Would benefit from structural equality |
| `with` bypass risk | Moderate — could skip `ValidateTimestamp` | Mitigated by team discipline; `Create`/`Rehydrate` remain the documented API |

---

## Recommendation

**Keep the current `sealed class` for existing entities** — changing it is philosophical, not practical. It doesn't fix a bug or enable a feature, and the codebase is small and consistent.

**For new entities going forward**, consider `sealed record` with the existing factory method pattern:

```csharp
public sealed record ChatMessage
{
    public Guid Id { get; }
    public RoomId RoomId { get; }
    public AuthorName AuthorName { get; }
    public MessageText Text { get; }
    public DateTimeOffset CreatedAtUtc { get; }

    private ChatMessage(Guid id, RoomId roomId, AuthorName authorName,
        MessageText text, DateTimeOffset createdAtUtc)
    {
        Id = id;
        RoomId = roomId;
        AuthorName = authorName;
        Text = text;
        CreatedAtUtc = createdAtUtc;
    }

    public static ChatMessage Create(...) { ... }
    public static ChatMessage Rehydrate(...) { ... }

    // Optional: override equality to use Id if identity semantics are needed
    public bool Equals(ChatMessage? other) => other is not null && Id == other.Id;
    public override int GetHashCode() => Id.GetHashCode();
}
```

The industry trajectory — functional DDD, event sourcing, immutable-first design, and C#'s own language evolution (records, pattern matching, future discriminated unions) — favors records. The "classes for entities" rule was shaped by ORM constraints, not a universal DDD truth.

---

## Sources

1. [Microsoft — Records (C# fundamentals)](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/records)
2. [Khorikov — C# Records as DDD Value Objects](https://enterprisecraftsmanship.com/posts/csharp-records-value-objects/)
3. [Khorikov — Entity vs Value Object: The Ultimate List of Differences](https://enterprisecraftsmanship.com/posts/entity-vs-value-object-the-ultimate-list-of-differences/)
4. [Dudycz — How to Effectively Compose Your Business Logic (Java)](https://event-driven.io/en/how_to_effectively_compose_your_business_logic/)
5. [Dudycz — Writing and Testing Business Logic in F#](https://event-driven.io/en/writing_and_testing_business_logic_in_fsharp/)
6. [Wlaschin — Designing with Types (F# for Fun and Profit)](https://fsharpforfunandprofit.com/posts/designing-with-types-intro/)
7. [Wlaschin — Domain Modeling Made Functional (book)](https://pragprog.com/titles/swdddf/domain-modeling-made-functional/)
8. [StackOverflow — When to use record vs class vs struct](https://stackoverflow.com/questions/64816714/when-to-use-record-vs-class-vs-struct)
9. [Microsoft — EF Core Constructor Binding](https://learn.microsoft.com/en-us/ef/core/modeling/constructors)
10. [Dudycz — Immutable Value Objects](https://event-driven.io/en/immutable_value_objects/)

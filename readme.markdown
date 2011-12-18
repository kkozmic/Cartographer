# What is Cartographer?

Cartographer is a convention driven .NET library for mapping objects with strong emphasis on ease of use, extensibility, traceability and structure.

## What problem does it solve again?

Trying to build a loosely coupled application in a strongly typed, type oriented language like C# means you'll inevitably have multiple places in code where you need to map from one type to another. `Order` to `OrderDto`, `OrderDto` to `OrderViewModel` etc.

This tends to be a very mundane, repetitive and boring code - one that's a perfect candidate for automatization, and that's where Cartographer steps in.

Using a set of out of the box conventions, as well as custom ones, supplied by you it compiles code that does the mapping for you, so that instead of writing:

```csharp
var orderDto = new OrderDto
               {
               	OrderLines = Array.ConvertAll(order.OrderLines, ol => new OrderLineDto
               	                                                      {
               	                                                      	ItemId = ol.ItemId,
               	                                                      	ItemName = ol.ItemName
               	                                                      }),
               	// usually quite a few other properties like that
               };
```

you write

```csharp
var orderDto = mapper.Convert<OrderDto>(order);
```

And cartographer compiles that to something that's pretty much like the code above (with addition of some error checking so that if something goes wrong, you'll know exactly what and why*).




*This part is not implemented yet, but will be part of the first public release.
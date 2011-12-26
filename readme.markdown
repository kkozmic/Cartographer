Cartographer
========

# What is Cartographer?

Cartographer is a convention driven .NET library for mapping objects to objects with strong emphasis on ease of use, extensibility, traceability and structure.

## What problem does it solve again?

Trying to build a loosely coupled application in a strongly typed, class oriented language like C# means you'll inevitably have multiple places in code where you need to map from one type to another. `Order` to `OrderDto`, `OrderDto` to `OrderViewModel` etc.

This tends to be a very mundane, repetitive and boring code - one that's a perfect candidate for automation, and that's where Cartographer steps in.

## How does it work?

Using a set of out-of-the-box conventions, as well as custom ones, supplied by you, it compiles code that does the mapping for you, so that you write:

```java
var orderDto = mapper.Convert<OrderDto>(order);
```

instead of:

```java
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

# Why Cartographer?

Cartographer is quite unique among libraries solving similar problems, as it offers the following useful features.

## Simplicity

Being simple and natural to use is one of the main goals of Cartographer. You will be able to get things done with it just after reading this readme file.

## Pre-cached, convention-driven scanning

Cartographer uses conventions to find pairs of types to map, rather than requiring you to specify each mapping pair by hand.*

## Traceability

Code that does things based on conventions can be hard to debug. Cartographer offers two featuers that make it a non-issue:

 - Using `IMappingDescriptor` interface Cartographer will output exactly the steps it uses to map each type pair in a C#-like pseudocode. Inspecting it, you'll be able to quickly spot invalid configuration. Moreover, if you're doing approval testing it will help you ensure no refactoring breaks your mapping. Here's a simple example

```
Mapping for CartographerTests.Types.Order => CartographerTests.Types.OrderDto
(target = new OrderDto())
(target.OrderLines = MapCollection(source.OrderLines, context))
Mapping for CartographerTests.Types.OrderLine => CartographerTests.Types.OrderLineDto
(target = new OrderLineDto())
(target.ItemId = source.ItemId)
(target.ItemName = source.ItemName)
```

 - Detailed and helpful exceptions. If something does go wrong, Cartographer will do its best to tell you exactly where the problem is, what the problem is, and, as much as it can, how to fix it.

## Compiled mapping

Cartographer compiles the mapping rather than interpreting them each time using Reflection. Compilation of the mappings can happen in batches(\*) and asynchronously(\*) to ensure optimal startup performance of your app.

## Structure

Cartographer requires you to follow certain structure when working with it. This will help you keep your mapping code clean and easy to maintain, and ensure that each member of the team creates mapping code in the same way.*

## Extensibility

Cartographer offers extension points for every step of the way, so you can quite easily extend and customize the way it works.


*\*This part is not implemented yet, but will be part of the 1.0 release.*

### Acknowledgements

Cartographer would not exist without the following tools that inspired and influenced some of its design and features (in no particular order):

- [Castle DynamicProxy](http://docs.castleproject.org/Tools.DynamicProxy.ashx)
- [Castle Windsor](http://docs.castleproject.org/Windsor.MainPage.ashx)
- [Autofac](http://autofac.org)
- [Automapper](http://automapper.org/)
- [NSubstitute](http://nsubstitute.github.com/)
- [FluentNHibernate](http://fluentnhibernate.org/)
- [NHibernate](http://nhforge.org/Default.aspx)
- [ApprovalTests](http://approvaltests.sourceforge.net/)
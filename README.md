# SharpCachingAop NuGet Package

## Overview

The CachingAop NuGet package provides a powerful framework for implementing caching using aspect-oriented programming (AOP) principles in C#. It allows developers to easily apply caching to methods in their applications, improving performance and reducing redundant computations.

## Features

- **CacheableAttribute**: Mark methods as cacheable using this attribute, specifying cache keys and other caching parameters.
- **CachingConfigurationBlock**: Configure caching settings globally and for individual cached items.
- **AsyncCachingInterceptor**: Intercept method invocations and handle caching asynchronously, ensuring optimal performance.
- **ISerializationProvider**: Define custom serialization providers for object serialization operations.
- **DefaultSerializationProvider**: Default implementation of the ISerializationProvider interface using Newtonsoft.Json for serialization.
- **Dependency Injection Support**: Register caching and AOP services in the service collection with ease, allowing for seamless integration into existing applications.

## Components

### CacheableAttribute

This attribute is used to mark methods as cacheable. It allows developers to specify cache keys and other caching parameters such as duration and whether to return a deep copy of cached data.

### CachingConfigurationBlock

This class represents a configuration block for caching settings, including deadlock timeout and item-specific cache settings. It allows developers to configure caching settings globally and for individual cached items.

### AsyncCachingInterceptor

The AsyncCachingInterceptor class implements the IAsyncInterceptor interface and provides methods for intercepting synchronous and asynchronous method invocations. It checks if the method is marked with the CacheableAttribute and handles caching asynchronously, ensuring optimal performance.

### ISerializationProvider and DefaultSerializationProvider

ISerializationProvider defines an interface for object serialization operations, allowing developers to define custom serialization providers if needed. DefaultSerializationProvider provides a default implementation using Newtonsoft.Json for serialization.

### Dependency Injection Support

CachingDiManager contains extension methods for registering caching and AOP services in the service collection. Developers can easily configure caching and serialization providers and register interceptors for dependency injection.

## Usage

1. **Install the NuGet Package**: Install the CachingAop NuGet package in your project using NuGet Package Manager or Package Manager Console.

2. **Mark Methods as Cacheable**: Mark methods that you want to cache with the CacheableAttribute, specifying cache keys and other caching parameters as needed.

3. **Configure Caching Settings**: Optionally, configure caching settings globally and for individual cached items using the CachingConfigurationBlock.

4. **Register Services**: Register caching and AOP services in the service collection using CachingDiManager. Customize caching and serialization providers if needed.

5. **Apply Dependency Injection**: Inject caching services and interceptors into your application components using dependency injection.

6. **Enjoy Improved Performance**: Enjoy improved performance and reduced redundant computations by leveraging caching in your application.

## Example

```csharp
using CachingAop.Attributes;
using CachingAop.Caching;
using CachingAop.Configuration;
using CachingAop.Serialization;
using CachingAop.DI;

// Mark method as cacheable
[Cacheable("MethodName")]
public string GetData(int id)
{
    // Method implementation
}

// Register services in service collection
services.SetSharpCachingAopRegistration();

// Inject caching services and interceptors
services.AddInterceptedSingleton<IMyService, MyService, AsyncCachingInterceptor>();

# Powerumc.AspNetCore.Core

Easyily build up ours Micro Services Architecture(MSA) and Domain Driven Development(DDD).


## Micro services architecture

### RegisterAttribute

```csharp
public interface ITestService
{
    Task<List<string>> GetStringsAsync();
}

[Register(typeof(ITestService))] // Regist components
public class TestService : ITestService
{
    public async Task<List<string>> GetStringsAsync()
    {
        return await Task.FromResult(new List<string>()
        {
            "Junil Um",
            "Hanji Jung"
        });
    }
}
```


## Domain Driven Development

### Implements ValueObject
```csharp
public class Author : ValueObject
{
    public string Title { get; }
    public string Url { get; }

    public Author(string title, string url)
    {
        Title = title;
        Url = url;
    }
}
```

### Implements DomainEvent

```csharp
public class RssFeedCreateDomainEvent : DomainEvent
{
    public Author Author { get; }

    public RssFeedCreateDomainEvent(Author author)
    {
        this.Author = author;
    }
}
```


### Implements DomainEventHandler
```csharp
public class RssFeedCreateDomainEventHandler : IDomainEventHandler<RssFeedCreateDomainEvent>
{
    private readonly TraceId _traceId;
    private readonly ILogger<RssFeedCreateDomainEventHandler> _logger;
    private readonly IRssFeedsRepository _rssFeedsRepository;

    public RssFeedCreateDomainEventHandler(TraceId traceId,
        ILogger<RssFeedCreateDomainEventHandler> logger,
        IRssFeedsRepository rssFeedsRepository)
    {
        _traceId = traceId;
        _logger = logger;
        _rssFeedsRepository = rssFeedsRepository;
    }

    public async Task Handle(RssFeedCreateDomainEvent @event)
    {
        _logger.Log(_traceId, @event.ToJson());

        await _rssFeedsRepository.CreateAsync(new RssFeed
        {
            Title = @event.Author.Title,
            Url = @event.Author.Url,
            CreateDate = DateTime.UtcNow,
            ModifyDate = DateTime.UtcNow
        });
    }
}
```


### Publishing EventBus

```csharp
public interface IRssFeedsService
{
    Task CreateAsync(RssFeedCreateRequest request);
}

[Register(typeof(IRssFeedsService))]
public class RssFeedsService : IRssFeedsService
{
    private readonly TraceId _traceId;
    private readonly ILogger<RssFeedsService> _logger;
    private readonly IEventBus _eventBus;

    public RssFeedsService(TraceId traceId,
        ILogger<RssFeedsService> logger,
        IRssFeedsRepository repository,
        IEventBus eventBus)
    {
        _traceId = traceId;
        _logger = logger;
        _repository = repository;
        _eventBus = eventBus;
    }

    public async Task CreateAsync(Domain.Requests.V1.RssFeedCreateRequest request)
    {
        Guard.ThrowIfNull(request, nameof(request));
        Guard.ThrowIfNullOrWhitespace(request.Title, nameof(request.Title));
        Guard.ThrowIfNullOrWhitespace(request.Url, nameof(request.Url));
        
        _logger.Log(_traceId, request.ToJson());
        
        _eventBus.Publish(new RssFeedCreateDomainEvent(new Author(request.Title, request.Url)));

        await Task.CompletedTask;
    }
}
```
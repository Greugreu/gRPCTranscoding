using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using TestGfCat.Data;
using TestGfCat.Repository;

namespace TestGfCat.Services;

public class MessagesApiService : MessagesService.MessagesServiceBase
{
    private readonly MessagesRepository _messagesRepository;

    public MessagesApiService(MessagesRepository messagesRepository)
    {
        _messagesRepository = messagesRepository;
    }

    public override Task<ListReply> ListMessages(Empty request, ServerCallContext context)
    {
        List<Message> items = _messagesRepository.GetMessages();

        var listReply = new ListReply();
        var messagesList = items.Select(item => new AdventMessageReply()
        {
            Id = item.Id,
            Message = item.MessageContent
        }).ToList();

        listReply.AdventMessages.AddRange(messagesList);

        return Task.FromResult(listReply);
    }

    public override Task<AdventMessageReply> GetMessage(GetAdventMessageRequest request, ServerCallContext context)
    {
        Message message = _messagesRepository.GetSingleMessage(request.Id);

        return Task.FromResult(new AdventMessageReply()
        {
            Id = message.Id,
            Message = message.MessageContent
        });
    }

    public override Task<AdventMessageReply> CreateMessage(CreateAdventMessageRequest request,
        ServerCallContext context)
    {
        Message messageToCreate = new Message()
        {
            Id = request.Id,
            MessageContent = request.Message
        };

        Message createdMessage = _messagesRepository.AddMessage(messageToCreate);

        AdventMessageReply reply = new AdventMessageReply()
        {
            Id = createdMessage.Id,
            Message = createdMessage.MessageContent
        };

        return Task.FromResult(reply);
    }

    public override Task<AdventMessageReply> UpdateMessage(UpdateAdventMessageRequest request,
        ServerCallContext context)
    {
        Message existingMessage = _messagesRepository.GetSingleMessage(request.Id);

        if (existingMessage == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Message Not Found"));
        }

        Message messageToUpdate = new Message()
        {
            Id = request.Id,
            MessageContent = request.Message
        };
        Message updatedMessage = _messagesRepository.UpdateMessage(messageToUpdate);
        
        var reply = new AdventMessageReply()
        {
            Id = updatedMessage.Id,
            Message = updatedMessage.MessageContent
        };

        return Task.FromResult(reply);
    }

    public override Task<Empty> DeleteMessage(DeleteAdventMessageRequest request, ServerCallContext context)
    {
        Message existingMessage = _messagesRepository.GetSingleMessage(request.Id);

        if (existingMessage == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Message Not Found"));
        }
        
        _messagesRepository.DeleteMessage(request.Id);

        return Task.FromResult(new Empty());
    }
}
using TestGfCat.Data;

namespace TestGfCat.Repository;

public class MessagesRepository
{
    private readonly MessageData _messageData;

    public MessagesRepository(MessageData messageData)
    {
        _messageData = messageData;
    }

    public List<Message> GetMessages()
    {
        return _messageData.Messages;
    }

    public Message GetSingleMessage(int id)
    {
        return _messageData.Messages.FirstOrDefault(a => a.Id == id) ?? throw new InvalidOperationException();
    }

    public Message AddMessage(Message message)
    {
        _messageData.Messages.Add(new Message(){Id = message.Id, MessageContent = message.MessageContent});
        return message;
    }
    
    public Message UpdateMessage(Message message)
    {
        Message oldMessage = _messageData.Messages.FirstOrDefault(a => a.Id == message.Id) ?? throw new InvalidOperationException();
        oldMessage.MessageContent = message.MessageContent;
        return oldMessage;
    }

    public void DeleteMessage(int id)
    {
        Message message = _messageData.Messages.FirstOrDefault(a => a.Id == id) ?? throw new InvalidOperationException();
        _messageData.Messages.Remove(message);
    }
}
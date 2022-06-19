# Financial Chat Code Challenge

The goal of this exercise is to create a simple browser-based chat application using .NET.

This application should allow several users to talk in a chatroom and also to get stock quotes
from an API using a specific command.

## Mandatory Features
- [x] Allow registered users to log in and talk with other users in a chatroom.
- [x] Allow users to post messages as commands into the chatroom with the following format
  /stock=stock_code
- [x] Create a decoupled bot that will call an API using the stock_code as a parameter
  (https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv, here aapl.us is the
  stock_code)
- [x] The bot should parse the received CSV file and then it should send a message back into
  the chatroom using a message broker like RabbitMQ. The message will be a stock quote
  using the following format: “APPL.US quote is $93.42 per share”. The post owner will be
  the bot.
- [x] Have the chat messages ordered by their timestamps and show only the last 50
  messages.
- [] Unit test the functionality you prefer.

### The Chat
The Web UI is made with Blazor WebAssembly, following the tutorial from Microsoft Docs: [Tutorial: Build a Blazor Server chat app](https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-tutorial-build-blazor-server-chat-app)

The code from the example is modified to communicate to an API that host a SignalR hub and to handle messages being saved to a database and sending commands to the bot.

### The Bot
The Blazor Web App communicates with an Web API that saves new messages to the chat in an SQLite database.

The Web API interprets if the message is a bot command and sends an command through a kafka topic.

The bot is listening to this specific topic, and communicates with stooq.com to get the information requested. Once it parsed the message, it send an event to a kafka topic for the Web API to receive.

The Web API will receive the event and send the message to all clients of the SignalR Hub.

### Docker support
I added a docker-compose.yaml file to get kafka up and running.
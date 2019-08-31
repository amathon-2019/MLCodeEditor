using Prism.Events;

namespace MLCodeEditor.Messages
{
    class ThemeMessage : PubSubEvent<string>
    {
    }

    class fontColorThemeMessage : PubSubEvent<string>
    {
    }
     class syntaxThemeMessage : PubSubEvent<string>
    {
    }

    class rightPanelTalkMessage : PubSubEvent
    {
    }

    class onAzureMLWorking : PubSubEvent
    {
    }

    class onAzureMLEnded : PubSubEvent<string>
    {
    }
}
